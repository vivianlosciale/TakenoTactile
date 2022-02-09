using System;

public enum CardTypes
{
    None,
    Panda,
    Land,
    Bamboo,
}


public static class CardTypesMethods
{
    public static string ToString(CardTypes cardType)
    {
        string? res = Enum.GetName(typeof(CardTypes), cardType);
        if (res == null) return "None";
        return res;
    }

    public static CardTypes ToPowers(string cardType)
    {
        try
        {
            return (CardTypes)Enum.Parse(typeof(CardTypes), cardType);
        }
        catch (Exception)
        {
            return CardTypes.None;
        }
    }
}