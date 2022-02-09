using server.Game.Board.Fields;
using server.Game.Board.Upgrades;

namespace server.Game.Board.Tiles;

public class Tile
{
    private readonly string _name;
    private readonly TileColor _color;
    private int _growthAmount;
    private readonly bool _hasPermanentUpgrade;
    private UpgradeStrategy _upgrade;
    private readonly Tile?[] _neighbors = { null , null , null , null , null , null };
    private readonly bool[] _irrigation = { false, false, false, false, false, false};

    public Tile(string name, TileColor color, UpgradeStrategy upgrade):
        this(name, color, true, upgrade) {}

    public Tile(string name, TileColor color):
        this(name, color, false, new NoUpgrade()) {}

    private Tile(string name, TileColor color, bool hasPermanentUpgrade, UpgradeStrategy upgrade)
    {
        _name = name;
        _color = color;
        _hasPermanentUpgrade = hasPermanentUpgrade;
        _upgrade = upgrade;
    }

    public TileColor GetColor()
    {
        return _color;
    }

    public int GetGrowthAmount()
    {
        return _growthAmount;
    }

    public bool CanGrow()
    {
        return _irrigation.Contains(true) || _upgrade.BambooCanGrow();
    }

    public void Grow()
    {
        _growthAmount = Math.Min(_growthAmount + _upgrade.MaxGrowthSize(), 4);
    }

    public bool CanEat()
    {
        return _upgrade.PandaCanStay();
    }

    public void Eat()
    {
        _growthAmount--;
    }

    public Tile? GetNeighbor(RelativePosition position)
    {
        return _neighbors[(int)position];
    }

    public List<Tile> GetNeighbors()
    {
        List<Tile> neighbors = new List<Tile>();
        foreach (Tile? neighbor in _neighbors)
        {
            if (neighbor != null) neighbors.Add(neighbor);
        }
        return neighbors;
    }

    public void AddNeighbor(RelativePosition position, Tile neighbor)
    {
        if (_neighbors[(int)position] != null) return;
        _neighbors[(int)position] = neighbor;
        if (_irrigation[(int)position])
        {
            neighbor.AddIrrigation(Position.Opposite(position));
        }
    }

    public void AddIrrigation(RelativePosition position)
    {
        if (_irrigation[(int)position])
        {
            Console.Error.WriteLine("Irrigation at position "+(int)position+" already assigned!");
            return;
        }
        _irrigation[(int)position] = true;
    }

    public bool CanSetUpgrade()
    {
        return _hasPermanentUpgrade;
    }

    public UpgradeStrategy SetUpgrade(UpgradeStrategy upgrade)
    {
        UpgradeStrategy oldUpgrade = _upgrade;
        _upgrade = upgrade;
        return oldUpgrade;
    }

    public override string ToString()
    {
        return _name;
    }
}