using System;
using System.Collections.Generic;

public class DynamicRefService
{
    private readonly Dictionary<string, IDynamicRefTarget> _refsById = new Dictionary<string, IDynamicRefTarget>();

    private readonly Dictionary<Type, List<IDynamicRefTarget>> _refsByType = new Dictionary<Type, List<IDynamicRefTarget>>();

    public bool GetById<T>(string id, out T referencedItem)
    {
        if (_refsById.TryGetValue(id, out var dynamicRefTarget))
        {
#if DEVELOPER_TOOLS
            Log.Assert(dynamicRefTarget.Target is T, "Could not cast reference");
#endif
            referencedItem = (T)dynamicRefTarget.Target;
            return true;
        }
        
        referencedItem = default(T);
        return false;
    }

    public bool GetByType<T>(out T referencedItem)
    {
        if (_refsByType.TryGetValue(typeof(T), out var dynamicReferences))
        {
#if DEVELOPER_TOOLS
            Log.Assert(dynamicReferences != null, "Could not find DynamicRef {0}", typeof(T));
            Log.Assert(dynamicReferences.Count <= 1, "Multiple copies of DynamicRef {0}", typeof(T));
#endif

            if (dynamicReferences.Count < 1)
            {
                referencedItem =  default(T);
                return false;
            }
#if DEVELOPER_TOOLS
            Log.Assert(dynamicReferences[0].Target is T, "Could not cast reference");
#endif
            referencedItem = (T)dynamicReferences[0].Target;
            return true;
        }

        referencedItem =  default(T);
        return false;
    }


    public void Register(IDynamicRefTarget dynamicRefTarget)
    {
        //add by id
        if (!string.IsNullOrEmpty(dynamicRefTarget.Id))
        {
            _refsById[dynamicRefTarget.Id] = dynamicRefTarget;
        }

        //add by type (adds all base types)
        var types = GetAllBaseTypes(dynamicRefTarget.Target.GetType());
        while (types.MoveNext())
        {
            var refType = types.Current;
            if (!_refsByType.TryGetValue(refType, out var dynamicReferences))
            {
                dynamicReferences = new List<IDynamicRefTarget>();
                _refsByType.Add(refType, dynamicReferences);
            }
            dynamicReferences.Add(dynamicRefTarget);
        }

    }

    private IEnumerator<Type> GetAllBaseTypes(Type type)
    {
        Type baseType = type;
        while (baseType != null)
        {
            yield return baseType;
            baseType = baseType.BaseType;
        }

        foreach (var iType in type.GetInterfaces())
        {
            yield return iType;
        }
    }
    
    public void Unregister(IDynamicRefTarget dynamicRefTarget)
    {
        //remove by id
        if (!string.IsNullOrEmpty(dynamicRefTarget.Id))
        {
            _refsById.Remove(dynamicRefTarget.Id);
        }

        //remove by type
        var types = GetAllBaseTypes(dynamicRefTarget.Target.GetType());
        while (types.MoveNext())
        {
            var refType = types.Current;
            if (_refsByType.TryGetValue(refType, out var dynamicReferences))
            {
                dynamicReferences.Remove(dynamicRefTarget);
            }
        }
    }
}