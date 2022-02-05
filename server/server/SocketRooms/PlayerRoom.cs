using server.Game;
using server.Game.Board.Cards;
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
    private bool _canPlayPowerTwice;
    private int _powerUses;
    private DiceFaces _diceRoll = DiceFaces.None;
    private readonly List<Powers> _chosenPowers = new();

    public PlayerRoom(Takenoko game, int playerNumber)
    {
        _game = game;
        _playerNumber = playerNumber;
        InitGameVariables();
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
                //Console.WriteLine("Player "+_playerNumber+" rolled the dice");
                _diceRoll = DiceFacesMethods.ToDiceFace(message.GetBody());
                break;
            case MessageQuery.ValidateObjective:
                Console.WriteLine("Player "+_playerNumber+" wants to validate the objective '"+message.GetBody()+"'");
                ValidateCard(message.GetBody());
                break;
            case MessageQuery.ChosePower:
                Powers power = PowersMethods.ToPowers(message.GetBody());
                if (!_canPlayPowerTwice && _chosenPowers.Contains(power)) break;
                _chosenPowers.Add(power);
                Console.WriteLine("Power '"+message.GetBody()+"' selected!");
                break;
            case MessageQuery.FinishTurn:
                //Console.WriteLine("Player " + _playerNumber + " finished their turn");
                _endTurn = true;
                break;
        }
    }

    private void InitGameVariables()
    {
        _isPlaying = false;
        _endTurn = false;
        _canPlayPowerTwice = false;
        _powerUses = 2;
        _diceRoll = DiceFaces.None;
        _chosenPowers.Clear();
    }
    
    public bool FinishedGame()
    {
        return _validatedCards.Count >= _game.GetNeededValidations();
    }
    
    private void ValidateCard(string cardName)
    {
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

    public DiceFaces GetDiceResult()
    {
        Console.WriteLine("Waiting for player " + _playerNumber + " to roll the dice...");
        while (_diceRoll.Equals(DiceFaces.None)) WaitSeconds(1);
        return _diceRoll;
    }

    public void GiveCard(VictoryCard card)
    {
        _victoryCards.Add(card);
        Console.WriteLine("Sending card " + card.GetName() + " to player " + _playerNumber);
        Sender.Send(MessageQuery.ReceivedCard,card.GetName());
    }

    public void WaitForEndTurn()
    {
        Console.WriteLine("Waiting for player " + _playerNumber + " to end their turn...");
        while (!_endTurn) WaitSeconds(1);
        InitGameVariables();
    }

    public List<Powers> ChosePowers()
    {
        _chosenPowers.Add(Powers.PickCard); // TODO remove
        
        /* TODO add
        
        Console.WriteLine("Waiting for player " + _playerNumber + " to chose " + _powerUses + " powers...");
        Sender.Send(MessageQuery.ChosePower, _powerUses.ToString());
        while (_chosenPowers.Count < _powerUses) WaitSeconds(1);
        
         */
        
        return new List<Powers>(_chosenPowers);
    }

    public void AddPowerUses(int supUses)
    {
        _powerUses += supUses;
    }

    public void CanPlayPowerTwice(bool value)
    {
        _canPlayPowerTwice = value;
    }
}