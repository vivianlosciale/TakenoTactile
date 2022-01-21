abstract class TuioEntity
{
    protected TuioState previousState;
    protected TuioState state;
    private float xCoord;
    private float yCoord;
    private readonly int id;
    public int Id { get => id; }
    protected float XCoord { get => xCoord; }
    protected float YCoord { get => yCoord; }
    public TuioState State
    {
        get => state;
        set
        {
            previousState = state;
            state = value;
        }
    }

    public TuioEntity(int id, float x, float y)
    {
        this.id = id;
        xCoord = x;
        yCoord = y;
    }

    public void updateCoordinates(float xCoord, float yCoord)
    {
        if (this.xCoord != xCoord || this.yCoord != yCoord)
        {
            State = TuioState.DRAG;
            this.xCoord = xCoord;
            this.yCoord = yCoord;
        }
    }
}
