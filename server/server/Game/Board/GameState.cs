using server.Game.Board.Cards;
using server.Game.Board.Field;
using server.Game.Board.Tiles;
using server.SocketRooms;

namespace server.Game.Board;

public class GameState
{
    private readonly Deck _deck;
    private readonly List<PlayerRoom> _players;
    private readonly Field.Field _field;
    private PlayerRoom _currentPlayer;

    public GameState(List<PlayerRoom> players)
    {
        _players = players;
        _currentPlayer = players[0];
        _deck = new Deck();
        _field = new Field.Field();
    }

    public FoodStorage GetCurrentPlayerFoodStorage()
    {
        return _currentPlayer.GetFoodStorage(); 
    }

    public PlayerRoom NextPlayerTurn()
    {
        PlayerRoom oldPlayer = _currentPlayer;
        _currentPlayer = _players[(_players.IndexOf(oldPlayer) + 1) % _players.Count];
        _currentPlayer.SetPlaying(true);
        oldPlayer.SetPlaying(false);
        return _currentPlayer;
    }

    public bool APlayerWon()
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

    public VictoryCard? PickCard()
    {
        return _deck.PickCard();
    }

    public Tile? PickTile()
    {
        return _deck.PickTile();
    }

    public Tile? GetTile(Position position)
    {
        return _field.GetTile(position);
    }

    public void PlaceTile(Position position, Tile tile)
    {
        _field.AddTile(position, tile);
    }
}