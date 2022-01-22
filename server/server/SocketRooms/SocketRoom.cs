using WebSocketSharp.Server;

namespace takenoko_server.SocketRooms;

public class SocketRoom : WebSocketBehavior
{
    public void Write(string message)
    {
        Send(message);
    }
}