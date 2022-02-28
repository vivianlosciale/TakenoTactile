using server.Game.Board;
using server.Game.Board.Fields;
using server.Game.Board.Tiles;
using server.SocketRooms;
using server.Utils.Game;
using server.Utils.Protocol;

namespace server.Game.GameActions.PowerActions;

public class MovePanda: PowerAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        /*
        player.SendEvent(MessageQuery.WaitingMovePanda);
        table.SendEvent(MessageQuery.WaitingMovePanda);
        
        PositionDto chosenPosition = table.WaitForSelectPosition();
        Tile? tile = game.GetTile(new Position(chosenPosition.I, chosenPosition.J));
        
        if (tile == null)
        {
            player.SendEvent(MessageQuery.Error, "No tile at position ("+chosenPosition+")");
            Use(player, table, game);
        }
        else if (tile.CanEat())
        {
            tile.Eat();
            int green = tile.GetColor().Equals(TileColor.Green) ? 1 : 0;
            int yellow = tile.GetColor().Equals(TileColor.Yellow) ? 1 : 0;
            int pink = tile.GetColor().Equals(TileColor.Pink) ? 1 : 0;
            player.GetFoodStorage().Store(green,yellow,pink);
            
            player.SendEvent(MessageQuery.EatBamboo, "true");
            table.SendEvent(MessageQuery.EatBamboo, chosenPosition.ToString());
            Console.WriteLine("Waiting for a bamboo to be removed from the panda position!");
            table.WaitForObjectMoved();
        }
        else 
        {
            player.SendEvent(MessageQuery.EatBamboo, "false");
            Console.WriteLine("Player "+player.GetNumber()+" has no right to use that much power !!!");
        }
        */
    }
}