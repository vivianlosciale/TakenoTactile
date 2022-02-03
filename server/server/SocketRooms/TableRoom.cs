using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class TableRoom : SocketRoom
{
    private readonly Server _server;
    private bool _pickCard;

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
                Console.WriteLine("Table said: " + message.GetBody());
                break;
            case MessageQuery.PickCard:
                Console.WriteLine("Table asked for a card pick.");
                _pickCard = true;
                break;
        }
    }

    public void WaitForCardPick(int currentPlayer)
    {
        _pickCard = false;
        SendEvent(MessageQuery.WaitingPickCard);
        Console.WriteLine("The table waiting for picking card");
        while (!_pickCard) WaitSeconds(1);
    }

    public void SendCurrentPlayerNumber(int currentPlayer)
    {
        SendEventWithMessage(MessageQuery.CurrentPlayerNumber, currentPlayer.ToString());
    }
}