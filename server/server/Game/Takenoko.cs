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

    private readonly Server _server;
    private readonly TableRoom _table;
    private readonly List<PlayerRoom> _players;
    private readonly GameState _gameState = new();
    private PlayerRoom _currentPlayer;
    private int _currentPlayerIndex;
    private readonly int _neededValidations;

    public Takenoko(TableRoom table, List<PlayerRoom> players, Server server)
    {
        _server = server;
        _table = table;
        _players = players;
        _currentPlayer = new(this,-1, "");
        _currentPlayerIndex = -1;
        _neededValidations = 2;
    }

    public void StartGame()
    {
        _players.Sort((p1, p2) => p1.GetNumber() - p2.GetNumber());
        _currentPlayerIndex = _players.Count - 1;
        _currentPlayer = _players[_currentPlayerIndex];
        _currentPlayer.SetPlaying(true);
        _table.SendEvent(MessageQuery.StartGame);
        foreach (PlayerRoom player in _players)
        {
            player.SendEvent(MessageQuery.StartGame);
            VictoryCard? bamboo = _gameState.PickCard(CardTypes.Bamboo);
            VictoryCard? land = _gameState.PickCard(CardTypes.Land);
            VictoryCard? panda = _gameState.PickCard(CardTypes.Panda);
            if (bamboo != default) player.GiveCard(bamboo);
            if (land != default) player.GiveCard(land);
            if (panda != default) player.GiveCard(panda);
        }
        PlayerRoom? winner = APlayerWon();
        while (winner == default(PlayerRoom))
        {
            SwitchPlayer();
            PlayATurn();
            winner = APlayerWon();
        }
        foreach (PlayerRoom player in _players)
        {
            player.SendEvent(MessageQuery.EndGame, player.TotalPoints().ToString());
        }
        _table.SendEvent(MessageQuery.EndGame, winner.GetNumber().ToString());
        Console.WriteLine("\n======== GAME ENDED ========\n");
        Console.WriteLine("        Player "+_currentPlayer.GetNumber()+" won\n\n");
        _server.EndGame();
    }

    private void PlayATurn()
    {
        Console.WriteLine("\n======== Player " + _currentPlayer.GetNumber() + " turn ========");
        _table.SendEvent(MessageQuery.CurrentPlayerNumber,_currentPlayer.GetNumber().ToString());
        PlayDice();
        if (_currentPlayer.IsDisconnected()) return;
        PlayPower();
        _currentPlayer.WaitForEndTurn();
    }

    private void SwitchPlayer()
    {
        if (_players.Count == 0) throw new Exception("No players found !");
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        PlayerRoom newCurrent = _players[_currentPlayerIndex];
        _currentPlayer.SetPlaying(false);
        _currentPlayer = newCurrent;
        if (_currentPlayer.IsDisconnected()) RefreshPlayer();
        else _currentPlayer.SetPlaying(true);
    }

    private void RefreshPlayer()
    {
        _currentPlayer = _players[_currentPlayerIndex];
        while (_currentPlayer.IsDisconnected())
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }

    private PlayerRoom? APlayerWon()
    {
        foreach (PlayerRoom player in _players)
        {
            if (player.FinishedGame(_neededValidations))
            {
                return player;
            }
        }
        return default;
    }

    private void PlayDice()
    {
        DiceFaces diceFace = _currentPlayer.WaitingDiceResult();
        if (diceFace.Equals(DiceFaces.None)) return;
        Console.WriteLine("Player " + _currentPlayer.GetNumber() + " rolled '" + DiceFacesMethods.ToString(diceFace) + "'");
        _table.SendEvent(MessageQuery.RollDice, diceFace.ToString());
        DiceAction.GetPower(diceFace).Use(_currentPlayer, _table, _gameState);
    }

    private void PlayPower()
    {
        List<Actions>? chosenPowers = _table.ChosePowers(_currentPlayer);
        if (chosenPowers == default) return;
        foreach (Actions power in chosenPowers)
        {
            Console.WriteLine("--- POWER " + power + " ---");
            PowerAction.GetPower(power).Use(_currentPlayer, _table, _gameState);
            if (_currentPlayer.IsDisconnected()) return;
        }
    }

    public bool ValidateCard(PlayerRoom player, VictoryCard card)
    {
        if (!player.Equals(_currentPlayer))
        {
            player.SendEvent(MessageQuery.Error, "Vous ne pouvez valider un objectif seulement à votre tour!");
            return false;
        }
        bool result = card.IsValid(_gameState, _currentPlayer);
        if (result) card.Validate(_gameState, _currentPlayer);
        return result;
    }

    public void RemovePlayer(PlayerRoom player)
    {
        _server.RemovePlayer(player);
    }

    public void SendToTable(MessageQuery query, string message)
    {
        _table.SendEvent(query, message);
    }
    
    public void SendToTable(MessageQuery query, string dest, string message)
    {
        _table.SendEvent(query, dest, message);
    }
}