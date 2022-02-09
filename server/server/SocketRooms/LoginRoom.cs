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

    private void ConnectPlayer(string dest, int roomNumber)
    {
        string? playerRoot = _server.AddPlayer(roomNumber);
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
                Console.WriteLine("Player "+message.GetBody()+" requires a connection in room "+message.GetDest()+".");
                ConnectPlayer(message.GetBody() ,int.Parse(message.GetDest()));
                break;
            case MessageQuery.TableConnection:
                Console.WriteLine("Table "+message.GetBody()+" requires a connection.");
                ConnectTable(message.GetBody());
                break;
        }
    }
}