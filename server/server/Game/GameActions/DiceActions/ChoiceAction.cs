using server.Game.Board;
using server.SocketRooms;
using server.Utils.Game;
using server.Utils.Protocol;

namespace server.Game.GameActions.DiceActions;

public class ChoiceAction: DiceAction
{
    public override void Use(PlayerRoom player, TableRoom table, GameState game)
    {
        player.SendEvent(MessageQuery.WaitingChoseWeather);
        DiceFaces diceFace = player.WaitingDiceFace();
        if (diceFace == DiceFaces.None) return;
        Console.WriteLine("Player "+player.GetNumber()+" chose '"+DiceFacesMethods.ToString(diceFace)+"'");
        table.SendEvent(MessageQuery.RollDice, diceFace.ToString());
        GetPower(diceFace).Use(player,table,game);
    }
}