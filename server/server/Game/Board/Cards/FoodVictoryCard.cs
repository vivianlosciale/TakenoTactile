namespace server.Game.Board.Cards;

public class FoodVictoryCard : VictoryCard
{
    private readonly FoodStorage _condition;

    public FoodVictoryCard(string name, int value, FoodStorage condition) : base(name, value)
    {
        _condition = condition;
    }
    
    public override bool IsValid(GameState gameState)
    {
        return _condition.Match(gameState.GetCurrentPlayerFoodStorage());
    }

    public override void Validate(GameState gameState)
    {
        gameState.GetCurrentPlayerFoodStorage().Dispose(_condition);
    }
}