using System.Collections.Generic;
using UnityEngine;

class OSCObjectEvent : OSCEvent
{

    public override void RunFunction(TuioEntity tuio)
    {

        if (tuio is TuioObject)
        {
            if (tuio.IsDrag())
            {
                OnDrag.Invoke();
            }
            if (tuio.State == TuioState.CLICK_DOWN)
            {
                OnClickDown.Invoke();
            }
            if (tuio.State == TuioState.CLICK_UP)
            {
                OnClickUp.Invoke();
            }
            if (!detections.Contains(tuio))
            {
                OnCollisionEnter.Invoke();
                detections.Add(tuio);
            }
        }
    }
}
