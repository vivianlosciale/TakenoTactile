using server.Game.Board.Cards;
using server.Game.Board.Decks;
using server.Game.Board.Fields;
using server.Game.Board.Tiles;
using server.SocketRooms;
using server.Utils.Game;

namespace server.Game.Board;

public class GameState
{
    private readonly FieldCardsDeck _fieldCardsDeck = new();
    private readonly FoodCardsDeck _foodCardsDeck = new();
    private readonly GrowthCardsDeck _growthCardsDeck = new();
    private readonly TileDeck _tilesDeck = new();
    private readonly List<PlayerRoom> _players;
    private readonly Field _field = new();
    private PlayerRoom? _currentPlayer;

    public GameState(List<PlayerRoom> players)
    {
        _players = players;
    }

    public FoodStorage GetCurrentPlayerFoodStorage()
    {
        if (_currentPlayer == null) throw new Exception("No current player was found !");
        return _currentPlayer.GetFoodStorage(); 
    }

    public PlayerRoom NextPlayerTurn()
    {
        if (_players.Count == 0) throw new Exception("No current player was found !");
        PlayerRoom oldPlayer = _currentPlayer ?? _players[0];
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

    public VictoryCard? PickCard(CardTypes type)
    {
        switch (type)
        {
            case CardTypes.Bamboo:
                return _growthCardsDeck.Pick();
            case CardTypes.Land:
                return _fieldCardsDeck.Pick();
            case CardTypes.Panda:
            default:
                return _foodCardsDeck.Pick();
        }
    }

    public Tile? PickTile()
    {
        return _tilesDeck.Pick();
    }

    public Tile? GetTile(Position position)
    {
        return _field.GetTile(position);
    }

    public void PlaceTile(Position position, Tile tile)
    {
        _field.AddTile(position, tile);
    }

    public void ReturnTile(Tile tile)
    {
        _tilesDeck.Return(tile);
    }
}