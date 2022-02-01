using UnityEngine;
using UnityEngine.Events;

class OSCCursorEvent : OSCEvent
{
    [SerializeField]
    protected UnityEvent OnClick;
    [SerializeField]
    protected UnityEvent OnLongClick;

    public override void RunFunction(TuioEntity tuio)
    {
        if (tuio is TuioCursor)
        {
            if (tuio.isDrag())
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
            if (((TuioCursor)tuio).isClick())
            {
                OnClick.Invoke();
            }
            if (((TuioCursor)tuio).isLongClick())
            {
                OnLongClick.Invoke();
            }
        }
    }
}
