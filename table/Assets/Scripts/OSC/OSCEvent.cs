using UnityEngine;
using UnityEngine.Events;

public class OSCEvent : MonoBehaviour
{
    public UnityEvent OnClickDown;
    public UnityEvent OnClickUp;
    public UnityEvent OnClick;
    public UnityEvent OnDrag;

    public void RunFunction(TuioEntity tuio)
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
        if( tuio is TuioCursor && ((TuioCursor)tuio).isClick())
        {
            OnClick.Invoke();
        }
    }
}
