using server.Utils.Game;

namespace server.Game.GameActions.PowerActions;

public abstract class PowerAction : GameAction
{
    public static PowerAction GetPower(Powers power)
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