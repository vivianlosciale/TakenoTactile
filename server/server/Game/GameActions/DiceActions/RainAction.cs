using server.Game.Board;
using server.Game.Board.Field;
using server.SocketRooms;
using server.Utils.Game;
using server.Utils.Protocol;

namespace server.Game.GameActions.DiceActions;

public class RainAction: DiceAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        /* TODO add
        
        player.SendEvent(MessageQuery.WaitingChoseRain);
        WaitChoice(player, table, game);
        
        */
    }

    private void WaitChoice(PlayerRoom player, TableRoom table, GameState game)
    {
        table.SendEvent(MessageQuery.WaitingChoseRain);
        PositionDto chosenPosition = table.WaitForSelectPosition();
        if (game.GrowAt(new Position(chosenPosition.I, chosenPosition.J)))
        {
            table.SendEvent(MessageQuery.Rain, chosenPosition.ToString());
        }
        else
        {
            player.SendEvent(MessageQuery.ImpossibleAction);
            WaitChoice(player, table, game);
        }
    }
}