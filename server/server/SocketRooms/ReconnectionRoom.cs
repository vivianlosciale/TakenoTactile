using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class ReconnectionRoom : SocketRoom
{

    private readonly Server _server;

    public ReconnectionRoom(Server server)
    {
        _server = server;
    }

    private void ReconnectPlayer(string dest, int roomNumber)
    {
        string playerRoot = _server.GetPlayerRoot(roomNumber);
        Console.WriteLine("Accept connection on: "+playerRoot);
        Sender.Send(MessageQuery.AcceptConnection, dest, playerRoot);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        switch (message.GetQuery())
        {
            case MessageQuery.PlayerConnection:
                Console.WriteLine("Player " + message.GetBody() + " requires a reconnection in room " + message.GetDest() + ".");
                ReconnectPlayer(message.GetBody(), int.Parse(message.GetDest()));
                break;
        }
    }
}