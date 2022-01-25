using server.SocketRooms;

namespace server.Utils.Protocol;

public class MessageSender
{

    private readonly SocketRoom _room;

    public MessageSender(SocketRoom room)
    {
        _room = room;
    }

    public void Send(MessageQuery query)
    {
        Send(QueryMethods.ToString(query));
    }

    public void Send(MessageQuery query, string message)
    {
        Send(QueryMethods.ToString(query)+"-"+message);
    }

    public void Send(MessageQuery query, string dest, string message)
    {
        Send(QueryMethods.ToString(query)+"-"+dest+"-"+message);
    }

    public void Send(string message)
    {
        for (int tries = 0; tries < 5;)
        {
            try
            {
                _room.Write(message);
                return;
            }
            catch (Exception)
            {
                tries++;
                Console.WriteLine("Message sent try "+tries+": "+message);
            }
        }
        Console.Error.Write("Message sent failed!");
    }
}