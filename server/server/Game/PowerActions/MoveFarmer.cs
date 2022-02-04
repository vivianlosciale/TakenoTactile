using server.Game.Board;
using server.SocketRooms;

namespace server.Game.PowerActions;

public class MoveFarmer: Power
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        // TODO
        Console.WriteLine("Player "+player.GetNumber()+" has no right to use that much power !!!");
    }
}