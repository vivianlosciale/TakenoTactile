namespace server.Utils.Game;

public class PositionDto
{
    private const char Separator = ',';
    
    public readonly int I;
    public readonly int J;

    private PositionDto(int i, int j)
    {
        I = i;
        J = j;
    }

    public static PositionDto ToPosition(string value)
    {
        string[] seq = value.Split(Separator);
        return new PositionDto(int.Parse(seq[0]), int.Parse(seq[1]));
    }

    public static string ToString(int i, int j)
    {
        return i.ToString() + Separator + j;
    }

    public override string ToString()
    {
        return ToString(I, J);
    }
}