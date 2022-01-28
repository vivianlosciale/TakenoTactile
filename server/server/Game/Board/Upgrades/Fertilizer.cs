namespace server.Game.Board.Upgrades;

public class Fertilizer : NoUpgrade
{
    public override int MaxGrowthSize()
    {
        return 2;
    }
}