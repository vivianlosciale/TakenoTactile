using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Server : MonoBehaviour
{
    private readonly string _loginPath = "/login";
    private static Server _server;

    public GameObject qrRenderer;
    
    private WebSocketServer _ws;
    private List<PlayerRoom> _players;

    
    /*
     * Private constructor to avoid outside instantiations.
     */
    private Server() {}

    
    /*
     * Initialise the QR code renderer.
     * Initialise the websocket login room.
     */
    private void Start()
    {
        _server = this;
        
        // generate socket address
        string socketAddress = "ws://"+Device.GetIPv4()+":8080";
        
        // initialize and activate QR code renderer
        qrRenderer.GetComponent<QRCreator>().address = socketAddress + _loginPath;
        qrRenderer.SetActive(true);
        
        // initialize attributes
        _ws = new WebSocketServer(socketAddress);
        _players = new List<PlayerRoom>();
        
        // start the login websocket
        _ws.AddWebSocketService<LoginBehavior>(_loginPath);
        _ws.Start();
        Debug.Log("Server started on " + socketAddress);
    }

    
    /*
     * Add a new player to the game and connect to its private websocket.
     * Start the game if enough players are logged in.
     */
    private void ConnectClient(string clientAddress)
    {
        _players.Add(new PlayerRoom(_players.Count,clientAddress));
        if (_players.Count >= 4) StartGame();
    }

    
    /*
     * Stop the login websocket.
     * Start the game.
     */
    private void StartGame()
    {
        _ws.Stop();
        new Takenoko().StartGame(_players);
    }

    
    private class LoginBehavior : WebSocketBehavior
    {

        protected override void OnMessage(MessageEventArgs e)
        {
            MessageParser parser = new MessageParser(e.Data);
            switch (parser.GetQuery())
            {
                case MessageQuery.RequestConnection:
                    Debug.Log("Client request connection: " + parser.GetBody());
                    _server.ConnectClient(parser.GetBody());
                    break;
                default:
                    Send("Unknown operation");
                    break;
            }
        }
    }
}