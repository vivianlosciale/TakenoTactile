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
    private bool _isPlaying;
    private bool _finishedGame = false;
    private DiceFaces _diceRoll = DiceFaces.None;
    private bool _endTurn;
    private readonly List<Powers> _chosenPowers = new();

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
                Console.WriteLine("Client "+_playerNumber+" said: " + message.GetBody());
                break;
            case MessageQuery.RollDice:
                //Console.WriteLine("Player "+_playerNumber+" rolled the dice");
                _diceRoll = DiceFacesMethods.ToDiceFace(message.GetBody());
                break;
            case MessageQuery.ValidateObjective:
                Console.WriteLine("Client "+_playerNumber+" wants to validate the objective '"+message.GetBody()+"'");
                ValidateCard(message.GetBody());
                break;
            case MessageQuery.FinishTurn:
                //Console.WriteLine("Player " + _playerNumber + " finished their turn");
                _endTurn = true;
                break;
        }
    }
    
    public bool FinishedGame()
    {
        return _finishedGame;
    }
    
    private void ValidateCard(string cardName)
    {
        foreach (VictoryCard card in _victoryCards)
        {
            if (!card.GetName().Equals(cardName)) continue;
            if (!_game.ValidateCard(card)) continue;
            Sender.Send(MessageQuery.ValidateObjective, cardName);
        }
    }

    public DiceFaces GetDiceResult()
    {
        _diceRoll = DiceFaces.None;
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
        _endTurn = false;
        Console.WriteLine("Waiting for player " + _playerNumber + " to end their turn...");
        while (!_endTurn) WaitSeconds(1);
    }

    public List<Powers> ChosePowers(DiceFaces diceFace)
    {
        _chosenPowers.Clear();
        _chosenPowers.Add(Powers.PickCard); // TODO remove
        // TODO
        return new List<Powers>(_chosenPowers);
    }
}