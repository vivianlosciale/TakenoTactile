using server.Game.Board.Fields;
using server.Game.Board.Tiles;
using server.SocketRooms;

namespace server.Game.Board.Cards;

public class FieldVictoryCard : VictoryCard
{
    private readonly Tile _condition;
    
    public FieldVictoryCard(string name, int value, Tile condition) : base(name, value)
    {
        _condition = condition;
    }

    public override void Validate(GameState gameState, PlayerRoom currentPlayer) {}

    public override bool IsValid(GameState gameState, PlayerRoom currentPlayer)
    {
        Tile? tile = gameState.GetTile(new Position(0,0));
        if (tile == null) return false;
        List<Tile> toVisit = new();
        List<Tile> visited = new();
        toVisit.AddRange(tile.GetNeighbors());
        Console.WriteLine(); // TODO
        while (toVisit.Count > 0)
        {
            tile = toVisit[0];
            toVisit.Remove(tile);
            visited.Add(tile);
            if (CheckValidityFrom(tile)) return true;
            foreach (Tile neighbor in tile.GetNeighbors())
            {
                if (!(visited.Contains(neighbor) || toVisit.Contains(neighbor))) toVisit.Add(neighbor);
            }
        }
        return false;
    }

    private bool CheckValidityFrom(Tile tile)
    {
        if (!tile.GetColor().Equals(_condition.GetColor())) return false;
        for (int i = 0; i < 6; i++)
        {
            if (CheckValidityFrom(tile, _condition, i, new List<Tile>())) return true;
        }
        return false;
    }

    private bool CheckValidityFrom(Tile tile, Tile condition, int rotate, List<Tile> checkedTiles)
    {
        for (int i = 0; i < 6; i++)
        {
            RelativePosition conditionRelative = (RelativePosition) i;
            RelativePosition tileRelative = (RelativePosition) ((i + rotate) % 6);
            Tile? tileRelativeNeighbor = tile.GetNeighbor(tileRelative);
            Tile? conditionRelativeNeighbor = condition.GetNeighbor(conditionRelative);
            if (conditionRelativeNeighbor == null || checkedTiles.Contains(conditionRelativeNeighbor)) continue;
            if (tileRelativeNeighbor == null) return false;
            if (!tileRelativeNeighbor.GetColor().Equals(conditionRelativeNeighbor.GetColor())) return false;
            checkedTiles.Add(conditionRelativeNeighbor);
            return CheckValidityFrom(tileRelativeNeighbor, conditionRelativeNeighbor, rotate, checkedTiles);
        }
        return true;
    }
}