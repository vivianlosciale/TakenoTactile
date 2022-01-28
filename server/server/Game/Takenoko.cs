using server.Game.Board;
using server.Game.Board.Cards;
using server.SocketRooms;
using server.Utils.Protocol;

namespace server.Game;

public class Takenoko
{

    private readonly TableRoom _table;
    private readonly List<PlayerRoom> _players;
    private readonly GameState _gameState;
    private PlayerRoom _currentPlayer;

    public Takenoko(TableRoom table, List<PlayerRoom> players)
    {
        _table = table;
        _players = players;
        _gameState = new GameState(_players);
        _currentPlayer = new PlayerRoom(this,-1);
    }

    public void StartGame()
    {
        _currentPlayer = _players[0];
        _currentPlayer.SetPlaying(true);
        _table.SendEvent(MessageQuery.StartGame);
        foreach (PlayerRoom player in _players)
        {
            player.SendEvent(MessageQuery.StartGame);
        }
        while (!(_gameState.APlayerWon() || GameFinished()))
        {
            PlayATurn();
            _currentPlayer = _gameState.NextPlayerTurn();
        }
    }

    private void PlayATurn()
    {
        RollDice();
        PickACard();
        _currentPlayer.WaitForEndTurn();
    }

    private void PickACard()
    {
        _currentPlayer.SendEvent(MessageQuery.PickCard);
        _table.WaitForCardPick();
        VictoryCard card = _gameState.PickCard();
        _currentPlayer.GiveCard(card);
    }

    private void RollDice()
    {
        _currentPlayer.SendEvent(MessageQuery.RollDice);
        int diceFace = _currentPlayer.GetDiceResult();
        Console.WriteLine("The player rolled a " + diceFace);
    }

    private bool GameFinished()
    {
        return false;
        // TODO
    }

    public bool ValidateCard(VictoryCard card)
    {
        return card.IsValid(_gameState);
    }
}