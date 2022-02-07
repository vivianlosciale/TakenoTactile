namespace server.Utils.Protocol;

public enum MessageQuery
{
    // COMMON
    None,
    Ping,
    Error,
    AcceptConnection,
    StartGame,
    WaitingPickCard,
    WaitingPickTile,
    WaitingChoseRain,
    ImpossibleAction,
    ValidateChoice,
    ChosenTile,

    // PLAYERS
    PlayerBroadcast,
    PlayerConnection,
    GameIsFull,
    RollDice,
    ValidateObjective,
    FinishTurn,
    ReceivedCard,
    ReceivedTiles,
    InvalidObjective,
    
    // TABLE
    TableConnection,
    APlayerJoined,
    CurrentPlayerNumber,
    ChoseAction,
    RemoveAction,
    PickCard,
    PickTiles,
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