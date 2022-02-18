using System;
using UnityEngine;
using WebSocketSharp;

public class MobileClient : MonoBehaviour
{
    private WebSocket _serverSocket; // pour parler avec le serveur
    private MessageSender _messageSender;
    public GameObject phoneCamera;
    private GameActions _gameActions;
    private PopUpSystem _popUpSystem;
    private string _playerName;
    public AudioSource soundManager;
    public AudioClip connectionSound;

    /*
     * Deactivate the camera gameObject.
     * Ask the server for the client's private communication path
     * Load Game Scene
     */
    public void Connect(string address, string playerPlace)
    {
        DeactivateCamera();
        ConnectToPrivatePath(address, playerPlace);
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
    }

    public string GetPlayerName()
    {
        return _playerName;
    }

    public void SetGameActions(GameActions gameActions)
    {
        _gameActions = gameActions;
    }

    public void SetPopUpSystem(PopUpSystem popUpSystem)
    {
        _popUpSystem = popUpSystem;
    }

    public void SendDiceResult(DiceFaces result)
    {
        _messageSender.Send(MessageQuery.RollDice, result.ToString());
    }

    public void ValidateChoice()
    {
        _messageSender.Send(MessageQuery.ValidateChoice);
    }

    public void EndTurn()
    {
        _messageSender.Send(MessageQuery.FinishTurn);
    }

    public void SendChosenTile(String tileName)
    {
        _messageSender.Send(MessageQuery.ChosenTile, tileName);
    }

    public void SendChosenObjective(String objectiveName)
    {
        _messageSender.Send(MessageQuery.ValidateObjective, objectiveName);
    }
    
    /*
     * Retrieve my private path and connect to it
     */
    private void ConnectToPrivatePath(string address, string playerPlace)
    {
        _serverSocket = new WebSocket(address);
        _messageSender = new MessageSender(_serverSocket);
        _serverSocket.Connect();
        soundManager.PlayOneShot(connectionSound);
        _serverSocket.OnMessage += ReceiveConnectionPath;
        _messageSender.Send(MessageQuery.PlayerConnection, playerPlace, Device.GetIPv4());
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
        string[] playerName = parser.GetMessageBody().Split('/');
        _playerName = playerName[playerName.Length - 1];
    }

    /*
     * Generic message receiver //TODO
     */
    private void ReceiveGameMessages(object sender, MessageEventArgs args)
    {
        var parser = new MessageParser(args.Data);
        
        Debug.Log("--------- RECEIVED : " + parser.GetFullMessage());
        switch(parser.GetQuery())
        {
            case MessageQuery.WaitingDiceResult:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.StartTurn();
                });
                break;
            case MessageQuery.ValidateChoice:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.ValidateChoice(bool.Parse(parser.GetMessageBody()));
                });
                break;
            case MessageQuery.WaitingPickCard:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Please pick a card.");
                });
                break;
            case MessageQuery.ReceivedCard:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.HidePopUp();
                    _gameActions.AddCardToHand(parser.GetMessageBody());
                });
                break;
            case MessageQuery.WaitingPickTiles:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Please pick your tiles.");
                });
                break;
            case MessageQuery.WaitingChoseTile :
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.HidePopUp();
                    _gameActions.DisplayTilesToChoose(parser.GetMessageBody());
                });
                break;
            case MessageQuery.TilePlaced :
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.TilePlaced();
                });
                break;
            case MessageQuery.WaitingChoseAction :
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("You must now choose your actions on the table.");
                });
                break;
            case MessageQuery.WaitingEndTurn:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.WaitingEndTurn();
                });
                break;
            case MessageQuery.InvalidObjective:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.InvalidObjective();
                });
                break;
            case MessageQuery.ValidateObjective:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Your card was validated.");
                    _gameActions.ValidateObjective(parser.GetMessageBody());
                });
                break;
        }
    }

    private HandManagement GetHandManagement()
    {
        var handGo = GameObject.FindWithTag(TagManager.Hand.ToString());
        return handGo.GetComponent<HandManagement>();
    }

}