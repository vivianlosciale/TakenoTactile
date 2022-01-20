using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Client : MonoBehaviour
{
    private static Client _client;
    
    public GameObject unityCamera;
    public Text text;

    private WebSocketServer _ws;
    private string _privateAddress;
    
    
    /*
     * Private constructor to avoid outside instantiations.
     */
    private Client() {}


    /*
     * Create a singleton.
     */
    private void Start()
    {
        _client = this;
        _privateAddress = OpenPrivateSocket();
    }


    /*
     * Deactivate the camera gameObject.
     * Open a private websocket to communicate with the server.
     * Send the private websocket address to the server.
     */
    public void Connect(string address)
    {
        text.text = "Before desactivate camera";
        DeactivateCamera();
        text.text = "After desactivate camera";
        RequestConnection(address);
        text.text = "After Request camera";
    }

    
    /*
     * Deactivate the camera gameObject.
     */
    private void DeactivateCamera()
    {
        unityCamera.SetActive(false);
    }

    
    /*
     * Open a private websocket.
     * Return the websocket address.
     */
    private string OpenPrivateSocket()
    {
        string socketAddress = "ws://" + Device.GetIPv4() + ":8080";
        string socketPath = "/player";
        _ws = new WebSocketServer(socketAddress);
        _ws.AddWebSocketService<ClientBehavior>(socketPath);
        _ws.Start();
        Debug.Log("Client started on " + socketAddress);
        text.text = socketAddress + socketPath;
        return socketAddress + socketPath;
    }

    
    /*
     * Connect to the server websocket.
     * Send the private websocket address to the server.
     */
    private void RequestConnection(string serverAddress)
    {
        WebSocket server = new WebSocket(serverAddress);
        server.Connect();
        server.Send(QueryMethods.ToString(MessageQuery.RequestConnection) + " " + _privateAddress);
        server.Close();
    }
    
    
    private class ClientBehavior : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            MessageParser parser = new MessageParser(e.Data);
            switch (parser.GetQuery())
            {
                case MessageQuery.Ping:
                    Debug.Log("Server says: " + parser.GetBody());
                    _client.text.text = parser.GetBody();
                    Send(QueryMethods.ToString(MessageQuery.Ping) + " Received!");
                    break;
                case MessageQuery.Broadcast:
                    Debug.Log("Server says: " + parser.GetBody());
                    _client.text.text = parser.GetBody();
                    break;
                default:
                    Send(QueryMethods.ToString(MessageQuery.Ping) + " Unknown query!");
                    break;
            }
        }
    }
}