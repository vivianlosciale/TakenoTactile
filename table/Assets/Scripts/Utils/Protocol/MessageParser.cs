public class MessageParser
{
    private readonly string _message;
    private readonly string _query;
    private readonly string _dest;
    private readonly string _body;
    
    public MessageParser(string message)
    {
        _message = message;
        string[] split = message.Split('-');
        _query = split[0];
        if (split.Length > 2)
        {
            _dest = split[1];
            _body = split[2];
        } 
        else if (split.Length > 1)
        {
            _dest = "";
            _body = split[1];
        }
        else
        {
            _dest = "";
            _body = "";
        }
    }

    public MessageQuery GetQuery()
    {
        return QueryMethods.ToQuery(_query);
    }

    public string GetMessage()
    {
        return _message;
    }

    public string GetDest()
    {
        return _dest;
    }

    public string GetBody()
    {
        return _body;
    }
}