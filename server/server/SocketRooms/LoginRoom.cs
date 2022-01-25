using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class LoginRoom : SocketRoom
{

    private readonly Server _server;
    private readonly MessageSender _sender;

    public LoginRoom(Server server)
    {
        _server = server;
        _sender = new MessageSender(this);
    }

    private void ConnectPlayer(string dest)
    {
        string? playerRoot = _server.AddPlayer();
        if (playerRoot == null) _sender.Send(MessageQuery.GameIsFull);
        else _sender.Send(MessageQuery.AcceptConnection, dest, playerRoot);
    }

    private void ConnectTable(string dest)
    {
        string tableRoot = _server.SetTable();
        _sender.Send(MessageQuery.AcceptConnection, dest, tableRoot);
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