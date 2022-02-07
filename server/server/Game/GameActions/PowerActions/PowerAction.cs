using server.Utils.Game;

namespace server.Game.GameActions.PowerActions;

public abstract class PowerAction : GameAction
{
    public static PowerAction GetPower(Actions action)
    {
        switch (action)
        {
            case Actions.MoveFarmer:
                return new MoveFarmer();
            case Actions.MovePanda:
                return new MovePanda();
            case Actions.PlaceTile:
                return new PlaceTile();
            default:
                return new PickCard();
        }
    }
}