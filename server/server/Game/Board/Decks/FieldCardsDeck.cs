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

        positions = new List<RelativePosition>()
            {RelativePosition.TopRight,RelativePosition.Left,RelativePosition.TopRight};
        colors = new List<TileColor>()
            {TileColor.Pink,TileColor.Green,TileColor.Green,TileColor.Pink};
        Items.Add(new FieldVictoryCard("card_land_2r2gw", 4, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopRight,RelativePosition.Left,RelativePosition.TopRight};
        colors = new List<TileColor>()
            {TileColor.Yellow,TileColor.Pink,TileColor.Pink,TileColor.Yellow};
        Items.Add(new FieldVictoryCard("card_land_2r2yw", 5, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.TopRight};
        colors = new List<TileColor>()
            {TileColor.Green,TileColor.Green,TileColor.Green};
        Items.Add(new FieldVictoryCard("card_land_3gw1", 2, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.TopLeft};
        colors = new List<TileColor>()
            {TileColor.Green,TileColor.Green,TileColor.Green};
        Items.Add(new FieldVictoryCard("card_land_3gw2", 2, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.BottomLeft};
        colors = new List<TileColor>()
            {TileColor.Green,TileColor.Green,TileColor.Green};
        Items.Add(new FieldVictoryCard("card_land_3gw3", 2, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.TopRight};
        colors = new List<TileColor>()
            {TileColor.Pink,TileColor.Pink,TileColor.Pink};
        Items.Add(new FieldVictoryCard("card_land_3rw1", 4, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.TopLeft};
        colors = new List<TileColor>()
            {TileColor.Pink,TileColor.Pink,TileColor.Pink};
        Items.Add(new FieldVictoryCard("card_land_3rw2", 4, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.BottomLeft};
        colors = new List<TileColor>()
            {TileColor.Pink,TileColor.Pink,TileColor.Pink};
        Items.Add(new FieldVictoryCard("card_land_3rw3", 4, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.TopRight};
        colors = new List<TileColor>()
            {TileColor.Yellow,TileColor.Yellow,TileColor.Yellow};
        Items.Add(new FieldVictoryCard("card_land_3yw1", 3, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.TopLeft};
        colors = new List<TileColor>()
            {TileColor.Yellow,TileColor.Yellow,TileColor.Yellow};
        Items.Add(new FieldVictoryCard("card_land_3yw2", 3, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopLeft,RelativePosition.BottomLeft};
        colors = new List<TileColor>()
            {TileColor.Yellow,TileColor.Yellow,TileColor.Yellow};
        Items.Add(new FieldVictoryCard("card_land_3yw3", 3, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopRight,RelativePosition.Left,RelativePosition.TopRight};
        colors = new List<TileColor>()
            {TileColor.Green,TileColor.Green,TileColor.Green,TileColor.Green};
        Items.Add(new FieldVictoryCard("card_land_4gw", 3, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopRight,RelativePosition.Left,RelativePosition.TopRight};
        colors = new List<TileColor>()
            {TileColor.Pink,TileColor.Pink,TileColor.Pink,TileColor.Pink};
        Items.Add(new FieldVictoryCard("card_land_4rw", 5, BuildCondition(positions, colors)));

        positions = new List<RelativePosition>()
            {RelativePosition.TopRight,RelativePosition.Left,RelativePosition.TopRight};
        colors = new List<TileColor>()
            {TileColor.Yellow,TileColor.Yellow,TileColor.Yellow,TileColor.Yellow};
        Items.Add(new FieldVictoryCard("card_land_4yw", 4, BuildCondition(positions, colors)));
        
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