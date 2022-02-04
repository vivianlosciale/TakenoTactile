using server.Game.Board.Cards;
using server.Game.Board.Cards.Food;
using server.Game.Board.Tiles;

namespace server.Game.Board;

public class Deck
{
    private readonly List<VictoryCard> _victoryCards = new();
    private readonly List<Tile> _tiles = new();

    public Deck()
    {
        _victoryCards.Add(new FoodVictoryCard("card_panda_3",3,new FoodCondition(2,0,0)));
        _victoryCards.Add(new FoodVictoryCard("card_panda_4",4,new FoodCondition(0,2,0)));
        _victoryCards.Add(new FoodVictoryCard("card_panda_5",5,new FoodCondition(0,0,2)));
        _victoryCards.Add(new FoodVictoryCard("card_panda_6",6,new FoodCondition(1,1,1)));
    }

    public VictoryCard? PickCard()
    {
        if (_victoryCards.Count == 0) return null;
        Random r = new Random(1234567890);
        VictoryCard card = _victoryCards[r.Next()%_victoryCards.Count];
        _victoryCards.Remove(card);
        return card;
    }
}