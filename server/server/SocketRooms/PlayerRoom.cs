using server.Game;
using server.Game.Board.Cards;
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
    private int _diceRoll = -1;
    private bool _endTurn;

    public PlayerRoom(Takenoko game, int playerNumber)
    {
        _game = game;
        _playerNumber = playerNumber;
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
                Console.WriteLine("Client "+_playerNumber+" rolled the dice");
                _diceRoll = int.Parse(message.GetBody());
                break;
            case MessageQuery.ValidateObjective:
                Console.WriteLine("Client "+_playerNumber+" wants to validate the objective '"+message.GetBody()+"'");
                ValidateCard(message.GetBody());
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

    public int GetDiceResult()
    {
        _diceRoll = -1;
        while (_diceRoll < 0) WaitSeconds(1);
        return _diceRoll;
    }

    public void GiveCard(VictoryCard card)
    {
        _victoryCards.Add(card);
        Sender.Send(MessageQuery.PickCard,card.GetName());
    }

    public void WaitForEndTurn()
    {
        _endTurn = false;
        while (!_endTurn) WaitSeconds(1);
    }
}