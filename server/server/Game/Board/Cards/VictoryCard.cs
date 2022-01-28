namespace server.Game.Board.Cards;

public abstract class VictoryCard
{
    private string _name;

    protected VictoryCard(string name)
    {
        _name = name;
    }

    public abstract bool IsValid(GameState gameState);
}