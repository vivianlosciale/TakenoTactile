using server.SocketRooms;

namespace server.Game.Board.Cards;

public class FoodVictoryCard : VictoryCard
{
    private readonly FoodStorage _condition;

    public FoodVictoryCard(string name, int value, FoodStorage condition) : base(name, value)
    {
        _condition = condition;
    }
    
    public override bool IsValid(GameState gameState, PlayerRoom currentPlayer)
    {
        return _condition.Match(currentPlayer.GetFoodStorage());
    }

    public override void Validate(GameState gameState, PlayerRoom currentPlayer)
    {
        currentPlayer.GetFoodStorage().Dispose(_condition);
    }
}