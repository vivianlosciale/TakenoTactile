using server.Game.Board;
using server.SocketRooms;

namespace server.Game.GameActions.DiceActions;

public class SunAction: DiceAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        //Console.WriteLine("Player " + player.GetNumber() + " has now 3 actions to make!");
        //player.PowerUses += 1;
    }
}