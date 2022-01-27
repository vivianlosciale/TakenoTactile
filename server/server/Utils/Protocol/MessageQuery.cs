namespace server.Utils.Protocol;

public enum MessageQuery
{
    // COMMON
    None,
    Ping,
    AcceptConnection,
    StartGame, 
    
    // PLAYERS
    PlayerBroadcast,
    PlayerConnection,
    GameIsFull,
    
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