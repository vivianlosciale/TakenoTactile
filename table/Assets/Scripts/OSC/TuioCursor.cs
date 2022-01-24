

public class TuioCursor : TuioEntity
{
    public TuioCursor(int id, float x, float y) :base(id,  x, y)
    {
    }

    public bool isClick()
    {
        return previousState == TuioState.CLICK_DOWN && state == TuioState.CLICK_UP;
    }

    public bool isLongClick()
    {
        return previousState == TuioState.LONG_CLICK && state == TuioState.CLICK_UP;
    }

    public override string ToString()
    {
        return $"detection numero :{Id} clic: {isClick()} drag: {isDrag()} longclic: {isLongClick()} {position}\n";
    }
}
