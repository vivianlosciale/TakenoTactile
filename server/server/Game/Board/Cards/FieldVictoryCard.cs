using server.Game.Board.Tiles;

namespace server.Game.Board.Cards;

public class FieldVictoryCard : VictoryCard
{
    private readonly Tile _condition;
    
    public FieldVictoryCard(string name, int value, Tile condition) : base(name, value)
    {
        _condition = condition;
    }

    public override bool IsValid(GameState gameState)
    {
        // TODO
        return false;
    }

    public override void Validate(GameState gameState) {}
}