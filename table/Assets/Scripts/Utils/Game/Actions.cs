using System;
using UnityEngine;

public enum Actions
{
    MoveFarmer,
    MovePanda,
    PickCard,
    PlaceTile,
}


public class ActionsMethods : MonoBehaviour
{
    public static string ToString(Actions action)
    {
        string? res = Enum.GetName(typeof(Actions), action);
        if (res == null) return "PickCard";
        return res;
    }

    public static Actions ToActions(string action)
    {
        try
        {
            return (Actions)Enum.Parse(typeof(Actions), action);
        }
        catch (Exception)
        {
            return Actions.PickCard;
        }
    }
}
