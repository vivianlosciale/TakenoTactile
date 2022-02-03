using System;

public enum MessageQuery
{
    // COMMON
    None,
    Ping,
    AcceptConnection,
    StartGame,
    PickCard,

    // PLAYERS
    PlayerBroadcast,
    PlayerConnection,
    GameIsFull,
    //les actions du joueur : lancer le dé
    ValidateObjective, //nom de la carte, on demande l'autorisation
    RollDice, // on donne un entier
    ChooseTile, //TODO
    FinishTurn,

    // TABLE
    TableConnection,
    APlayerJoined,
}

public static class QueryMethods
{
    public static string ToString( MessageQuery query)
    {
        string? res = Enum.GetName(typeof(MessageQuery), query);
        if (res == null) return "None";
        return res;
    }

    public static MessageQuery ToQuery(string query)
    {
        try
        {
            return (MessageQuery)Enum.Parse(typeof(MessageQuery), query);
        }
        catch (Exception)
        {
            return MessageQuery.None;
        }
    }
}