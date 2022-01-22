

class TuioCursor : TuioEntity
{
    public TuioCursor(int id, float x, float y) :base(id,  x, y)
    {
        state = TuioState.CLICK_DOWN;
        previousState = TuioState.CLICK_DOWN;
    }

    public bool isClick()
    {
        return previousState == TuioState.CLICK_DOWN && state == TuioState.CLICK_UP;
    }

    public bool isDrag()
    {
        return previousState == TuioState.DRAG;
    }

    public bool isLongClick()
    {
        return previousState == TuioState.LONG_CLICK && state == TuioState.CLICK_UP;
    }
}
