using server.Utils.Game;

namespace server.Game.Board.Decks;

public class UpgradesDeck
{
    private int _fencing;
    private int _fertilizers;
    private int _watterTanks;

    public UpgradesDeck()
    {
        _fencing = 3;
        _fertilizers = 3;
        _watterTanks = 3;
    }
    
    public UpgradeType PickUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Fencing:
                _fencing--;
                if (_fencing <= 0) return UpgradeType.None;
                return type;
            case UpgradeType.Fertilizer:
                _fertilizers--;
                if (_fertilizers <= 0) return UpgradeType.None;
                return type;
            case UpgradeType.WatterTank:
            default:
                _watterTanks--;
                if (_watterTanks <= 0) return UpgradeType.None;
                return type;
        }
    }
}