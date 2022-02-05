using server.Game.Board.Tiles;

namespace server.Game.Board.Cards;

public class GrowthVictoryCard : VictoryCard
{
    private readonly List<SingleBambooCondition> _condition;

    public GrowthVictoryCard(string name, int value, List<SingleBambooCondition> condition) : base(name, value)
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

public class SingleBambooCondition
{
    private readonly int _size;
    private readonly TileColor _color;

    public SingleBambooCondition(int size, TileColor color)
    {
        _size = size;
        _color = color;
    }

    public bool IsValid(Tile tile)
    {
        return tile.GetColor().Equals(_color)
            && tile.GetGrowthAmount().Equals(_size);
    }
}