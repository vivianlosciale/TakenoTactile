using System;

public enum MessageQuery
{
    None,
    Ping,
    RequestConnection,
}

public static class QueryMethods
{
    public static string ToString(MessageQuery query)
    {
        return Enum.GetName(typeof(MessageQuery), query);
    }

    public static MessageQuery ToQuery(string query)
    {
        try
        {
            return (MessageQuery)Enum.Parse(typeof(MessageQuery), query);
        }
        catch (Exception e)
        {
            return MessageQuery.None;
        }
    }
}