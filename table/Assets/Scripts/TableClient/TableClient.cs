﻿using System;
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
    private List<GameObject> _tilesOnBoard;
    private AudioSource audioSource;
    public AudioClip audioClip;

    private Player _currentPlayer;
    private Player[] players;

    private PlaceHolderBoard _placeHolderBoard;

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
        _tilesOnBoard = new List<GameObject>();
        audioSource = gameObject.GetComponent<AudioSource>();
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
        _sender.Send(MessageQuery.PickCard, cardType);
        _canPickCard = false;
        StartCoroutine(_currentPlayer.UseAction());
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

    internal bool CanPlaceBamboo()
    {
        return _canPlaceBamboo;
    }

    internal void SetPlaceHolderBoard(PlaceHolderBoard placeHolderBoard)
    {
        _placeHolderBoard = placeHolderBoard;
    }

    internal void SendTilePosition(GameObject tile)
    {
        TileEvent tileEvent = tile.GetComponent<TileEvent>();
        _tilesOnBoard.Add(tile);
        string res = PositionDto.ToString(tileEvent.GetPosition().x, tileEvent.GetPosition().y);
        _sender.Send(MessageQuery.ChosenPosition , res);
        StartCoroutine(_currentPlayer.UseAction());
    }

    internal void SendBambooPlaced(string cardPosition)
    {
        _sender.Send(MessageQuery.ChosenPosition, cardPosition);
        _canPlaceBamboo = false;
        foreach (GameObject tile in _tilesOnBoard)
        {
            TileEvent tileEvent = tile.GetComponent<TileEvent>();
            tileEvent.ChangeActive(false);
        }
    }

    /*
     * Action section
     */
    internal void SendChoseActionToServer(Actions action, Player player)
    {
        if (_currentPlayer.id == player.id && player.CanChoseAction())
        {
            _sender.Send(MessageQuery.ChoseAction, action.ToString());
            StartCoroutine(_currentPlayer.AddIcon(action.ToString()));
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
                    audioSource.PlayOneShot(audioClip);
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
                    //audioSource.PlayOneShot();
                    GameObject.Find(message.GetBody()).GetComponent<ParticleSystem>().Play();
                    StartCoroutine(_currentPlayer.ShowWeatherImage(message.GetBody()));
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
            case MessageQuery.WaitingPlaceTile :
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
                    if (_tilesOnBoard.Count == 0) 
                    {
                        SendBambooPlaced(PositionDto.ToString(0, 0));
                        return;
                    }
                    Debug.Log("JE NE SUIS PAS SENSE ETRE LA");
                    List<TileEvent> tilesEventAvailable = new List<TileEvent>();
                    foreach(GameObject tile in _tilesOnBoard)
                    {
                        TileEvent tileEvent = tile.GetComponent<TileEvent>();
                        if (tileEvent.CanPlaceBamboo())
                        {
                            tilesEventAvailable.Add(tileEvent);
                        }
                    }
                    if (tilesEventAvailable.Count == 0)
                    {
                        SendBambooPlaced(PositionDto.ToString(0, 0));
                        return;
                    }
                    _canPlaceBamboo = true;
                    foreach(TileEvent tileEvent in tilesEventAvailable)
                    {
                        tileEvent.ChangeActive(true);
                    }
                });
                break;
            default:
                _sender.Send(MessageQuery.Ping, "Unknown : " + message.GetQuery());
                break;
        }
    }
}