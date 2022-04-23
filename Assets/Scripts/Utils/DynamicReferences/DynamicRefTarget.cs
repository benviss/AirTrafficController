using System.Collections;
using UnityEngine;

public class DynamicRefTarget : MonoBehaviour, IDynamicRefTarget
{
    [SerializeField] private string _id;

    public string Id
    {
        get { return _id; }
    }

    public Component Ref;

    public object Target
    {
        get { return Ref; }
    }

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        while (Ref == null) yield return null;
#endif
        App.Instance.DynamicRefs.Register(this);
        yield break;
    }

    private void OnDestroy()
    {
        if (App.Instance) App.Instance.DynamicRefs.Unregister(this);
    }
}