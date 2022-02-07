
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
    }

    public void SetGameActions(GameActions gameActions)
    {
        _gameActions = gameActions;
    }

    public void SetPopUpSystem(PopUpSystem popUpSystem)
    {
        this._popUpSystem = popUpSystem;
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
        
        //GameObject tileSelector = GameObject.FindWithTag(TagManager.TileSelector.ToString());
        Debug.Log("--------- RECEIVED : " + parser.GetFullMessage());
        Debug.Log("GAME ACTIONS IS : " + _gameActions);
        switch(parser.GetQuery())
        {
            case MessageQuery.RollDice:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.StartTurn();
                });
                break;
            case MessageQuery.ValidateChoice:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    Debug.Log("BOOLEAN PARSED : " + bool.Parse(parser.GetMessageBody()));
                    _gameActions.ValidateChoice(bool.Parse(parser.GetMessageBody()));
                });
                break;
            case MessageQuery.ReceivedCard:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.AddCardToHand(parser.GetMessageBody());
                });
                break;
            case MessageQuery.WaitingPickTiles:
                _popUpSystem.PopUp("Please pick your tiles.");
                break;
            case MessageQuery.ReceivedTiles :
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.HidePopUp();
                    _gameActions.DisplayTilesToChoose(parser.GetMessageBody());
                });
                break;
            case MessageQuery.TilePlaced :
                var tileSelectorGo = GameObject.FindWithTag(TagManager.TileSelector.ToString());
                var tileSelector = tileSelectorGo.GetComponent<TileSelector>();
                tileSelector.DestroyChildren();
                tileSelectorGo.SetActive(false);
                break;
            case MessageQuery.ChoseAction :
                _popUpSystem.PopUp("You must now choose your actions on the table.");
                break;
            case MessageQuery.ImpossibleAction:
                _popUpSystem.PopUp("You cannot do this now.");
                break;
        }
    }

}