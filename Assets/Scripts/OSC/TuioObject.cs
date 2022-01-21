class TuioObject : TuioEntity
{
    public TuioObject(int id, float x, float y) : base(id, x, y)
    {
        state = TuioState.CLICK_DOWN;
        previousState = TuioState.CLICK_DOWN;
    }

    public bool isDrag()
    {
        return previousState == TuioState.DRAG;
    }
    public bool isOnTable()
    {
        return previousState != TuioState.CLICK_UP;
    }
}