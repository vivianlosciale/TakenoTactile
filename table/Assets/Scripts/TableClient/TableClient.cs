using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool _canPickTile;
    private bool _canPickCard;

    private bool _canPlaceBamboo;
    private List<Tile> _tilesEventNotAvailable;

    private DiceFaces _actualDice;

    private AudioSource audioSource;
    public AudioClip jingleClip;
    public List<AudioClip> diceAudioClips;

    private Player _currentPlayer;
    private Player[] players;

    private PlaceHolderBoard _placeHolderBoard;
    private TileBoard _tileBoard;

    public Loading _loading;

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
        players = new Player[4];
        _canPickTile = false;
        _canPickCard = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        _actualDice = DiceFaces.None;
        _tilesEventNotAvailable = new List<Tile>();
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
        return players[boardPosition];
    }

    /*
     * Cards section
     */
    internal void PickCard(string cardType)
    {
        _canPickCard = false;
        StartCoroutine(_currentPlayer.UseAction());
        StartCoroutine(PickCardCorountine(cardType));
    }

    private IEnumerator PickCardCorountine(string cardType)
    {
        float waitTime = 0.0f;
        switch(_currentPlayer.id)
        {
            case 0:
                waitTime = 2.0f;
                break;
            case 1:
                waitTime = 3.0f;
                break;
            case 2:
                waitTime = 2.0f;
                break;
            case 3:
                waitTime = 1.5f;
                break;
        }
        yield return new WaitForSeconds(waitTime);
        _sender.Send(MessageQuery.PickCard, cardType);
    }

    internal bool CanPickCard()
    {
        return _canPickCard;
    }

    /*
     *  Dice section
     */
    internal DiceFaces GetActualDice()
    {
        return _actualDice;
    }

    /*
     * Tiles section
     */
    internal void PickTile()
    {
        _canPickTile = false;
        StartCoroutine(PickTileCorountine());
    }

    private IEnumerator PickTileCorountine()
    {
        float waitTime = 0.0f;
        switch (_currentPlayer.id)
        {
            case 0:
                waitTime = 2.5f;
                break;
            case 1:
                waitTime = 3.5f;
                break;
            case 2:
                waitTime = 3.5f;
                break;
            case 3:
                waitTime = 2.5f;
                break;
        }
        yield return new WaitForSeconds(waitTime);
        _sender.Send(MessageQuery.PickTiles);
    }

    internal bool CanPickTile()
    {
        return _canPickTile;
    }

    internal bool CanPlaceBamboo()
    {
        return _canPlaceBamboo;
    }

    internal void SetPlaceHolderBoard(PlaceHolderBoard placeHolderBoard)
    {
        _placeHolderBoard = placeHolderBoard;
    }

    internal void SetTileBoard(TileBoard tileBoard)
    {
        _tileBoard = tileBoard;
    }

    internal void SendTilePosition(Tile tile)
    {
        string res = PositionDto.ToString(tile.position.x, tile.position.y);
        _sender.Send(MessageQuery.ChosenPosition , res);
        StartCoroutine(_currentPlayer.UseAction());
    }

    internal void SendBambooPlaced(string cardPosition)
    {
        _sender.Send(MessageQuery.ChosenPosition, cardPosition);
        _canPlaceBamboo = false;
        foreach (Tile tile in _tilesEventNotAvailable)
        {
            Destroy(tile.GameObject.GetComponent<TileMaterial>());
        }
    }

    /*
     * Action section
     */
    internal void SendChoseActionToServer(Actions action, Player player)
    {
        if (_currentPlayer.id == player.id && player.CanChoseAction(_actualDice, action))
        {
            player.ChoseAction(action);
            _sender.Send(MessageQuery.ChoseAction, action.ToString());
            StartCoroutine(_currentPlayer.AddIcon(action.ToString()));
        } else
        {
            //TODO envoie au serveur un message d'erreur
        }
    }

    internal void SendRemoveActionToServer(Actions action, Player player)
    {
        if (_currentPlayer.id == player.id && player.CanRemoveAction())
        {
            player.RemoveAction(action);
            _sender.Send(MessageQuery.RemoveAction, action.ToString());
            StartCoroutine(_currentPlayer.RemoveIcon(action.ToString()));
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
        players[position] = new Player(position);
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
     * Open a private websocket to communicate with the server.
     * Send the private websocket address to the server.
     */
    public void Connect(QRCreator qrcreator)
    {
        _loginServerSocket = new WebSocket(adresseInput.text);
        _loginServerSocket.Connect();
        _loginServerSocket.OnMessage += OnMessage;

        _sender = new MessageSender(_loginServerSocket);
        Exception exception = _sender.Send(MessageQuery.TableConnection, _privateAddress);
        if (exception == null)
        {
            qrcreator.DisplayQR();
        }
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
                    int player = int.Parse(message.GetBody());
                    AddPlayerToGame(player);
                    GameObject.Find("P" + player).SetActive(false);
                    _loading.AddPlayer();
                });
                break;
            case MessageQuery.StartGame:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    ChangeScene(GAME_SCENE, HOME_SCENE);
                    audioSource.PlayOneShot(jingleClip);
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
                    _actualDice = DiceFacesMethods.ToDiceFace(message.GetBody());
                    GameObject.Find(message.GetBody()).GetComponent<ParticleSystem>().Play();
                    StartCoroutine(_currentPlayer.ShowWeatherImage(message.GetBody()));
                    string diceSoundName = message.GetBody() + "_sound";
                    foreach(AudioClip audioClip in diceAudioClips)
                    {
                        if (audioClip.name.Equals(diceSoundName))
                        {
                            audioSource.PlayOneShot(audioClip);
                            return;
                        }
                    }
                });
                break;
            case MessageQuery.CurrentPlayerNumber:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    if (_currentPlayer != null)
                    {
                        StartCoroutine(_currentPlayer.RemoveWeatherImage());
                        StartCoroutine(_currentPlayer.RemoveAllIcon());
                    }
                    _currentPlayer = players[int.Parse(message.GetBody())];
                });
                break;
            case MessageQuery.WaitingChoseAction:
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
            case MessageQuery.WaitingPlaceTile:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _placeHolderBoard.ActivateNeighborsSlot(message.GetBody());
                });
                break;
            case MessageQuery.ValidateObjective:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _currentPlayer.ValidateObjective(message.GetBody());
                });
                break;
            case MessageQuery.WaitingChoseRain:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _tilesEventNotAvailable = _tileBoard.TilesWhereCantPlaceBamboo();
                    if (_tilesEventNotAvailable.Count == _tileBoard.tilesPositions.Count)
                    {
                        SendBambooPlaced(PositionDto.ToString(0, 0));
                        return;
                    }
                    _canPlaceBamboo = true;
                    foreach (Tile tile in _tilesEventNotAvailable)
                    {
                        tile.GameObject.AddComponent<TileMaterial>();
                    }
                });
                break;
            default:
                _sender.Send(MessageQuery.Ping, "Unknown : " + message.GetQuery());
                break;
        }
    }
}