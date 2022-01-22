using UnityEngine;
using WebSocketSharp;

public class PlayerRoom
{

    private WebSocket _client;
    private int _playerNumber;

    public PlayerRoom(int playerNumber, string clientSocket)
    {
        _playerNumber = playerNumber;
        _client = new WebSocket(clientSocket);
        _client.Connect();
        _client.OnMessage += ClientMessageReceived;
        _client.Send(QueryMethods.ToString(MessageQuery.Ping) + " Player" + _playerNumber);
    }

    private void ClientMessageReceived(object sender, MessageEventArgs message)
    {
        MessageParser parser = new MessageParser(message.Data);
        switch (parser.GetQuery())
        {
            case MessageQuery.Ping:
                Debug.Log("Client said: " + parser.GetBody());
                break;
            default:
                _client.Send(QueryMethods.ToString(MessageQuery.Ping) + " Unsupported query!");
                break;
        }
    }

    public void ShowMessage(string message)
    {
        _client.Send(QueryMethods.ToString(MessageQuery.Broadcast) + " " + message);
    }
}