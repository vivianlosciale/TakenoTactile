using server.Game;
using server.Game.Board.Cards;
using server.Game.Board.Tiles;
using server.Utils.Game;
using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class PlayerRoom : SocketRoom
{
    private readonly Takenoko _game;
    private readonly int _playerNumber;
    
    private readonly List<VictoryCard> _victoryCards = new();
    private readonly List<VictoryCard> _validatedCards = new();
    
    private bool _isPlaying;
    private bool _endTurn;
    public bool Validate;
    public bool CanPlayPowerTwice;
    public int PowerUses = 2;
    private string _chosenTile = string.Empty;
    private DiceFaces _diceRoll = DiceFaces.None;

    public PlayerRoom(Takenoko game, int playerNumber)
    {
        _game = game;
        _playerNumber = playerNumber;
    }
    
    public int GetNumber()
    {
        return _playerNumber;
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
                _chosenTile = message.GetBody();
                break;
            case MessageQuery.FinishTurn:
                _endTurn = true;
                break;
            default:
                Console.WriteLine("Unknown query: "+message.GetFullMessage());
                break;
        }
    }
    
    public bool FinishedGame()
    {
        return _validatedCards.Count >= _game.GetNeededValidations();
    }

    private void ResetPowerUses()
    {
        CanPlayPowerTwice = false;
        PowerUses = 2;
        Validate = false;
    }

    public void GiveCard(VictoryCard card)
    {
        _victoryCards.Add(card);
        Console.WriteLine("Sending card " + card.GetName() + " to player " + _playerNumber);
        Sender.Send(MessageQuery.ReceivedCard,card.GetName());
    }
    
    private void ValidateCard(string cardName)
    {
        Console.WriteLine("Player "+_playerNumber+" wants to validate the objective '"+cardName+"'");
        foreach (VictoryCard card in _victoryCards)
        {
            if (!card.GetName().Equals(cardName)) continue;
            bool valid = _game.ValidateCard(card);
            if (valid) _validatedCards.Add(card);
            Sender.Send(valid ? MessageQuery.ValidateObjective : MessageQuery.InvalidObjective, cardName);
            break;
        }
        _victoryCards.RemoveAll(_validatedCards.Contains);
    }

    public DiceFaces WaitingDiceResult()
    {
        _diceRoll = DiceFaces.None;
        Sender.Send(MessageQuery.WaitingDiceResult);
        Console.WriteLine("Waiting for player " + _playerNumber + " to roll the dice...");
        while (_diceRoll.Equals(DiceFaces.None)) WaitSeconds(1);
        return _diceRoll;
    }

    public void WaitForEndTurn()
    {
        _endTurn = false;
        Sender.Send(MessageQuery.WaitingEndTurn);
        Console.WriteLine("Waiting for player " + _playerNumber + " to end their turn...");
        while (!_endTurn) WaitSeconds(1);
        ResetPowerUses();
    }

    public Tile WaitingChoseTile(List<Tile> pickedTiles)
    {
        _chosenTile = string.Empty;
        Sender.Send(MessageQuery.WaitingChoseTile, MultiNames.ToMessage(pickedTiles));
        Console.WriteLine("Waiting for player " + _playerNumber + " to chose a tile...");
        while (_chosenTile == string.Empty) WaitSeconds(1);
        Console.WriteLine("The player chose the tile '"+_chosenTile+"'");
        foreach (var tile in pickedTiles)
        {
            if (tile.ToString().Equals(_chosenTile)) return tile;
        }
        return pickedTiles[0];
    }
}