
using System;
using UnityEngine;
using WebSocketSharp;

public class MobileClient : MonoBehaviour
{
    private WebSocket _serverSocket; // pour parler avec le serveur
    private MessageSender _messageSender;
    public GameObject phoneCamera;
    private GameActions _gameActions;

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
        phoneCamera.SetActive(false);
    }

    /*
     * Switch to the game scene
     */
    public void StartGame()
    {
        var move = gameObject.GetComponent<MoveObject>();
        move.MoveToAnotherScene();
        //GameManager = GameObject.FindWithTag(TagManager.GameManager.ToString());
    }

    public void SetGameActions(GameActions gameActions)
    {
        _gameActions = gameActions;
    }

    public void SendDiceResult(DiceFaces result)
    {
        _messageSender.Send(MessageQuery.RollDice, result.ToString());
    }

    public void EndTurn()
    {
        _messageSender.Send(MessageQuery.FinishTurn);
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
        _messageSender = new MessageSender(_serverSocket);
        _serverSocket.Connect();
        _serverSocket.OnMessage += ReceiveGameMessages;
    }

    /*
     * Generic message receiver //TODO
     */
    private void ReceiveGameMessages(object sender, MessageEventArgs args)
    {
        var parser = new MessageParser(args.Data);
        var popUpManager = GameObject.FindGameObjectWithTag(TagManager.PopUpManager.ToString());
        var popUpSystem = popUpManager.GetComponent<PopUpSystem>();
        switch(parser.GetQuery())
        {
            case MessageQuery.StartGame:
                break;
            case MessageQuery.PlayerBroadcast:
                break;
            case MessageQuery.RollDice:
                //Do Action Roll Dice
                //subscribe to ontrigger event du DiceChecker
                //on retrieve le result
                //on renvoie 
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    // on fait apparatire le message
                    // 
                    _gameActions.StartTurn();
                });
                break;
            case MessageQuery.PickCard:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.AddCardToHand(parser.GetMessageBody());
                });
                break;
            default:
                break;
        }
    }
    
}