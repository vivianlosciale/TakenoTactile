public class MessageParser
{
    private readonly string _message;
    private readonly string _query;
    private readonly string _body;
    
    public MessageParser(string message)
    {
        _message = message;
        _query = message.Split(' ')[0];
        _body = message.Substring(_query.Length + 1);
    }

    public MessageQuery GetQuery()
    {
        return QueryMethods.ToQuery(_query);
    }

    public string GetMessage()
    {
        return _message;
    }

    public string GetBody()
    {
        return _body;
    }
}