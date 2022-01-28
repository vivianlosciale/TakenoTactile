namespace server.Game.Board.Upgrades;

public class Fencing : NoUpgrade
{
    public override bool PandaCanStay()
    {
        return false;
    }
}