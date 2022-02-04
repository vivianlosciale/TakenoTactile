namespace server.Utils.Game;

public enum Powers
{
    MoveFarmer,
    MovePanda,
    PickCard,
    PlaceTile,
}


public static class PowersMethods
{
    public static string ToString(Powers diceFace)
    {
        string? res = Enum.GetName(typeof(Powers), diceFace);
        if (res == null) return "PickCard";
        return res;
    }

    public static Powers ToPowers(string power)
    {
        try
        {
            return (Powers)Enum.Parse(typeof(DiceFaces), power);
        }
        catch (Exception)
        {
            return Powers.PickCard;
        }
    }
}