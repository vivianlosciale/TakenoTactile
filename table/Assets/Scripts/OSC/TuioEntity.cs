using UnityEngine;

abstract class TuioEntity
{
    protected TuioState previousState;
    protected TuioState state;
    private readonly int id;
    public int Id { get => id; }
    private Position position;
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
        position = new Position(x, y);
    }

    public void updateCoordinates(Vector2 newPosition)
    {
        
        if (Vector2.Distance(newPosition, position.TUIOPosition) > 0.01f)
        {
            State = TuioState.DRAG;
            position.TUIOPosition = newPosition;
        }
    }
}
