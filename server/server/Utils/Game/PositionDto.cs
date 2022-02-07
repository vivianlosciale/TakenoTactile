namespace server.Utils.Game;

public class PositionDto
{
    public readonly int I;
    public readonly int J;

    private PositionDto(int i, int j)
    {
        I = i;
        J = j;
    }

    public static PositionDto ToPosition(string value)
    {
        Console.WriteLine("Position string value: " + value);
        string[] seq = value.Split(',');
        return new PositionDto(int.Parse(seq[0]), int.Parse(seq[1]));
    }

    public static string ToString(int i, int j)
    {
        return i.ToString() + ',' + j;
    }

    public override string ToString()
    {
        return ToString(I, J);
    }
}