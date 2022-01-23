using server.SocketRooms;

namespace server.Game;

public class Takenoko
{

    private TableRoom _table;
    private List<PlayerRoom> _players;

    public Takenoko(TableRoom table, List<PlayerRoom> players)
    {
        _table = table;
        _players = players;
    }

    public void StartGame()
    {
        
    }

}