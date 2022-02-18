using server.Game.Board.Cards;
using server.Game.Board.Tiles;

namespace server.Game.Board.Decks;

public class GrowthCardsDeck: Deck<GrowthVictoryCard>
{
    public GrowthCardsDeck()
    {
        GrowthCondition condition = new(4, TileColor.Green, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1gf",3,condition));
        
        condition = new(4, TileColor.Green, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1gn",5,condition));
        
        condition = new(4, TileColor.Green, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1gp",4,condition));
        
        condition = new(4, TileColor.Green, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1gw",4,condition));
        
        condition = new(4, TileColor.Pink, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1rf",5,condition));
        
        condition = new(4, TileColor.Pink, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1rn",7,condition));
        
        condition = new(4, TileColor.Pink, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1rp",6,condition));
        
        condition = new(4, TileColor.Pink, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1rw",6,condition));
        
        condition = new(4, TileColor.Yellow, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1yf",4,condition));
        
        condition = new(4, TileColor.Yellow, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1yn",6,condition));
        
        condition = new(4, TileColor.Yellow, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1yp",5,condition));
        
        condition = new(4, TileColor.Yellow, 1);
        Items.Add(new GrowthVictoryCard("card_bamboo_1yw",5,condition));
        
        condition = new(3, TileColor.Pink, 2);
        Items.Add(new GrowthVictoryCard("card_bamboo_2r",6,condition));
        
        condition = new(3, TileColor.Yellow, 3);
        Items.Add(new GrowthVictoryCard("card_bamboo_3y",7,condition));
        
        condition = new(3, TileColor.Green, 4);
        Items.Add(new GrowthVictoryCard("card_bamboo_4g",8,condition));
        
        Shuffle();
    }
}