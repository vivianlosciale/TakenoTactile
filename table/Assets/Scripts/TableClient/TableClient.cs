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
    
    public InputField adresseInput;

    private int playerCount;
    private bool _canPickTile;
    private bool _canPickCard;

    private Player _currentPlayer;
    private Player[] players;

    private PlaceHolderBoard _placeHolderBoard;

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
        players = new Player[4];
        _canPickTile = false;
        _canPickCard = false;
    }

    /*
     *  GAME SECTION
     */

    internal Player GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    internal Player GetPlayerFromPosition(int boardPosition)
    {
        for(int i = 0; i < playerCount; i++)
        {
            if (players[i].GetBoardPosition() == boardPosition)
            {
                return players[i];
            }
        }
        return null;
    }

    /*
     * Cards section
     */
    internal void PickCard()
    {
        _sender.Send(MessageQuery.PickCard);
        _canPickCard = false;
    }

    internal bool CanPickCard()
    {
        return _canPickCard;
    }

    /*
     * Tiles section
     */
    internal void PickTile()
    {
        _sender.Send(MessageQuery.PickTiles);
        _canPickTile = false;
    }

    internal bool CanPickTile()
    {
        return _canPickTile;
    }

    internal void SetPlaceHolderBoard(PlaceHolderBoard placeHolderBoard)
    {
        _placeHolderBoard = placeHolderBoard;
    }

    internal void SendTilePosition(Vector2Int position)
    {
        string res = PositionDto.ToString(position.x, position.y);
        Debug.Log("Entre temps " + res + " Entre temps");
        _sender.Send(MessageQuery.ChosenTile, res);
    }

    /*
     * Action section
     */
    internal void SendChoseActionToServer(Actions action, Player player)
    {
        if (_currentPlayer.id == player.id)
        {
            _sender.Send(MessageQuery.ChoseAction, action.ToString());
        } else
        {
            //TODO envoie au serveur un message d'erreur
        }
    }

    internal void SendRemoveActionToServer(Actions action, Player player)
    {
        if (_currentPlayer.id == player.id && player.CanChoseAction())
        {
            _sender.Send(MessageQuery.RemoveAction, action.ToString());
        }
        else
        {
            //TODO envoie au serveur un message d'erreur
        }
    }

    /*
     * CONNEXION SECTION
     */
    private void AddPlayerToGame(int position)
    {
        players[playerCount] = new Player(playerCount, position);
        playerCount++;
    }

    /*
     * Start the Game
     */
    public void StartGame()
    {
        _sender.Send(MessageQuery.StartGame);
    }

    /*
     * Change the Scene
     */
    public void ChangeScene(string sceneToLoad, string sceneToUnload)
    {
        FindObjectOfType<OSC>().Close();
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
     * Message Listener from server
     */
    private void OnMessage(object sender, MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        Debug.Log("Server says: " + message.GetFullMessage());
        switch (message.GetQuery())
        {
            case MessageQuery.Ping:
                _sender.Send(MessageQuery.Ping, "Received!");
                break;
            case MessageQuery.AcceptConnection:
                _loginServerSocket.Close();
                _serverSocket = new WebSocket(message.GetBody());
                _serverSocket.Connect();
                _serverSocket.OnMessage += OnMessage;
                _sender = new MessageSender(_serverSocket);
                break;
            case MessageQuery.APlayerJoined:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    AddPlayerToGame(int.Parse(message.GetBody()));
                });
                break;
            case MessageQuery.StartGame:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    ChangeScene(GAME_SCENE, HOME_SCENE);
                });
                break;
            case MessageQuery.WaitingPickTiles:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _canPickTile = true;
                });
                break;
            case MessageQuery.WaitingPickCard:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _canPickCard = true;
                });
                break;
            case MessageQuery.RollDice:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    GameObject.Find(message.GetBody()).GetComponent<ParticleSystem>().Play();
                });
                break;
            case MessageQuery.CurrentPlayerNumber:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _currentPlayer = players[int.Parse(message.GetBody())];
                });
                break;
            case MessageQuery.ChoseAction:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _currentPlayer.ChangeChoseAction();
                });
                break;
            case MessageQuery.ValidateChoice:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _currentPlayer.ChangeChoseAction();
                });
                break;
            case MessageQuery.ChosenTile:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _placeHolderBoard.ActivateNeighborsSlot(message.GetBody());
                    //TODO CHANGER LA CARTE
                    //string cardName = ;
                });
                break;
            default:
                _sender.Send(MessageQuery.Ping, "Unknown : " + message.GetQuery());
                break;
        }
    }
}