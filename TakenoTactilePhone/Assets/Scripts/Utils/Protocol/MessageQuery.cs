using System;

public enum MessageQuery
{
    // COMMON
    None,
    Ping,
    AcceptConnection,
    StartGame,
    WaitingPickCard, //on reçoit : on attend qu'on pioche une carte objectif
    WaitingPickTiles, //on reçoit : on attend qu'on pioche une tuile sur la table
    WaitingChoseRain, // on reçoit
    ImpossibleAction, // on reçoit 
    
    // PLAYERS
    PlayerBroadcast,
    PlayerConnection,
    GameIsFull,
    RollDice, // on reçoit
    ValidateObjective, // on envoie
    FinishTurn, // on envoie : Quand on a fini notre tour
    ReceivedCard, // on reçoit : la carte objectif qu'on vient de piocher sur la table
    ReceivedTiles, // on reçoit : les 3 tuiles qu'on vient de piocher
    ChosenTile, //on envoie : la tuile qu'on a choisie 
    TilePlaced,
    InvalidObjective,
    ValidateChoice, // on reçoit et on renvoie
    ChoseAction,
}

public static class QueryMethods
{
    public static string ToString( MessageQuery query)
    {
        string? res = Enum.GetName(typeof(MessageQuery), query);
        if (res == null) return "None";
        return res;
    }

    public static MessageQuery ToQuery(string query)
    {
        try
        {
            return (MessageQuery)Enum.Parse(typeof(MessageQuery), query);
        }
        catch (Exception)
        {
            return MessageQuery.None;
        }
    }
}