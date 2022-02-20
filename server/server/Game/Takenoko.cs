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
    private readonly GameState _gameState = new();
    private PlayerRoom _currentPlayer;
    private readonly int _neededValidations;

    public Takenoko(TableRoom table, List<PlayerRoom> players)
    {
        _table = table;
        _players = players;
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
        while (!APlayerWon())
        {
            PlayATurn();
            SwitchPlayer();
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

    private void SwitchPlayer()
    {
        if (_players.Count == 0) throw new Exception("No players found !");
        PlayerRoom newCurrent = _players[(_players.IndexOf(_currentPlayer) + 1) % _players.Count];
        newCurrent.SetPlaying(true);
        _currentPlayer.SetPlaying(false);
        _currentPlayer = newCurrent;
    }

    private bool APlayerWon()
    {
        foreach (PlayerRoom player in _players)
        {
            if (player.FinishedGame())
            {
                return true;
            }
        }
        return false;
    }
    
    private void PlayDice()
    {
        DiceFaces diceFace = _currentPlayer.WaitingDiceResult();
        Console.WriteLine("Player "+_currentPlayer.GetNumber()+" rolled '"+DiceFacesMethods.ToString(diceFace)+"'");
        _table.SendEvent(MessageQuery.RollDice, diceFace.ToString());
        DiceAction.GetPower(diceFace).Use(_currentPlayer,_table,_gameState);
    }

    private void PlayPower()
    {
        List<Actions> chosenPowers = _table.ChosePowers(_currentPlayer);
        foreach (Actions power in chosenPowers)
        {
            Console.WriteLine("--- POWER " + power + " ---");
            PowerAction.GetPower(power).Use(_currentPlayer,_table,_gameState);
        }
    }

    public bool ValidateCard(VictoryCard card)
    {
        bool result = card.IsValid(_gameState, _currentPlayer);
        if (result) card.Validate(_gameState, _currentPlayer);
        return result;
    }

    public int GetNeededValidations()
    {
        return _neededValidations;
    }

    public void SendToTable(MessageQuery query, string message)
    {
        _table.SendEvent(query, message);
    }
}