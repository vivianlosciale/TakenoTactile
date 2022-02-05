namespace server.Game.Board.Field;

public class Position
{
    public static readonly RelativePosition[] Neighbors =
    {
        RelativePosition.TopRight,
        RelativePosition.Right,
        RelativePosition.BottomRight,
        RelativePosition.BottomLeft,
        RelativePosition.Left,
        RelativePosition.TopLeft
    };

    public static RelativePosition Opposite(RelativePosition position)
    {
        return Neighbors[((int)position+(Neighbors.Length/2))%Neighbors.Length];
    }
    
    public readonly int I;
    public readonly int J;

    public Position(int i, int j)
    {
        I = i;
        J = j;
    }

    public Position GetPositionAt(RelativePosition relative)
    {
        switch (relative)
        {
            case RelativePosition.TopRight:
                return new Position(I + 1, J + 1);
            case RelativePosition.Right:
                return new Position(I, J + 1);
            case RelativePosition.BottomRight:
                return new Position(I - 1, J);
            case RelativePosition.BottomLeft:
                return new Position(I - 1, J -1);
            case RelativePosition.Left:
                return new Position(I, J - 1);
            default:
                return new Position(I + 1, J);
        }
    }
}