using server.Game.Board;
using server.SocketRooms;

namespace server.Game.GameActions.DiceActions;

public class WindAction: DiceAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        Console.WriteLine("Player " + player.GetNumber() + " can now play the same action twice!");
        player.CanPlayPowerTwice = true;
    }
}