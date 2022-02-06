using server.Game.Board;
using server.Game.Board.Cards;
using server.Game.GameActions.DiceActions;
using server.Game.GameActions.PowerActions;
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
    private readonly int _neededValidations;

    public Takenoko(TableRoom table, List<PlayerRoom> players)
    {
        _table = table;
        _players = players;
        _gameState = new GameState(_players);
        _currentPlayer = new PlayerRoom(this,-1);
        _neededValidations = 5;
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
        while (!_gameState.APlayerWon())
        {
            PlayATurn();
            _currentPlayer = _gameState.NextPlayerTurn();
        }
    }

    private void PlayATurn()
    {
        Console.WriteLine("\n======== Player " + _currentPlayer.GetNumber() + " turn ========");
        _table.SendEvent(MessageQuery.CurrentPlayerNumber,_currentPlayer.GetNumber().ToString());
        PlayDice();
        PlayPower();
        _currentPlayer.WaitForEndTurn();
    }
    
    private void PlayDice()
    {
        _currentPlayer.SendEvent(MessageQuery.RollDice);
        DiceFaces diceFace = _currentPlayer.GetDiceResult();
        Console.WriteLine("Player "+_currentPlayer.GetNumber()+" rolled '"+DiceFacesMethods.ToString(diceFace)+"'");
        _table.SendEvent(MessageQuery.RollDice, diceFace.ToString());
        DiceAction.GetPower(diceFace).Use(_currentPlayer,_table,_gameState);
    }

    private void PlayPower()
    {
        List<Powers> chosenPowers = _table.ChosePowers();
        foreach (Powers power in chosenPowers)
        {
            PowerAction.GetPower(power).Use(_currentPlayer,_table,_gameState);
        }
    }

    public bool ValidateCard(VictoryCard card)
    {
        bool result = card.IsValid(_gameState);
        if (result) card.Validate(_gameState);
        return result;
    }

    public int GetNeededValidations()
    {
        return _neededValidations;
    }
}