namespace server.Game.Board.Upgrades;

public class NoUpgrade : UpgradeStrategy
{
    public override bool PandaCanStay()
    {
        return true;
    }

    //TODO
    public override bool BambooCanGrow()
    {
        return true; //should be false mais on fait true pour la démo pour faire pousser des bambous plus loin
    }

    public override int MaxGrowthSize()
    {
        return 1;
    }
}