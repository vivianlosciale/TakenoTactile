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
    private Helper _helper;

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
    private void StartGame()
    {
        var move = gameObject.GetComponent<MoveObject>();
        move.MoveToAnotherScene();
    }

    public string GetPlayerName()
    {
        return "Joueur " + _playerName.Substring(_playerName.Length - 1);
    }

    public void SetGameActions(GameActions gameActions)
    {
        _gameActions = gameActions;
    }

    public void SetPopUpSystem(PopUpSystem popUpSystem)
    {
        _popUpSystem = popUpSystem;
    }

    public void SetHelper(Helper helper)
    {
        _helper = helper;
    }

    public void SendDiceResult(DiceFaces result)
    {
        _messageSender.Send(MessageQuery.RollDice, result.ToString());
        _helper.DiceResultExplanation(result);
    }

    public void ValidateChoice()
    {
        _messageSender.Send(MessageQuery.ValidateChoice);
    }

    public void EndTurn()
    {
        _messageSender.Send(MessageQuery.FinishTurn);
        _helper.UpdateHelpMessage("Ce n'est pas votre tour, mais vous pouvez toujours consulter votre réserve...");
    }

    public void SendChosenTile(String tileName)
    {
        _messageSender.Send(MessageQuery.ChosenTile, tileName);
        _helper.UpdateHelpMessage("Vous avez sélectionné une tuile.");
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
     * Generic message receiver
     */
    private void ReceiveGameMessages(object sender, MessageEventArgs args)
    {
        var parser = new MessageParser(args.Data);
        
        Debug.Log("--------- RECEIVED : " + parser.GetFullMessage());
        switch(parser.GetQuery())
        {

            //METEO ACTIONS
            case MessageQuery.WaitingDiceResult:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.StartTurn();
                });
                break;
            case MessageQuery.WaitingChoseRain:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                   _popUpSystem.PopUp("Il pleut ! Vous pouvez faire pousser un bambou sur la tuile de votre choix.");
                   _helper.UpdateHelpMessage("Il pleut ! Vous pouvez faire pousser un bambou sur la tuile de votre choix.");
                });
                break;
            case MessageQuery.WaitingMoveFarmer:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Vous pouvez déplacer le jardinier.");
                    _helper.UpdateHelpMessage("Vous pouvez déplacer le jardinier.");
                });
                break;
            
            //CHOSEN ACTIONS ON THE TABLE
            case MessageQuery.ValidateChoice:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.ValidateChoice(bool.Parse(parser.GetMessageBody()));
                });
                break;
            case MessageQuery.WaitingChoseAction :
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Placez vos pions sur la table pour choisir vos actions.");
                    _helper.UpdateHelpMessage("Placez vos pions sur la table pour choisir vos actions.");
                });
                break;
           
            //CARD INTERACTIONS
            case MessageQuery.WaitingPickCard:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Veuillez piocher une carte.");
                    _helper.UpdateHelpMessage("Veuillez piocher une carte.");
                });
                break;
            case MessageQuery.ReceivedCard:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.HidePopUp();
                    _gameActions.AddCardToHand(parser.GetMessageBody());
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
                    _popUpSystem.PopUp("Votre objectif a été validé !");
                    _gameActions.ValidateObjective(parser.GetMessageBody());
                });
                break;
            
            //TILE INTERACTIONS
            case MessageQuery.WaitingPickTiles:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Veuillez piocher des tuiles.");
                    _helper.UpdateHelpMessage("Veuillez piocher des tuiles.");
                });
                break;
            case MessageQuery.WaitingChoseTile :
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.HidePopUp();
                    _helper.UpdateHelpMessage("Veuillez sélectionner une tuile et la placer sur la table.");
                    _gameActions.DisplayTilesToChoose(parser.GetMessageBody());
                });
                break;
            case MessageQuery.TilePlaced :
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.TilePlaced();
                });
                break;
            case MessageQuery.WaitingEndTurn:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.WaitingEndTurn();
                    _helper.UpdateHelpMessage("Vous avez terminé vos actions. Vous pouvez valider un objectif ou finir votre tour.");
                });
                break;
        }
    }

}