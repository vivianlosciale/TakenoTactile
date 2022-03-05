using server.Game.Board.Tiles;

namespace server.Game.Board.Decks;

public class TileDeck: Deck<Tile>
{
    public TileDeck()
    {
        Items.Add(new Tile("tiles_g1", TileColor.Green));
        Items.Add(new Tile("tiles_g2", TileColor.Green));
        Items.Add(new Tile("tiles_g3", TileColor.Green));
        Items.Add(new Tile("tiles_g4", TileColor.Green));
        Items.Add(new Tile("tiles_g5", TileColor.Green));
        
        Items.Add(new Tile("tiles_y1", TileColor.Yellow));
        Items.Add(new Tile("tiles_y2", TileColor.Yellow));
        Items.Add(new Tile("tiles_y3", TileColor.Yellow));
        Items.Add(new Tile("tiles_y4", TileColor.Yellow));
        Items.Add(new Tile("tiles_y5", TileColor.Yellow));
        Items.Add(new Tile("tiles_y6", TileColor.Yellow));

        Items.Add(new Tile("tiles_r1", TileColor.Pink));
        Items.Add(new Tile("tiles_r2", TileColor.Pink));
        Items.Add(new Tile("tiles_r3", TileColor.Pink));
        Items.Add(new Tile("tiles_r4", TileColor.Pink));
        
        
        Shuffle();
    }
}