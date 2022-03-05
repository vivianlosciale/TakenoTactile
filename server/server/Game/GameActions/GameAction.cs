using server.Game.Board;
using server.SocketRooms;

namespace server.Game.GameActions;

public abstract class GameAction
{
    public abstract void Use(PlayerRoom player, TableRoom table, GameState game);
}