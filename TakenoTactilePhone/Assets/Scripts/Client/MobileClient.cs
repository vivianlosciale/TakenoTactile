
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class MobileClient : MonoBehaviour
{
    private WebSocket _serverSocket; // pour parler avec le serveur
    private MessageSender _messageSender;
    public GameObject unityCamera;
    public GameObject PopUpManager;

    /*
     * Deactivate the camera gameObject.
     * Ask the server for the client's private communication path
     * Load Game Scene
     */
    public void Connect(string address)
    {
        DeactivateCamera();
        ConnectToPrivatePath(address);
        StartGame();
    }
    
    
    /*
    * Deactivate the camera gameObject.
    */
    private void DeactivateCamera() 
    {
        unityCamera.SetActive(false);
    }

    /*
     * Switch to the game scene
     */
    public void StartGame()
    {
        var move = gameObject.GetComponent<MoveObject>();
        move.MoveToAnotherScene();
    }
    
    
    /*
     * Retrieve my private path and connect to it
     */
    private void ConnectToPrivatePath(string address)
    {
        _serverSocket = new WebSocket(address);
        _messageSender = new MessageSender(_serverSocket);
        _serverSocket.Connect();
        _serverSocket.OnMessage += ReceiveConnectionPath;
        _messageSender.Send(MessageQuery.PlayerConnection, Device.GetIPv4());
    }

    private void ReceiveConnectionPath(object sender, MessageEventArgs args)
    {
        var parser = new MessageParser(args.Data);
        if (!MessageQuery.AcceptConnection.Equals(parser.GetQuery())) return;
        if (!parser.GetDest().Equals(Device.GetIPv4())) return;
        _serverSocket.Close();
        _serverSocket = new WebSocket(parser.GetMessageBody());
        _serverSocket.Connect();
        _serverSocket.OnMessage += ReceiveGameMessages;
    }

    /*
     * Generic message receiver //TODO
     */
    private void ReceiveGameMessages(object sender, MessageEventArgs args)
    {
        var parser = new MessageParser(args.Data);
        PopUpManager = GameObject.FindGameObjectWithTag(TagManager.PopUpManager.ToString());
        switch(parser.GetQuery())
        {
            case MessageQuery.StartGame:
                break;
            case MessageQuery.PlayerBroadcast:
                break;
            default:
                break;
        }
    }
    
}