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

    private int _currentPlayer;
    private DeckEvent _deckEvent;

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
        FindObjectOfType<OSC>().Close();
        gameObject.GetComponent<MoveObject>().MoveToAnotherScene(sceneToLoad, sceneToUnload);
    }

    public int GetCurrentPlayer()
    {
        return 0;
    }

    public void SetDeckEvent(DeckEvent deckEvent)
    {
        _deckEvent = deckEvent;
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
        _sender.Send(MessageQuery.PickCard, "PickCard");
        _canPickCard = false;
    }

    public bool CanPickCard()
    {
        return _canPickCard;
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        switch (message.GetQuery())
        {
            case MessageQuery.Ping:
                Debug.Log("Server says: " + message.GetMessage());
                _sender.Send(MessageQuery.Ping, "Received!");
                break;
            case MessageQuery.AcceptConnection:
                Debug.Log("Server says: " + message.GetMessage());
                _loginServerSocket.Close();
                _serverSocket = new WebSocket(message.GetBody());
                _serverSocket.Connect();
                _serverSocket.OnMessage += OnMessage;
                _sender = new MessageSender(_serverSocket);
                break;
            case MessageQuery.APlayerJoined:
                Debug.Log("Server says: " + message.GetMessage());
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    ChangePlayerCount();
                });
                break;
            case MessageQuery.StartGame:
                Debug.Log("Server says: " + message.GetMessage());
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    ChangeScene(GAME_SCENE, HOME_SCENE);
                });
                break;
            case MessageQuery.WaitingPickCard:
                Debug.Log("Server says: " + message.GetMessage());
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _canPickCard = true;
                });
                break;
            case MessageQuery.RollDice:
                Debug.Log("Server says: " + message.GetMessage());
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    GameObject.Find(message.GetBody()).GetComponent<ParticleSystem>().Play();
                });
                break;
            case MessageQuery.CurrentPlayerNumber:
                Debug.Log("Server says: " + message.GetMessage());
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _currentPlayer = int.Parse(message.GetBody());
                });
                break;
            default:
                _sender.Send(MessageQuery.Ping, "Unknown query!");
                break;
        }
    }
}