using server.Game.Board;
using server.SocketRooms;

namespace server.Game.GameActions.PowerActions;

public class MovePanda: PowerAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        // TODO
        Console.WriteLine("Player "+player.GetNumber()+" has no right to use that much power !!!");
    }
}