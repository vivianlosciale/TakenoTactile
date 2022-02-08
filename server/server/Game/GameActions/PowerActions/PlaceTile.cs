using server.Game.Board;
using server.Game.Board.Field;
using server.Game.Board.Tiles;
using server.SocketRooms;
using server.Utils.Game;
using server.Utils.Protocol;

namespace server.Game.GameActions.PowerActions;

public class PlaceTile: PowerAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        player.SendEvent(MessageQuery.WaitingPickTiles);
        table.WaitForTilesPick();
        List<Tile> pickedTiles = PickTiles(game, 3);
        if (pickedTiles.Count == 0)
        {
            player.SendEvent(MessageQuery.Error, "No more tiles in the deck!");
            return;
        }
        Tile selectedTile = player.WaitingChoseTile(pickedTiles);
        table.SendEvent(MessageQuery.WaitingPlaceTile, selectedTile.ToString());
        PositionDto selectedPosition = table.WaitForSelectPosition();
        player.SendEvent(MessageQuery.TilePlaced);
        game.PlaceTile(new Position(selectedPosition.I, selectedPosition.J), selectedTile);
    }

    private List<Tile> PickTiles(GameState game, int number)
    {
        List<Tile> chosenTiles = new();
        for (int i = 0; i < number; i++)
        {
            Tile? tile = game.PickTile();
            if (tile == null) break;
            chosenTiles.Add(tile);
        }
        return chosenTiles;
    }
}