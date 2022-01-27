namespace server.Game.Board.Cards.Field;

public class FieldVictoryCard : VictoryCard
{
    private readonly FieldCondition _condition;
    
    public FieldVictoryCard(string name, FieldCondition condition) : base(name)
    {
        _condition = condition;
    }

    public override bool IsValid(GameState gameState)
    {
        return _condition.Match(gameState.GetField());
    }
}