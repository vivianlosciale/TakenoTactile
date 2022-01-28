namespace server.Game.Board.Cards.Food;

public class FoodVictoryCard : VictoryCard
{
    private readonly FoodCondition _condition;

    public FoodVictoryCard(string name, int value, FoodCondition condition) : base(name, value)
    {
        _condition = condition;
    }
    
    public override bool IsValid(GameState gameState)
    {
        FoodStorage currentPlayerStorage = gameState.GetCurrentPlayerFoodStorage();
        return _condition.Match(currentPlayerStorage);
    }
}