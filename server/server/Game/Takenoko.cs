using server.Game.Board;
using server.SocketRooms;
using server.Utils.Protocol;

namespace server.Game;

public class Takenoko
{

    private readonly TableRoom _table;
    private readonly List<PlayerRoom> _players;
    private readonly GameState _gameState;

    public Takenoko(TableRoom table, List<PlayerRoom> players)
    {
        _table = table;
        _players = players;
        _gameState = new GameState(_players);
    }

    public void StartGame()
    {
        _table.SendEvent(MessageQuery.StartGame);
        foreach (PlayerRoom player in _players)
        {
            player.SendEvent(MessageQuery.StartGame);
        }
    }

}