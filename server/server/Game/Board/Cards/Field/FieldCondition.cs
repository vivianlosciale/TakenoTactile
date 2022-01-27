using server.Game.Board.Tiles;

namespace server.Game.Board.Cards.Field;

public class FieldCondition
{
    private readonly Tile _condition;

    public FieldCondition(Tile tile)
    {
        _condition = tile;
    }

    public bool Match(Board.Field.Field field)
    {
        // TODO !
        return true;
    }
}