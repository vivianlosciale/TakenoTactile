using takenoko_server.Utils.Protocol;
using WebSocketSharp;

namespace takenoko_server.SocketRooms;

public class TableRoom : SocketRoom
{
    private readonly Server _server;
    private readonly MessageSender _sender;

    public TableRoom(Server server)
    {
        _server = server;
        _sender = new MessageSender(this);
    }

    public void SendEvent(MessageQuery query)
    {
        _sender.Send(query,"");
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