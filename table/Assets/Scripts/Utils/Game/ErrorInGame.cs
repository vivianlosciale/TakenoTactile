public enum ErrorInGameType
{
    FarmerToPlace,
    FarmerToRemove,
    PandaToPlace,
    PandaToRemove,
    BambooToPlace,
    BambooToRemove,
}

public class ErrorInGame
{
    public string tuioValue { get; }
    public string message { get; }
    public ErrorInGameType errorType { get; }
    public Tile errorTile { get; }

    public ErrorInGame(string tuioValue, string message, ErrorInGameType errorType, Tile tile)
    {
        this.tuioValue = tuioValue;
        this.message = message;
        this.errorType = errorType;
        this.errorTile = tile;
    }
}
