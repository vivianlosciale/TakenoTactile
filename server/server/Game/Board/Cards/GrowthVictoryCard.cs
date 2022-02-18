using server.Game.Board.Fields;
using server.Game.Board.Tiles;

namespace server.Game.Board.Cards;

public class GrowthVictoryCard : VictoryCard
{
    private readonly GrowthCondition _condition;

    public GrowthVictoryCard(string name, int value, GrowthCondition condition) : base(name, value)
    {
        _condition = condition;
    }

    public override bool IsValid(GameState gameState)
    {
        Tile? tile = gameState.GetTile(new Position(0, 0));
        if (tile != null)
        {
            int validTile = 0;
            List<Tile> visited = new List<Tile>() {tile};
            List<Tile> toVisit = new List<Tile>(tile.GetNeighbors());
            while (toVisit.Count > 0)
            {
                tile = toVisit[0];
                toVisit.Remove(tile);
                visited.Add(tile);
                if (_condition.IsValid(tile)) validTile++;
                if (validTile >= _condition.GetNumber()) return true;
                foreach (Tile neighbor in tile.GetNeighbors())
                {
                    if (!(visited.Contains(neighbor) || toVisit.Contains(neighbor))) toVisit.Add(neighbor);
                }
            }
        }
        return false;
    }

    public override void Validate(GameState gameState) {}
}

public class GrowthCondition
{
    private readonly int _size;
    private readonly TileColor _color;
    private readonly int _number;

    public GrowthCondition(int size, TileColor color, int number)
    {
        _size = size;
        _color = color;
        _number = number;
    }

    public int GetNumber()
    {
        return _number;
    }

    public bool IsValid(Tile tile)
    {
        return tile.GetColor().Equals(_color)
            && tile.GetGrowthAmount().Equals(_size);
    }
}