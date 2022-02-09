using server.Game.Board.Cards;
using server.Game.Board.Fields;
using server.Game.Board.Tiles;

namespace server.Game.Board.Decks;

public class FieldCardsDeck: Deck<FieldVictoryCard>
{
    public FieldCardsDeck()
    {
        List<RelativePosition> positions = new List<RelativePosition>()
            {RelativePosition.TopRight, RelativePosition.Left, RelativePosition.TopRight};
        List<TileColor> colors = new List<TileColor>() 
            {TileColor.Green, TileColor.Yellow, TileColor.Yellow, TileColor.Green};
        Items.Add(new FieldVictoryCard("card_land_2g2yw", 3, BuildCondition(positions, colors)));
        
        Shuffle();
    }

    private Tile BuildCondition(List<RelativePosition> positions, List<TileColor> colors)
    {
        Tile result = new Tile("", colors[0]);
        Tile current = result;
        for (int i = 0; i < positions.Count; i++)
        {
            Tile neighbor = new Tile("", colors[i + 1]);
            current.AddNeighbor(positions[i], neighbor);
            current = neighbor;
        }
        return result;
    }
}