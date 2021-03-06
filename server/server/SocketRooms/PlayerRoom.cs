using server.Game;
using server.Game.Board;
using server.Game.Board.Cards;
using server.Game.Board.Tiles;
using server.Utils.Game;
using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class PlayerRoom : SocketRoom
{
    private readonly string _reconnectionAddress;

    private bool _gameStarted;
    
    private readonly Takenoko _game;
    private readonly int _playerNumber;
    
    private readonly List<VictoryCard> _victoryCards = new();
    private readonly List<VictoryCard> _validatedCards = new();
    private readonly FoodStorage _foodStorage = new ();
    private readonly Dictionary<UpgradeType, int> _upgrades = new();
    
    private bool _isPlaying;
    private bool _endTurn;
    private string _chosenTileName = string.Empty;
    private DiceFaces _diceRoll = DiceFaces.None;
    
    public bool Validate;
    public bool CanPlayPowerTwice;
    public int PowerUses = 2;

    public PlayerRoom(Takenoko game, int playerNumber, string reconnectionAddress)
    {
        _reconnectionAddress = reconnectionAddress;
        _game = game;
        _playerNumber = playerNumber;
    }

    public PlayerRoom(PlayerRoom player)
    {
        _reconnectionAddress = player._reconnectionAddress;
        _gameStarted = player._gameStarted;
        Disconnected = player.Disconnected;
        _game = player._game;
        _playerNumber = player._playerNumber;
        _victoryCards = player._victoryCards;
        _validatedCards = player._validatedCards;
        _foodStorage = player._foodStorage;
        _upgrades = player._upgrades;
    }
    
    public int GetNumber()
    {
        return _playerNumber;
    }

    public FoodStorage GetFoodStorage()
    {
        return _foodStorage;
    }

    public void SetPlaying(bool playing)
    {
        _isPlaying = playing;
    }

    public bool IsPlaying()
    {
        return _isPlaying;
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        switch (message.GetQuery())
        {
            case MessageQuery.Ping:
                Console.WriteLine("Player "+_playerNumber+" said: " + message.GetBody());
                break;
            case MessageQuery.RollDice:
                _diceRoll = DiceFacesMethods.ToDiceFace(message.GetBody());
                break;
            case MessageQuery.ValidateObjective:
                ValidateCard(message.GetBody());
                break;
            case MessageQuery.ValidateChoice:
                Console.WriteLine("Choice validated!");
                Validate = true;
                break;
            case MessageQuery.ChosenTile:
                _chosenTileName = message.GetBody();
                break;
            case MessageQuery.WaitingFoodStorage:
                SendEvent(MessageQuery.FoodStorage, BambooDto.ToString(
                    _foodStorage.GetGreenAmount(),
                    _foodStorage.GetYellowAmount(),
                    _foodStorage.GetPinkAmount()));
                break;
            case MessageQuery.FinishTurn:
                _endTurn = true;
                break;
            default:
                Console.WriteLine("Unknown query: "+message.GetFullMessage());
                break;
        }
    }

    protected override void OnClose(CloseEventArgs e)
    {
        Console.WriteLine("<----- Player "+_playerNumber+" disconnected ----->");
        if (_gameStarted)
        {
            Disconnected = true;
            _game.SendToTable(MessageQuery.Disconnection, _playerNumber.ToString(), _reconnectionAddress);
        }
        else _game.SendToTable(MessageQuery.APlayerLeft, _playerNumber.ToString());
        _game.RemovePlayer(this);
    }

    protected override void OnOpen()
    {
        if (Disconnected) Reconnection();
    }

    private void Reconnection()
    {
        Disconnected = false;
        Sender.Send(MessageQuery.StartGame);
        foreach (VictoryCard card in _victoryCards)
        {
            Sender.Send(MessageQuery.ReceivedCard,card.GetName());
        }
        _game.SendToTable(MessageQuery.Reconnection, _playerNumber.ToString());
    }

    public bool FinishedGame(int neededValidations)
    {
        return _validatedCards.Count >= neededValidations;
    }

    public bool HasFullHand()
    {
        return _victoryCards.Count >= 5;
    }

    public int TotalPoints()
    {
        int result = 0;
        foreach (VictoryCard card in _validatedCards)
        {
            result += card.GetValue();
        }
        return result;
    }

    private void ResetPowerUses()
    {
        CanPlayPowerTwice = false;
        PowerUses = 2;
        Validate = false;
    }

    public void GiveCard(VictoryCard card)
    {
        _gameStarted = true;
        _victoryCards.Add(card);
        Console.WriteLine("Sending card " + card.GetName() + " to player " + _playerNumber);
        SendEvent(MessageQuery.ReceivedCard,card.GetName());
    }

    public void GiveUpgrade(UpgradeType upgrade)
    {
        _upgrades[upgrade]++;
    }
    
    private void ValidateCard(string cardName)
    {
        Console.WriteLine("Player "+_playerNumber+" wants to validate the objective '"+cardName+"'");
        foreach (VictoryCard card in _victoryCards)
        {
            if (!card.GetName().Equals(cardName)) continue;
            bool valid = _game.ValidateCard(this, card);
            if (valid) _validatedCards.Add(card);
            Sender.Send(valid ? MessageQuery.ValidateObjective : MessageQuery.InvalidObjective, cardName);
            if (valid) _game.SendToTable(MessageQuery.ValidateObjective, cardName);
            break;
        }
        _victoryCards.RemoveAll(_validatedCards.Contains);
    }

    public DiceFaces WaitingDiceResult()
    {
        if (Disconnected) return _diceRoll;
        _diceRoll = DiceFaces.None;
        SendEvent(MessageQuery.WaitingDiceResult);
        Console.WriteLine("Waiting for player " + _playerNumber + " to roll the dice...");
        while (_diceRoll.Equals(DiceFaces.None) && !Disconnected) WaitSeconds(1);
        return _diceRoll;
    }

    public DiceFaces WaitingDiceFace()
    {
        if (Disconnected) return _diceRoll;
        _diceRoll = DiceFaces.None;
        while (_diceRoll.Equals(DiceFaces.None) && !Disconnected) WaitSeconds(1);
        return _diceRoll;
    }

    public void WaitForEndTurn()
    {
        if (Disconnected) return;
        _endTurn = false;
        SendEvent(MessageQuery.WaitingEndTurn);
        Console.WriteLine("Waiting for player " + _playerNumber + " to end their turn...");
        while (!_endTurn && !Disconnected) WaitSeconds(1);
        ResetPowerUses();
    }

    public Tile? WaitingChoseTile(List<Tile> pickedTiles)
    {
        _chosenTileName = string.Empty;
        Sender.Send(MessageQuery.WaitingChoseTile, MultiNames.ToMessage(pickedTiles));
        Console.WriteLine("Waiting for player " + _playerNumber + " to chose a tile...");
        while (_chosenTileName == string.Empty && !Disconnected) WaitSeconds(1);
        Console.WriteLine("The player chose the tile '"+_chosenTileName+"'");
        foreach (var tile in pickedTiles)
        {
            if (tile.ToString().Equals(_chosenTileName)) return tile;
        }
        return default;
    }
}