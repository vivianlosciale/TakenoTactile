namespace server.Game.Board.Upgrades;

public class NoUpgrade : UpgradeStrategy
{
    public override bool PandaCanStay()
    {
        return true;
    }

    public override bool BambooCanGrow()
    {
        return false;
    }

    public override int MaxGrowthSize()
    {
        return 1;
    }
}