using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooDto
{
    private const char Separator = ':';

    public int _green;
    public int _yellow;
    public int _pink;

    private BambooDto(int green, int yellow, int pink)
    {
        _green = green;
        _yellow = yellow;
        _pink = pink;
    }

    public static string ToString(int green, int yellow, int pink)
    {
        return green.ToString() + Separator + yellow + Separator + pink;
    }

    public static BambooDto ToBambooDto(string msg)
    {
        string[] bamboos = msg.Split(Separator);
        return new BambooDto(int.Parse(bamboos[0]), int.Parse(bamboos[1]), int.Parse(bamboos[2]));
    }
}
