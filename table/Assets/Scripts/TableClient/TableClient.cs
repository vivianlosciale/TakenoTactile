using System;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class TableClient : MonoBehaviour
{
    private static TableClient _table;
    private MessageSender _sender;

    private WebSocket _serverSocket;
    private string _privateAddress;
    public InputField adresseInput;

    public GameObject playerCountText;
    private int playerCount;
        
    /*
     * Private constructor to avoid outside instantiations.
     */
    private TableClient() {}


    /*
     * Create a singleton.
     */
    private void Start()
    {
        _table = this;
        _privateAddress = "ws://" + Device.GetIPv4() + ":8080";
        playerCount = 0;
    }


    /*
     * Deactivate the camera gameObject.
     * Open a private websocket to communicate with the server.
     * Send the private websocket address to the server.
     */
    public void Connect()
    {
        Debug.Log(adresseInput.text);
        RequestConnection(adresseInput.text);
    }

    
    /*
     * Connect to the server websocket.
     * Send the private websocket address to the server.
     */
    private void RequestConnection(string serverAddress)
    {
        _serverSocket = new WebSocket(serverAddress);
        _serverSocket.Connect();
        _serverSocket.OnMessage += OnMessage;

        _sender = new MessageSender(_serverSocket);
        _sender.Send(MessageQuery.TableConnection, _privateAddress);
    }

    private void changePlayerCount()
    {
        playerCount++;
        playerCountText.GetComponent<Text>().text = playerCount + " / 4";
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        MessageParser parser = new MessageParser(e.Data);
        switch (parser.GetQuery())
        {
            case MessageQuery.Ping:
                Debug.Log("Server says: " + parser.GetBody());
                _sender.Send(MessageQuery.Ping, "Received!");
                break;
            case MessageQuery.AcceptConnection:
                Debug.Log("Server says: " + parser.GetBody());
                break;
            case MessageQuery.APlayerJoined:
                Debug.Log("Server says: A player joined");
                changePlayerCount();
                break;
            default:
                _sender.Send(MessageQuery.Ping, "Unknown query!");
                break;
        }
    }
}