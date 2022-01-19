using System;

class TuioCursor
{
    private float xCoord;
    private float yCoord;
    private int id;
    private bool alreadyUpdate;
    private CursorState previousState;
    private CursorState state;
    public int Id { get => id; set => id = value; }

    public CursorState State
    {
        get => state;
        set
        {
            previousState = state;
            state = value;
        }
    }
    public float XCoord { get => xCoord; }
    public float YCoord { get => yCoord; }
    public bool AlreadyUpdate { get => alreadyUpdate; }



    public TuioCursor(int id, float x, float y)
    {
        this.id = id;
        xCoord = x;
        yCoord = y;
        state = CursorState.CLICK_DOWN;
        state = CursorState.CLICK_DOWN;
        alreadyUpdate = false;
    }

    public Boolean isClick()
    {
        return previousState == CursorState.CLICK_DOWN && state == CursorState.CLICK_UP;
    }

    public Boolean isDrag()
    {
        return previousState == CursorState.DRAG && state == CursorState.CLICK_UP;
    }

    public Boolean isLongClick()
    {
        return previousState == CursorState.LONG_CLICK && state == CursorState.CLICK_UP;
    }

    public void updateCoordinates(float xCoord, float yCoord)
    {
        if (this.xCoord != xCoord || this.yCoord != yCoord)
        {
            alreadyUpdate = true;
            previousState = state;
            state = CursorState.DRAG;
            this.xCoord = xCoord;
            this.yCoord = yCoord;
        }
    }
}
