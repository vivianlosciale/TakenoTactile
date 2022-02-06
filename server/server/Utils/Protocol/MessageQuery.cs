namespace server.Utils.Protocol;

public enum MessageQuery
{
    // COMMON
    None,
    Ping,
    AcceptConnection,
    StartGame,
    WaitingPickCard,
    WaitingPickTile,
    WaitingChoseRain,
    ImpossibleAction,
    
    // PLAYERS
    PlayerBroadcast,
    PlayerConnection,
    GameIsFull,
    RollDice,
    ValidateObjective,
    FinishTurn,
    ReceivedCard,
    InvalidObjective,
    
    // TABLE
    TableConnection,
    APlayerJoined,
    CurrentPlayerNumber,
    ChosePower,
    PickCard,
    ChosenTile,
    Rain,
    StopGame,
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