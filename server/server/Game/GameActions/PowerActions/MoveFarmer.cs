using server.Game.Board;
using server.Game.Board.Fields;
using server.Game.Board.Tiles;
using server.SocketRooms;
using server.Utils.Game;
using server.Utils.Protocol;

namespace server.Game.GameActions.PowerActions;

public class MoveFarmer: PowerAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        /*
        player.SendEvent(MessageQuery.WaitingMoveFarmer);
        table.SendEvent(MessageQuery.WaitingMoveFarmer);
        
        PositionDto chosenPosition = table.WaitForSelectPosition();
        Tile? tile = game.GetTile(new Position(chosenPosition.I, chosenPosition.J));
        
        if (tile == null)
        {
            player.SendEvent(MessageQuery.Error, "No tile at position ("+chosenPosition+")");
            Use(player, table, game);
        }
        else if (tile.CanGrow())
        {
            tile.Grow();
            player.SendEvent(MessageQuery.RainPower, "true");
            table.SendEvent(MessageQuery.RainPower, chosenPosition.ToString());
        }
        
        else player.SendEvent(MessageQuery.RainPower, "false");
        Console.WriteLine("Player "+player.GetNumber()+" has no right to use that much power !!!");
        */
        
    }
}