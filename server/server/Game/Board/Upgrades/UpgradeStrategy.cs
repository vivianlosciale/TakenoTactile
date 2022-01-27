namespace server.Game.Board.Upgrades;

public abstract class UpgradeStrategy
{
    public abstract bool PandaCanStay();
    public abstract bool BambooCanGrow();
    public abstract int  MaxGrowthSize();
}