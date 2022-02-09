using server.Game.Board.Cards;

namespace server.Game.Board.Decks;

public class CardDeck: Deck<VictoryCard>
{
    public CardDeck()
    {
        Items.Add(new FoodVictoryCard("card_panda_3",3,new FoodStorage(2,0,0)));
        Items.Add(new FoodVictoryCard("card_panda_4",4,new FoodStorage(0,2,0)));
        Items.Add(new FoodVictoryCard("card_panda_5",5,new FoodStorage(0,0,2)));
        Items.Add(new FoodVictoryCard("card_panda_6",6,new FoodStorage(1,1,1)));

        Shuffle();
    }
}