using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class TableClient : MonoBehaviour
{
    private static TableClient _table;
    private MessageSender _sender;

    private WebSocket _loginServerSocket;
    private WebSocket _serverSocket;
    private string _privateAddress;
    private string _serverAddress;

    private QRCreator _qrCreator;
    public InputField addressInput;

    private bool _canPickTile;
    private bool _canPickCard;

    private bool _canPlaceBambooFromRainPower;
    private bool _canPlaceBambooFromFarmer;
    private bool _canMoveFarmer;
    private bool _canMovePanda;
    private bool _canEatBamboo;
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

    private GameObject _endGameButton;

    private static string GAME_SCENE = "Game";
    private static string HOME_SCENE = "TakenoHome";

    private List<ErrorInGame> _errorsInGame;

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
        _errorsInGame = new List<ErrorInGame>();
        _canPickTile = false;
        _canPickCard = false;
        _canPlaceBambooFromRainPower = false;
        _canPlaceBambooFromFarmer = false;
        _canEatBamboo = false;
        _canMoveFarmer = false;
        _canMovePanda = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        _actualDice = DiceFaces.None;
        _tilesEventNotAvailable = new List<Tile>();
        StartCoroutine(ReloadFromSave());
    }

    private IEnumerator ReloadFromSave()
    {
        yield return new WaitForSeconds(5);
        GameObject saveObject = GameObject.Find("SaveObject");
        if (saveObject != null)
        {
            QRCreator qrCreator = GameObject.Find("QRRenderer").GetComponent<QRCreator>();
            _qrCreator = qrCreator;
            Connect(saveObject.GetComponent<SaveObject>().GetAddressServer());
        }
    }

    /*
     *  Erros section
     */
    internal void AddNewError(string tuioValue, string message, ErrorInGameType errorInGameType, Tile tile)
    {
        Debug.Log("ERREUR : " + message);
        _errorsInGame.Add(new ErrorInGame(tuioValue, message, errorInGameType, tile));
        if (_errorsInGame.Count == 1)
        {
            if (_tilesEventNotAvailable.Contains(_errorsInGame[0].errorTile))
            {
                TileMaterial tileMaterial = _errorsInGame[0].errorTile.GameObject.GetComponent<TileMaterial>();
                tileMaterial.TwinkleTile();
            } else
            {
                TileMaterial tileMaterial = _errorsInGame[0].errorTile.GameObject.AddComponent<TileMaterial>();
                tileMaterial.TwinkleTile();
            }
            foreach(Player player in players)
            {
                if (player != null)
                {
                    GameObject error = player.GetBoard().transform.Find("Error").gameObject;
                    error.SetActive(true);
                    error.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(_errorsInGame[0].message);
                }
            }
        }
    }

    internal ErrorInGame GetNextError()
    {
        return _errorsInGame[0];
    }

    internal List<ErrorInGame> GetErrors()
    {
        return _errorsInGame;
    }

    internal void ResolveNextError(ErrorInGame errorInGame)
    {
        if (_errorsInGame[0].Equals(errorInGame))
        {
            if (_tilesEventNotAvailable.Contains(_errorsInGame[0].errorTile))
            {
                TileMaterial tileMaterial = _errorsInGame[0].errorTile.GameObject.GetComponent<TileMaterial>();
                tileMaterial.DeactivateTile();
            } else
            {
                Destroy(_errorsInGame[0].errorTile.GameObject.GetComponent<TileMaterial>());
            }
            if (_errorsInGame.Count > 1)
            {
                if (_tilesEventNotAvailable.Contains(_errorsInGame[1].errorTile))
                {
                    TileMaterial tileMaterial = _errorsInGame[1].errorTile.GameObject.GetComponent<TileMaterial>();
                    tileMaterial.TwinkleTile();
                }
                else
                {
                    TileMaterial tileMaterial = _errorsInGame[1].errorTile.GameObject.AddComponent<TileMaterial>();
                    tileMaterial.TwinkleTile();
                }
                foreach (Player player in players)
                {
                    if (player != null)
                    {
                        player.GetBoard().transform.Find("Error").transform.GetChild(0).GetComponent<TextMeshPro>().SetText(_errorsInGame[1].message);
                    }
                }
            } else
            {
                foreach (Player player in players)
                {
                    if (player != null)
                    {
                        player.GetBoard().transform.Find("Error").gameObject.SetActive(false);
                    }
                }
            }
        }
        _errorsInGame.Remove(errorInGame);
    }

    internal bool HasErrorsInGame()
    {
        return _errorsInGame.Count > 0; 
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

    /*
     * Bamboo section
     */
    internal bool CanRemoveFarmer()
    {
        return _canMoveFarmer;
    }

    internal bool CanMoveFarmer(Tile tile)
    {
        if (_canMoveFarmer)
        {
            if (_tilesEventNotAvailable.Contains(tile))
            {
                SendErrorToServer(_currentPlayer, "Vous ne pouvez pas poser le jardinier ici ! Replacez le jardinier !");
                return false;
            }
            return true;
        }
        return _tileBoard.IsFarmerPosition(tile.position);
    }

    internal bool CanRemovePanda()
    {
        return _canMovePanda;
    }

    internal bool CanMovePanda(Tile tile)
    {
        if (_canMovePanda)
        {
            if (_tilesEventNotAvailable.Contains(tile))
            {
                SendErrorToServer(_currentPlayer, "Vous ne pouvez pas poser le panda ici ! Replacez le panda !");
                return false;
            }
            return true;
        }
        return _tileBoard.IsPandaPosition(tile.position);
    }

    internal bool CanPlaceBambooFromRainPower(Tile tile)
    {
        if (_canPlaceBambooFromRainPower)
        {
            if (_tilesEventNotAvailable.Contains(tile))
            {
                SendErrorToServer(_currentPlayer, "Vous ne pouvez pas poser de bambou ici ! Placez le autre part !");
                return false;
            }
            return true;
        }
        return false;
    }
    
    internal bool CanPlaceBambooFromFarmer(Vector2Int position)
    {
        if (_canPlaceBambooFromFarmer)
        {
            if (!_tileBoard.IsFarmerPosition(position))
            {
                SendErrorToServer(_currentPlayer, "Vous ne pouvez pas poser de bambou ici ! Placez le autre part !");
                return false;
            }
            return true;
        }
        return false;
    }

    internal bool IsGardener(string tuioValue)
    {
        return _tileBoard.IsGardener(tuioValue);
    }

    internal bool IsPanda(string tuioValue)
    {
        return _tileBoard.IsPanda(tuioValue);
    }

    internal bool IsBamboo(string tuioValue)
    {
        return !IsGardener(tuioValue) && !IsPanda(tuioValue);
    }

    internal void SetGardenerPosition(Vector2Int newPosition)
    {
        _tileBoard.SetGardenerPosition(newPosition);
        SendBambooAction(MessageQuery.WaitingMoveFarmer, newPosition);
        _canMoveFarmer = false;
    }

    internal void SetPandaPosition(Vector2Int newPosition)
    {
        _tileBoard.SetPandaPosition(newPosition);
        SendBambooAction(MessageQuery.WaitingMovePanda, newPosition);
        _canMovePanda = false;
    }

    internal bool CanRemoveBamboo()
    {
        return _canEatBamboo;
    }

    internal bool CanEatBamboo(Vector2Int position)
    {
        if (_canEatBamboo)
        {
            if (!_tileBoard.IsPandaPosition(position))
            {
                SendErrorToServer(_currentPlayer, "Vous ne pouvez manger ce bambou ! Replacez le !");
                return false;
            }
            return true;
        }
        else
        {
            //Message global
            //SendErrorToTable("Vous ne pouvez pas manger ce bambou ! Replacez le !");
            return false;
        }
    }

    internal void SendBambooAction(MessageQuery messageQuery, Vector2Int newPosition)
    {
        string cardPosition = PositionDto.ToString(newPosition.x, newPosition.y);
        switch (messageQuery)
        {
            case MessageQuery.WaitingChoseRain:
                _sender.Send(MessageQuery.ChosenPosition, cardPosition);
                _canPlaceBambooFromRainPower = false;
                break;
            case MessageQuery.WaitingMoveFarmer:
            case MessageQuery.WaitingMovePanda:
                _sender.Send(MessageQuery.ChosenPosition, cardPosition);
                StartCoroutine(_currentPlayer.UseAction());
                break;
            case MessageQuery.PlaceBamboo:
                _sender.Send(MessageQuery.BambooPlaced);
                _canPlaceBambooFromFarmer = false;
                _tileBoard.DeactivateGardenerTile();
                break;
            case MessageQuery.EatBamboo:
                _sender.Send(MessageQuery.BambooEaten);
                _canEatBamboo = false;
                _tileBoard.DeactivatePandaTile();
                break;
        }
        foreach (Tile tile in _tilesEventNotAvailable)
        {
            Destroy(tile.GameObject.GetComponent<TileMaterial>());
        }
        _tilesEventNotAvailable = new List<Tile>();
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
            //SendErrorToServer(player, "Tu ne peux pas jouer d'actions !");
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
            //SendErrorToServer(player, "Tu ne peux pas jouer d'actions !")
        }
    }

    /*
     * Error section
     */

    internal void SendErrorToTable(string message)
    {
        foreach(Player player in players)
        {
            GameObject errorObject = player.GetBoard().transform.Find("Error").gameObject;
            errorObject.SetActive(true);
            errorObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = message;
        }
        
        //_sender.Send(MessageQuery.Error, player.id.ToString(), "Tu ne peux pas jouer d'actions !");
    }

    internal void SendErrorToServer(Player player, string message)
    {
        _sender.Send(MessageQuery.Error, player.id.ToString(), message);
    }

    /*
     * CONNEXION SECTION
     */
    private void AddPlayerToGame(int position)
    {
        players[position] = new Player(position);
    }

    private void RemovePlayerFromGame(int position)
    {
        players[position] = null;
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
        GameObject sound = GameObject.Find("Sound");
        if (sound != null) Destroy(sound);
        FindObjectOfType<OSC>().Close();
        gameObject.GetComponent<MoveObject>().MoveToAnotherScene(sceneToLoad, sceneToUnload);
    }

    private void ChangeSceneWithSave(string sceneToLoad, string sceneToUnload, GameObject saveObject)
    {
        FindObjectOfType<OSC>().Close();
        gameObject.GetComponent<MoveObject>().MoveToAnotherSceneWithSave(sceneToLoad, sceneToUnload, saveObject);
    }

    public void GoHomeWithSave()
    {
        GameObject saveObject = new GameObject();
        saveObject.name = "SaveObject";
        SaveObject script = saveObject.AddComponent<SaveObject>();
        script.SetAddressServer(_serverAddress);
        ChangeSceneWithSave(HOME_SCENE, GAME_SCENE, saveObject);
    }

    /*
     * Open a private websocket to communicate with the server.
     * Send the private websocket address to the server.
     */
    public void Connect(QRCreator qrcreator)
    {
        _serverAddress = addressInput.text;
        _loginServerSocket = new WebSocket(addressInput.text);
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
     * Endgame section
     */

    public void SetEndGameButton(GameObject endGameButton)
    {
        _endGameButton = endGameButton;
    }

    /*
     * Reconnection on server when end game
     */
    private void Connect(string serverAddress)
    {
        addressInput.text = serverAddress;
        _loginServerSocket = new WebSocket(serverAddress);
        _loginServerSocket.Connect();
        _loginServerSocket.OnMessage += OnMessage;
        _sender = new MessageSender(_loginServerSocket);
        Exception exception = _sender.Send(MessageQuery.TableConnection, _privateAddress);
        if (exception == null)
        {
            _qrCreator.DisplayQR();
        }
    }

    public void ResetTurn()
    {
        _canPickTile = false;
        _canPickCard = false;
        _canPlaceBambooFromRainPower = false;
        _canPlaceBambooFromFarmer = false;
        _canEatBamboo = false;
        _canMoveFarmer = false;
        _canMovePanda = false;
        foreach (Tile tile in _tilesEventNotAvailable)
        {
            Destroy(tile.GameObject.GetComponent<TileMaterial>());
        }
        _tilesEventNotAvailable = new List<Tile>();
        _tileBoard.DeactivateGardenerTile();
        _tileBoard.DeactivatePandaTile();
        _placeHolderBoard.DeactivateAllSlot();
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
            case MessageQuery.APlayerLeft:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    int player = int.Parse(message.GetBody());
                    RemovePlayerFromGame(player);
                    QRCreator qRCreator = GameObject.Find("QRRenderer").GetComponent<QRCreator>();
                    qRCreator.DisplayQRLeaveForPlayer(player);
                    _loading.RemovePlayer();
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
                        _currentPlayer.GetBoard().transform.Find("backgroundPlay").gameObject.SetActive(false);
                    }
                    _currentPlayer = players[int.Parse(message.GetBody())];
                    _currentPlayer.GetBoard().transform.Find("backgroundPlay").gameObject.SetActive(true);
                });
                break;
            case MessageQuery.WaitingChoseAction:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _currentPlayer.ChangeChoseAction(true);
                });
                break;
            case MessageQuery.ValidateChoice:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _currentPlayer.ChangeChoseAction(false);
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
                        _sender.Send(MessageQuery.ChosenPosition, PositionDto.ToString(0,0));
                        return;
                    }
                    _canPlaceBambooFromRainPower = true;
                    foreach (Tile tile in _tilesEventNotAvailable)
                    {
                        tile.GameObject.AddComponent<TileMaterial>().DeactivateTile();
                    }
                });
                break;
            case MessageQuery.WaitingChoseThunder:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _tilesEventNotAvailable = _tileBoard.TilesWhereCantEatBamboo();
                    if (_tilesEventNotAvailable.Count == _tileBoard.tilesPositions.Count)
                    {
                        _sender.Send(MessageQuery.ChosenPosition, PositionDto.ToString(0, 0));
                        return;
                    }
                    foreach (Tile tile in _tilesEventNotAvailable)
                    {
                        tile.GameObject.AddComponent<TileMaterial>().DeactivateTile();
                    }
                    _canMovePanda = true;
                });
                break;
            case MessageQuery.WaitingMoveFarmer:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _tilesEventNotAvailable = _tileBoard.ActivateGardenerNeighborsSlot();
                    if (_tilesEventNotAvailable.Count == _tileBoard.tilesPositions.Count)
                    {
                        _sender.Send(MessageQuery.ChosenPosition, PositionDto.ToString(0,0));
                        StartCoroutine(_currentPlayer.UseAction());
                        return;
                    }
                    _canMoveFarmer = true;
                });
                break;
            case MessageQuery.PlaceBamboo:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _tileBoard.ActivateGardenerTile();
                    _canPlaceBambooFromFarmer = true;
                });
                break;
            case MessageQuery.WaitingMovePanda:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _tilesEventNotAvailable = _tileBoard.ActivatePandaNeighborsSlot();
                    if (_tilesEventNotAvailable.Count == _tileBoard.tilesPositions.Count)
                    {
                        _sender.Send(MessageQuery.ChosenPosition, PositionDto.ToString(0, 0));
                        StartCoroutine(_currentPlayer.UseAction());
                        return;
                    }
                    _canMovePanda = true;
                });
                break;
            case MessageQuery.EatBamboo:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _tileBoard.ActivatePandaTile();
                    _canEatBamboo = true;
                });
                break;
            case MessageQuery.EndGame:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _serverSocket.Close();
                    QRCreator qRCreator = GameObject.Find("QRRenderer").GetComponent<QRCreator>();
                    foreach (Player player in players)
                    {
                        if (player != null)
                        {
                            GameObject.Find("P" + player.id).SetActive(false);
                            player.GetBoard().SetActive(true);
                        }
                    }
                    Player winnerPlayer = players[int.Parse(message.GetBody())];
                    winnerPlayer.GetBoard().transform.Find("VictoryParticuleSystem").GetComponent<ParticleSystem>().gameObject.SetActive(true);
                    _endGameButton.SetActive(true);
                });
                break;
            case MessageQuery.Disconnection:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    int playerId = int.Parse(message.GetDest());
                    if (playerId == _currentPlayer.id)
                    {
                        ResetTurn();
                    }
                    Debug.Log("PLAYER ID DISCONNEDTED : " + playerId);
                    Debug.Log("MESSAGE : " + message.GetBody());
                    players[playerId].GetBoard().SetActive(false);
                    QRCreator qRCreator = GameObject.Find("QRRenderer").GetComponent<QRCreator>();
                    qRCreator.DisplayQRDisconnectionForPlayer(message.GetBody(), playerId);
                });
                break;
            case MessageQuery.Reconnection:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    int playerId = int.Parse(message.GetBody());
                    players[playerId].GetBoard().SetActive(true);
                    GameObject.Find("P" + playerId).SetActive(false);
                });
                break;
            default:
                _sender.Send(MessageQuery.Ping, "Unknown : " + message.GetQuery());
                break;
        }
    }
}