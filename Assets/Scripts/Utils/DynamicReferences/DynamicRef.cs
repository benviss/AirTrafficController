using UnityEngine;

public static class DynamicRef
{
    public static DynamicRef<T> SearchById<T>(string id)
    {
        return new IdDynamicRef<T>(id);
    }

    public static DynamicRef<T> SearchByType<T>()
    {
        return new TypeDynamicRef<T>();
    }

    private class IdDynamicRef<T> : DynamicRef<T>
    {
        private readonly string _id;

        public IdDynamicRef(string id)
        {
            _id = id;
        }

        public override bool HasValue
        {
            get
            {
                T reference;
                return App.Instance && App.Instance.DynamicRefs.GetById(_id, out reference);
            }
        }

        public override T Value
        {
            get
            {
                T reference = default(T);
                if (App.Instance) App.Instance.DynamicRefs.GetById(_id, out reference);
                return reference;
            }
        }
    }

    private class TypeDynamicRef<T> : DynamicRef<T>
    {
        public override bool HasValue
        {
            get
            {
                T reference;
                return App.Instance && App.Instance.DynamicRefs.GetByType(out reference);
            }
        }

        public override T Value
        {
            get
            {
                T reference = default(T);
                if (App.Instance) App.Instance.DynamicRefs.GetByType(out reference);
                return reference;
            }
        }
    }
}

public abstract class DynamicRef<TValue>
{
    private readonly WaitUntil _wait;

    protected DynamicRef()
    {
        _wait = new WaitUntil(() => HasValue);
    }

    public virtual CustomYieldInstruction Wait
    {
        get { return _wait; }
    }

    public abstract bool HasValue { get; }

    public abstract TValue Value { get; }
}