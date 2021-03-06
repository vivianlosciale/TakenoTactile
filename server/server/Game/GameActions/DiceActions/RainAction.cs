using server.Game.Board;
using server.Game.Board.Fields;
using server.Game.Board.Tiles;
using server.SocketRooms;
using server.Utils.Game;
using server.Utils.Protocol;

namespace server.Game.GameActions.DiceActions;

public class RainAction: DiceAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        player.SendEvent(MessageQuery.WaitingChoseRain);
        table.SendEvent(MessageQuery.WaitingChoseRain);
        
        PositionDto? chosenPosition = table.WaitForSelectPosition(player);
        if (chosenPosition == default) return;
        Tile? tile = game.GetTile(new Position(chosenPosition.I, chosenPosition.J));
        
        if (tile == null)
        {
            player.SendEvent(MessageQuery.Error, "Aucune tuile à la position ("+chosenPosition+")");
            Use(player, table, game);
        }
        else if (tile.CanGrow())
        {
            tile.Grow();
            player.SendEvent(MessageQuery.RainPower, "true");
        }
        
        else player.SendEvent(MessageQuery.RainPower, "false");
    }
}