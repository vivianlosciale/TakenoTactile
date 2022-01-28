using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class LoginRoom : SocketRoom
{

    private readonly Server _server;

    public LoginRoom(Server server)
    {
        _server = server;
    }

    private void ConnectPlayer(string dest)
    {
        string? playerRoot = _server.AddPlayer();
        if (playerRoot == null) Sender.Send(MessageQuery.GameIsFull);
        else Sender.Send(MessageQuery.AcceptConnection, dest, playerRoot);
    }

    private void ConnectTable(string dest)
    {
        string tableRoot = _server.SetTable();
        Sender.Send(MessageQuery.AcceptConnection, dest, tableRoot);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        switch (message.GetQuery())
        {
            case MessageQuery.PlayerConnection:
                ConnectPlayer(message.GetBody());
                break;
            case MessageQuery.TableConnection:
                ConnectTable(message.GetBody());
                break;
        }
    }
}