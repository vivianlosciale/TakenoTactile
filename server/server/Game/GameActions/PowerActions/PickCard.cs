using server.Game.Board;
using server.Game.Board.Cards;
using server.SocketRooms;
using server.Utils.Protocol;

namespace server.Game.GameActions.PowerActions;

public class PickCard: PowerAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        player.SendEvent(MessageQuery.WaitingPickCard);
        Console.WriteLine("Waiting for player "+player.GetNumber()+" to pick a card...");
        table.WaitForCardPick();
        VictoryCard? card = game.PickCard();
        if (card != null) player.GiveCard(card);
        else Console.WriteLine("Player "+player.GetNumber()+" didn't pick a card as the deck is empty... :(");
    }
}