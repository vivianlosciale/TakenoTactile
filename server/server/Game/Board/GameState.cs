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

    public FoodStorage GetCurrentPlayerFoodStorage()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].IsPlaying())
            {
                return _foodStorages[i];
            }
        }
        return new FoodStorage();
    }

    public Field.Field GetField()
    {
        return _field;
    }

    public Deck GetDeck()
    {
        return _deck;
    }
}