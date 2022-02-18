using server.Game.Board;
using server.SocketRooms;
using server.Utils.Game;
using server.Utils.Protocol;

namespace server.Game.GameActions.DiceActions;

public class CloudAction: DiceAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        /*
        player.SendEvent(MessageQuery.WaitingPickUpgrade);
        UpgradeType upgrade = game.PickUpgrade(table.WaitForUpgradePick());
        if (!upgrade.Equals(UpgradeType.None)) player.GiveUpgrade(upgrade);
        else player.SendEvent(MessageQuery.Error, "No more upgrade in that deck!");
        */
        
    }
}