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
            CardTypes type = table.WaitForCardPick(player);
            if (type == CardTypes.None) return;
            VictoryCard? card = game.PickCard(type);
            if (card != null) player.GiveCard(card);
            else player.SendEvent(MessageQuery.Error, "Il n'y a plus de cartes dans le paquet !");
        }
        else player.SendEvent(MessageQuery.Error, "Vous avez déjà 5 cartes dans votre main !");
    }
}