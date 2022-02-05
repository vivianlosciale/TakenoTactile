using server.Game.Board.Cards;
using server.Game.Board.Tiles;

namespace server.Game.Board;

public class Deck
{
    private readonly List<VictoryCard> _victoryCards = new();
    private readonly List<Tile> _tiles = new();

    public Deck()
    {
        _victoryCards.Add(new FoodVictoryCard("card_panda_3",3,new FoodStorage(2,0,0)));
        _victoryCards.Add(new FoodVictoryCard("card_panda_4",4,new FoodStorage(0,2,0)));
        _victoryCards.Add(new FoodVictoryCard("card_panda_5",5,new FoodStorage(0,0,2)));
        _victoryCards.Add(new FoodVictoryCard("card_panda_6",6,new FoodStorage(1,1,1)));
    }

    public VictoryCard? PickCard()
    {
        return Pick(_victoryCards);
    }

    public Tile? PickTile()
    {
        return Pick(_tiles);
    }
    
    private T? Pick<T>(List<T> list)
    {
        if (list.Count == 0) return default;
        int random = new Random(1234567890).Next();
        T obj = list[random%list.Count];
        list.Remove(obj);
        return obj;
    }
}