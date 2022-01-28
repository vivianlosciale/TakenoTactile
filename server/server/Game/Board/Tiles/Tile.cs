using server.Game.Board.Field;
using server.Game.Board.Upgrades;

namespace server.Game.Board.Tiles;

public class Tile
{
    private readonly TileColor _color;
    private int _growthAmount;
    private readonly bool _hasPermanentUpgrade;
    private UpgradeStrategy _upgrade;
    private readonly Tile?[] _neighbors = { null , null , null , null , null , null };
    private readonly bool[] _irrigation = { false, false, false, false, false, false};

    public Tile(TileColor color, UpgradeStrategy upgrade)
    {
        _color = color;
        _hasPermanentUpgrade = true;
        _upgrade = upgrade;
    }

    public Tile(TileColor color)
    {
        _color = color;
        _hasPermanentUpgrade = false;
        _upgrade = new NoUpgrade();
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

    public void AddNeighbor(RelativePosition position, Tile neighbor)
    {
        if (_neighbors[(int)position] != null)
        {
            Console.Error.WriteLine("Neighbor at position "+(int)position+" already assigned!");
        }
        _neighbors[(int)position] = neighbor;
    }

    public void AddIrrigation(RelativePosition position)
    {
        if (_irrigation[(int)position])
        {
            Console.Error.WriteLine("Irrigation at position "+(int)position+" already assigned!");
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
}