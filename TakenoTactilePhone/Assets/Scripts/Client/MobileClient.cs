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
    private Reserv _reserv;
    private bool _gameStarted;
    private string score;

    private void Start()
    {
        _gameStarted = false;
    }

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
        move.MoveToInGameScene();
    }

    public string GetPlayerName()
    {
        return "Joueur " + _playerName.Substring(_playerName.Length - 1);
    }

    internal void AskServerNbOfBamboo()
    {
        _messageSender.Send(MessageQuery.WaitingFoodStorage);
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

    public void SetReserv(Reserv reserv)
    {
        _reserv = reserv;
    }

    public bool GameIsStarted()
    {
        return _gameStarted;
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
            //AVENGERS START GAME
            case MessageQuery.StartGame:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameStarted = true;
                });
                break;
            case MessageQuery.WaitingDiceResult:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.StartTurn();
                });
                break;
            
            //METEO ACTIONS
            case MessageQuery.WaitingChoseRain:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                   _popUpSystem.PopUp("Action météo\nIl pleut ! Vous pouvez faire pousser un bambou sur la tuile de votre choix.");
                });
                break;
            case MessageQuery.WaitingChoseWeather:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.QuestionMark();
                });
                break;
            case MessageQuery.RainPower:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    var wasPlaced = bool.Parse(parser.GetMessageBody());
                    _popUpSystem.PopUp(wasPlaced
                        ? "Votre bambou a poussé ! Regardez comme il est beau !"
                        : "Une erreur s'est produite ! Malheureusement, nous ne pouvons pas gérer ce cas de figure. Veuillez recommencer la partie.");
                });
                break;
            case MessageQuery.WaitingChoseThunder:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Action météo\nSacré orage ! Déplacez le panda sur la tuile de votre choix, et croquez un bambou.");
                });
                break;
            
            //FARMER ACTIONS
            case MessageQuery.WaitingMoveFarmer:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Vous pouvez déplacer le jardinier.");
                    _helper.UpdateHelpMessage("Vous pouvez déplacer le jardinier.");
                });
                break;
            case MessageQuery.PlaceBamboo:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.PlaceBamboo(bool.Parse(parser.GetMessageBody()));
                });
                break;
            case MessageQuery.BambooPlaced:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Le jardinier est satisfait de la croissance de votre bambou.");
                });
                break;
            
            //PANDAS ACTIONS
            case MessageQuery.WaitingMovePanda:                
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Vous pouvez déplacer le panda.");
                    _helper.UpdateHelpMessage("Vous pouvez déplacer le panda.");
                });
                break;
            case MessageQuery.EatBamboo:                
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    if (bool.Parse(parser.GetMessageBody()))
                    {
                        _popUpSystem.PopUp("Vous pouvez croquer un bambou.");
                    }
                    else
                    {
                        _popUpSystem.PopUp("Rien à manger par ici...");
                    }
                });
                break;
            case MessageQuery.BambooEaten:                
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Le bambou croqué s'est ajouté à votre réserve.");
                });
                break;
            
            //CHOSEN ACTIONS ON THE TABLE
            case MessageQuery.WaitingChoseAction :
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _popUpSystem.PopUp("Placez vos pions sur la table pour choisir vos actions.");
                    _helper.UpdateHelpMessage("Placez vos pions sur la table pour choisir vos actions.");
                });
                break;
            case MessageQuery.ValidateChoice:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.ValidateChoice(bool.Parse(parser.GetMessageBody()));
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
            
            //RESERV INTERACTIONS
            case MessageQuery.FoodStorage:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _reserv.CreateBamboo(parser.GetMessageBody());
                });
                break;
            
            //AVENGERS END GAME
            case MessageQuery.EndGame:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                        Debug.Log("END GAME RECEIVED");
                        _serverSocket.Close();
                        var move = gameObject.GetComponent<MoveObject>();
                        move.MoveToEndGameScene(parser.GetMessageBody());
                    });
                break;
            
            //ERROR
            case MessageQuery.Error:
                ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                {
                    _gameActions.DisplayError(parser.GetMessageBody());
                });
                break;
        }
    }

}