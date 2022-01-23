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

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        switch (message.GetQuery())
        {
            case MessageQuery.PlayerConnection:
                string playerRoot = _server.AddPlayer();
                _sender.Send(
                    MessageQuery.AcceptConnection,
                    message.GetBody(),
                    playerRoot);
                break;
            case MessageQuery.TableConnection:
                string tableRoot = _server.SetTable();
                _sender.Send(
                    MessageQuery.AcceptConnection,
                    message.GetBody(),
                    tableRoot);
                break;
        }
    }
}