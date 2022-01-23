using UnityEngine;

abstract class TuioEntity
{
    protected TuioState previousState;
    protected TuioState state;
    private readonly int id;
    private readonly Position pos;
    public int Id { get => id; }
    public Position position { get => pos; }
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
        pos = new Position(x, y);
    }

    public bool isDrag()
    {
        return previousState == TuioState.DRAG;
    }

    public void updateCoordinates(Vector2 newPosition)
    {
        
        if (Vector2.Distance(newPosition, position.TUIOPosition) > 0.01f || state == TuioState.DRAG)
        {
            State = TuioState.DRAG;
            position.TUIOPosition = newPosition;
        }
    }
}
