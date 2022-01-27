using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class PlayerRoom : SocketRoom
{

    private readonly Server _server;
    private readonly int _playerNumber;
    private bool _isPlaying = false;

    public PlayerRoom(Server server, int playerNumber)
    {
        _server = server;
        _playerNumber = playerNumber;
    }

    public bool IsPlaying()
    {
        return _isPlaying;
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