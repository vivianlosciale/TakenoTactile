namespace server.Utils.Game;

public enum UpgradeType
{
    None,
    Fencing,
    Fertilizer,
    WatterTank,
}


public static class UpgradeTypeMethods
{
    public static string ToString(UpgradeType upgradeType)
    {
        string? res = Enum.GetName(typeof(UpgradeType), upgradeType);
        if (res == null) return "None";
        return res;
    }

    public static UpgradeType ToUpgradeType(string upgradeType)
    {
        try
        {
            return (UpgradeType)Enum.Parse(typeof(UpgradeType), upgradeType);
        }
        catch (Exception)
        {
            return UpgradeType.None;
        }
    }
}