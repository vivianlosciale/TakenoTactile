using System;

class TuioObject : TuioEntity
{
    private float angle;
    private int value;
    public TuioObject(int id, float x, float y, float angle, int value) : base(id, x, y)
    {
        state = TuioState.CLICK_DOWN;
        previousState = TuioState.CLICK_DOWN;
        this.angle = angle;
        this.value = value;
    }
    public void updateAngle(float angle)
    {
        this.angle = angle;
    }

    public bool isOnTable()
    {
        return state != TuioState.CLICK_UP;
    }
    public override string ToString()
    {
        return $"Id : {Id} onTable: {isOnTable()} drag: {isDrag()}  {position} value: {value} angle: {angle}\n";
    }


}