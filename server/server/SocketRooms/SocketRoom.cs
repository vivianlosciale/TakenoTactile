using WebSocketSharp.Server;

namespace server.SocketRooms;

public class SocketRoom : WebSocketBehavior
{
    public void Write(string message)
    {
        Send(message);
    }
}