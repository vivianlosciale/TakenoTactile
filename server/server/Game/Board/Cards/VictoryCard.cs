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

    public abstract bool IsValid(GameState gameState);
    public abstract void Validate(GameState gameState);

    public string GetName()
    {
        return _name;
    }
}