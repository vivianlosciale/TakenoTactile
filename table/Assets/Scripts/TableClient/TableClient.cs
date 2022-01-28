using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class TableClient : MonoBehaviour
{
    private static TableClient _table;
    private MessageSender _sender;

    private WebSocket _loginServerSocket;
    private WebSocket _serverSocket;
    private string _privateAddress;
    
    public GameObject playerCountContainer;
    public InputField adresseInput;

    private int playerCount;
    private bool _canPickCard;

    private static string GAME_SCENE = "Game";
    private static string HOME_SCENE = "Takenotest";

        
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

    public void StartGame()
    {
        _sender.Send(MessageQuery.StartGame);
    }

    public void ChangeScene(string sceneToLoad, string sceneToUnload)
    {
        gameObject.GetComponent<MoveObject>().MoveToAnotherScene(sceneToLoad, sceneToUnload);
    }

    
    /*
     * Connect to the server websocket.
     * Send the private websocket address to the server.
     */
    private void RequestConnection(string serverAddress)
    {
        _loginServerSocket = new WebSocket(serverAddress);
        _loginServerSocket.Connect();
        _loginServerSocket.OnMessage += OnMessage;

        _sender = new MessageSender(_loginServerSocket);
        _sender.Send(MessageQuery.TableConnection, _privateAddress);
    }

    private void ChangePlayerCount()
    {
        playerCount++;
        playerCountContainer.GetComponent<Text>().text = playerCount + " / 4";
    }

    public void PickCard()
    {
        _sender.Send(MessageQuery.PickCard);
        _canPickCard = false;
    }

    public bool CanPickCard()
    {
        return _canPickCard;
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
                _loginServerSocket.Close();
                _serverSocket = new WebSocket(parser.GetBody());
                _serverSocket.Connect();
                _serverSocket.OnMessage += OnMessage;
                _sender = new MessageSender(_serverSocket);
                break;
            case MessageQuery.APlayerJoined:
                Debug.Log("Server says: A player joined");
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    ChangePlayerCount();
                });
                break;
            case MessageQuery.StartGame:
                Debug.Log("Server says: Start Game");
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    ChangeScene(GAME_SCENE, HOME_SCENE);
                });
                break;
            case MessageQuery.WaitingPickCard:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _canPickCard = true;
                });
                break;
            default:
                _sender.Send(MessageQuery.Ping, "Unknown query!");
                break;
        }
    }
}