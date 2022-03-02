using server.Utils.Game;

namespace server.Game.GameActions.DiceActions;

public abstract class DiceAction: GameAction
{
    public static DiceAction GetPower(DiceFaces face)
    {
        switch (face)
        {
            case DiceFaces.Cloud:
                return new CloudAction();
            case DiceFaces.Rain:
                return new RainAction();
            case DiceFaces.Sun:
                return new SunAction();
            case DiceFaces.Thunder:
                return new ThunderAction();
            case DiceFaces.Wind:
                return new WindAction();
            case DiceFaces.Questionmark:
            default:
                return new ChoiceAction();
        }
    }
}