namespace server.Game.Board.Upgrades;

public class Lake : NoUpgrade
{
    public override int MaxGrowthSize()
    {
        return 0;
    }
}