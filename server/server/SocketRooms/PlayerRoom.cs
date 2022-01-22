using takenoko_server.Utils.Protocol;
using WebSocketSharp;

namespace takenoko_server.SocketRooms;

public class PlayerRoom : SocketRoom
{

    private readonly Server _server;
    private readonly int _playerNumber;
    private readonly MessageSender _sender;

    public PlayerRoom(Server server, int playerNumber)
    {
        _server = server;
        _playerNumber = playerNumber;
        _sender = new MessageSender(this);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        switch (message.GetQuery())
        {
            case MessageQuery.Ping:
                Console.Write("Client said: " + message.GetBody());
                break;
        }
    }
    
    
}