namespace server.Game.Board.Dice;

public enum DiceFaces
{
    NONE,
    SUN,
    WIND,
    THUNDER,
    QUESTIONMARK,
    CLOUD,
    RAIN
}

public static class DiceFacesMethods
{
    public static string ToString( DiceFaces diceFace)
    {
        string? res = Enum.GetName(typeof(DiceFaces), diceFace);
        if (res == null) return "NONE";
        return res;
    }

    public static DiceFaces ToDiceFace(string diceFace)
    {
        try
        {
            return (DiceFaces)Enum.Parse(typeof(DiceFaces), diceFace);
        }
        catch (Exception)
        {
            return DiceFaces.NONE;
        }
    }
}
