using server.Game.Board.Tiles;
using server.Game.Board.Upgrades;

namespace server.Game.Board.Fields;

public class Field
{
    private readonly Dictionary<int, Dictionary<int, Tile>> _tiles;
    
    public Field()
    {
        Tile firstTile = new Tile("Initial", TileColor.None, new Castle());
        firstTile.AddIrrigation(RelativePosition.TopRight);
        firstTile.AddIrrigation(RelativePosition.Right);
        firstTile.AddIrrigation(RelativePosition.BottomRight);
        firstTile.AddIrrigation(RelativePosition.BottomLeft);
        firstTile.AddIrrigation(RelativePosition.Left);
        firstTile.AddIrrigation(RelativePosition.TopLeft);
        _tiles = new Dictionary<int, Dictionary<int, Tile>>
        {
            [0] = new ()
            {
                [0] = firstTile
            }
        };
    }

    public void AddTile(Position p, Tile tile)
    {
        if (!_tiles.ContainsKey(p.I))
        {
            _tiles[p.I] = new Dictionary<int, Tile>();
        }
        _tiles[p.I][p.J] = tile;
        foreach (RelativePosition relative in Position.Neighbors)
        {
            Tile? neighbor = GetTile(p.GetPositionAt(relative));
            if (neighbor == null) continue;
            tile.AddNeighbor(relative, neighbor);
            neighbor.AddNeighbor(Position.Opposite(relative), tile);
        }
    }

    public Tile? GetTile(Position p)
    {
        if (_tiles.ContainsKey(p.I))
        {
            if (_tiles[p.I].ContainsKey(p.J))
            {
                return _tiles[p.I][p.J];
            }
        }
        return null;
    }

    public bool GrowAt(Position position)
    {
        Tile? tile = GetTile(position);
        if (tile == null || !tile.CanGrow()) return false;
        tile.Grow();
        return true;
    }
}