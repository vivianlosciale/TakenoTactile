using server.SocketRooms;

namespace server.Game.Board.Cards;

public abstract class VictoryCard
{
    private readonly string _name;
    private readonly int _value;

    protected VictoryCard(string name, int value)
    {
        _name = name;
        _value = value;
    }

    public abstract bool IsValid(GameState gameState, PlayerRoom currentPlayer);
    public abstract void Validate(GameState gameState, PlayerRoom currentPlayer);

    public string GetName()
    {
        return _name;
    }

    public int GetValue()
    {
        return _value;
    }

    public override string ToString()
    {
        return _name;
    }
}