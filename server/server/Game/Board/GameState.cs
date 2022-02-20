using server.Game.Board.Cards;
using server.Game.Board.Decks;
using server.Game.Board.Fields;
using server.Game.Board.Tiles;
using server.Utils.Game;

namespace server.Game.Board;

public class GameState
{
    private readonly FieldCardsDeck _fieldCardsDeck = new();
    private readonly FoodCardsDeck _foodCardsDeck = new();
    private readonly GrowthCardsDeck _growthCardsDeck = new();
    private readonly UpgradesDeck _upgradesDeck = new();
    private readonly TileDeck _tilesDeck = new();
    private readonly Field _field = new();

    public VictoryCard? PickCard(CardTypes type)
    {
        switch (type)
        {
            case CardTypes.Bamboo:
                return _growthCardsDeck.Pick();
            case CardTypes.Land:
                return _fieldCardsDeck.Pick();
            case CardTypes.Panda:
            default:
                return _foodCardsDeck.Pick();
        }
    }

    public UpgradeType PickUpgrade(UpgradeType type)
    {
        return _upgradesDeck.PickUpgrade(type);
    }

    public Tile? PickTile()
    {
        return _tilesDeck.Pick();
    }

    public Tile? GetTile(Position position)
    {
        return _field.GetTile(position);
    }

    public void PlaceTile(Position position, Tile tile)
    {
        _field.AddTile(position, tile);
    }

    public void ReturnTile(Tile tile)
    {
        _tilesDeck.Return(tile);
    }
}