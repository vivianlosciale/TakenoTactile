using server.Game.Board;
using server.Game.Board.Cards;
using server.Game.PowerActions;
using server.SocketRooms;
using server.Utils.Game;
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
        Console.WriteLine("\n======== Player " + _currentPlayer.GetNumber() + " turn ========");
        _table.SendEvent(MessageQuery.CurrentPlayerNumber,_currentPlayer.GetNumber().ToString());
        DiceFaces diceFace = PlayDice();
        PlayPower(diceFace);
        _currentPlayer.WaitForEndTurn();
    }

    private DiceFaces PlayDice()
    {
        DiceFaces face = RollDice();
        // TODO Dice Powers
        return face;
    }
    
    private DiceFaces RollDice()
    {
        _currentPlayer.SendEvent(MessageQuery.RollDice);
        DiceFaces diceFace = _currentPlayer.GetDiceResult();
        Console.WriteLine("Player "+_currentPlayer.GetNumber()+" rolled '"+DiceFacesMethods.ToString(diceFace)+"'");
        _table.SendEvent(MessageQuery.RollDice, diceFace.ToString());
        return diceFace;
    }

    private void PlayPower(DiceFaces diceFace)
    {
        List<Powers> chosenPowers = _currentPlayer.ChosePowers(diceFace);
        foreach (Powers power in chosenPowers)
        {
            Power.GetPower(power).Use(_currentPlayer,_table,_gameState);
        }
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