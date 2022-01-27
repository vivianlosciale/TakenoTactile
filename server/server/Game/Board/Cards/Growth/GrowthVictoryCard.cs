namespace server.Game.Board.Cards.Growth;

public class GrowthVictoryCard : VictoryCard
{
    private readonly GrowthCondition _condition;

    public GrowthVictoryCard(string name, GrowthCondition condition) : base(name)
    {
        _condition = condition;
    }

    public override bool IsValid(GameState gameState)
    {
        return _condition.Match(gameState.GetField());
    }
}