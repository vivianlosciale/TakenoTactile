using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Server : MonoBehaviour
{
    private static Server _server;

    public GameObject qrRenderer;
    
    private WebSocketServer _ws;
    private List<PlayerRoom> _players;

    private Server() {}

    private void Start()
    {
        _server = this;
        
        // generate socket address
        string socketAddress = "ws://"+Device.GetIPv4()+":8080";
        //string socketAddress = "ws://192.168.254.169:8080";
        
        // initialize and activate QR code renderer
        qrRenderer.GetComponent<QRCreator>().address = socketAddress;
        qrRenderer.SetActive(true);
        
        // initialize attributes
        _ws = new WebSocketServer(socketAddress);
        _players = new List<PlayerRoom>();
        
        // start the login websocket
        _ws.AddWebSocketService<LoginBehavior>("/login");
        _ws.Start();
        Debug.Log("Server started on " + socketAddress);
    }

    private string ConnectClient()
    {
        // generate a personal path for the new player
        string socketServicePath = "/player" + _players.Count;
        
        // add a new player room
        _players.Add(new PlayerRoom(_ws, socketServicePath));
        
        // start the game if enough player are connected
        if (_players.Count >= 4)
        {
            StartGame();
        }
        
        return socketServicePath;
    }

    private void StartGame()
    {
        // stop the login websocket
        _ws.Stop();
        
        // run the game
        new Takenoko().StartGame(_players);
    }

    private class LoginBehavior : WebSocketBehavior
    {

        protected override void OnMessage(MessageEventArgs e)
        {
            string message = e.Data;
            Debug.Log("Message received from client: " + message);
            
            // retrieve the query from the received message
            string query = message.Split(' ')[0];
            
            switch (query)
            {
                case "REQUEST_LOG":
                    string id = message.Substring(query.Length + 1);
                    Send(id + " " + _server.ConnectClient());
                    break;
                default:
                    Send("OK");
                    break;
            }
        }
    }
}