using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class OSCEvent : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent OnClickDown;
    [SerializeField]
    protected UnityEvent OnClickUp;
    [SerializeField]
    protected UnityEvent OnDrag;
    [SerializeField]
    protected UnityEvent OnCollisionEnter;
    [SerializeField]
    protected UnityEvent OnCollisionExit;

    protected List<TuioEntity> detections = new List<TuioEntity>();

    public abstract void RunFunction(TuioEntity tuio);
}
