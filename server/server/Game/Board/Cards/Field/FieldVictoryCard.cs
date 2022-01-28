namespace server.Game.Board.Cards.Field;

public class FieldVictoryCard : VictoryCard
{
    private readonly FieldCondition _condition;
    
    public FieldVictoryCard(string name, int value, FieldCondition condition) : base(name, value)
    {
        _condition = condition;
    }

    public override bool IsValid(GameState gameState)
    {
        return _condition.Match(gameState.GetField());
    }
}