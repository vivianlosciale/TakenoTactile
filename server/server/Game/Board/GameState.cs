using server.Game.Board.Cards;
using server.Game.Board.Field;
using server.Game.Board.Tiles;
using server.SocketRooms;

namespace server.Game.Board;

public class GameState
{
    private readonly Deck _deck;
    private readonly List<PlayerRoom> _players;
    private readonly FoodStorage[] _foodStorages;
    private readonly Field.Field _field;

    public GameState(List<PlayerRoom> players)
    {
        _players = players;
        _foodStorages = new FoodStorage[_players.Count];
        for (int i = 0; i < _foodStorages.Length; i++)
        {
            _foodStorages[i] = new FoodStorage();
        }
        _deck = new Deck();
        _field = new Field.Field();
    }

    private PlayerRoom GetCurrentPlayerRoom()
    {
        foreach (PlayerRoom player in _players)
        {
            if (player.IsPlaying())
            {
                return player;
            }
        }
        return _players[0];
    }

    public FoodStorage GetCurrentPlayerFoodStorage()
    {
        return _foodStorages[_players.IndexOf(GetCurrentPlayerRoom())]; 
    }

    public Field.Field GetField()
    {
        return _field;
    }

    public Deck GetDeck()
    {
        return _deck;
    }

    public PlayerRoom NextPlayerTurn()
    {
        PlayerRoom currentPlayer = GetCurrentPlayerRoom();
        PlayerRoom nextPlayer = _players[(_players.IndexOf(currentPlayer) + 1) % _players.Count];
        nextPlayer.SetPlaying(true);
        currentPlayer.SetPlaying(false);
        return nextPlayer;
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

    public bool GrowAt(Position position)
    {
        return _field.GrowAt(position);
    }
}