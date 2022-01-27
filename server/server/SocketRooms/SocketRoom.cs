using server.Utils.Protocol;
using WebSocketSharp.Server;

namespace server.SocketRooms;

public class SocketRoom : WebSocketBehavior
{
    
    protected readonly MessageSender Sender;

    protected SocketRoom()
    {
        Sender = new MessageSender(this);
    }
    public void Write(string message)
    {
        Send(message);
    }

    public void SendEvent(MessageQuery query)
    {
        Sender.Send(query);
    }
}