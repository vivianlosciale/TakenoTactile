using System.Collections.Generic;

public class Takenoko
{

    private List<PlayerRoom> _players;

    public void StartGame(List<PlayerRoom> players)
    {
        _players = players;
    }

}