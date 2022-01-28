using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class TableRoom : SocketRoom
{
    private readonly Server _server;

    public TableRoom(Server server)
    {
        _server = server;
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        switch (message.GetQuery())
        {
            case MessageQuery.StartGame:
                _server.StartGame();
                break;
            case MessageQuery.Ping:
                Console.Write("Table said: " + message.GetBody());
                break;
        }
    }
}