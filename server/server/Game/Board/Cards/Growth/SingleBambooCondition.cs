using server.Game.Board.Tiles;

namespace server.Game.Board.Cards.Growth;

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
        return    tile.GetColor().Equals(_color)
               && tile.GetGrowthAmount().Equals(_size);
    }
}