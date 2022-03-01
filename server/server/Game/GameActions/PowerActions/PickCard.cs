using server.Game.Board;
using server.Game.Board.Cards;
using server.SocketRooms;
using server.Utils.Game;
using server.Utils.Protocol;

namespace server.Game.GameActions.PowerActions;

public class PickCard: PowerAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        if (!player.HasFullHand())
        {
            player.SendEvent(MessageQuery.WaitingPickCard);
            CardTypes type = table.WaitForCardPick();
            VictoryCard? card = game.PickCard(type);
            if (card != null) player.GiveCard(card);
            else player.SendEvent(MessageQuery.Error, "No more cards in that deck!");
        }
        else player.SendEvent(MessageQuery.Error, "You already have 5 cards in your hand!");
    }
}