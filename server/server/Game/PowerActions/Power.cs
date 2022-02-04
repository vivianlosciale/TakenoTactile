using server.Game.Board;
using server.SocketRooms;
using server.Utils.Game;

namespace server.Game.PowerActions;

public abstract class Power
{
    public abstract void Use(PlayerRoom player, TableRoom table, GameState game);

    public static Power GetPower(Powers power)
    {
        switch (power)
        {
            case Powers.MoveFarmer:
                return new MoveFarmer();
            case Powers.MovePanda:
                return new MovePanda();
            case Powers.PlaceTile:
                return new PlaceTile();
            default:
                return new PickCard();
        }
    }
}