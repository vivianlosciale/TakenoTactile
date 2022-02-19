using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int id;
    private bool _canChoseAction;

    private GameObject board;
    private PawnEvent pawnEvent;

    public Player(int id)
    {
        this.id = id;
    }

    internal bool CanChoseAction()
    {
        return _canChoseAction;
    }

    internal void ChangeChoseAction()
    {
        _canChoseAction = !_canChoseAction;
    }

    public void SetBoard(GameObject board)
    {
        this.board = board;
        this.pawnEvent = board.GetComponent<PawnEvent>();
    }

    public GameObject GetBoard()
    {
        return board;
    }

    internal void ValidateObjective(string objectiveName)
    {
        this.board.GetComponent<PawnEvent>().AddCardToBoard(objectiveName);
    }

    internal IEnumerator ShowWeatherImage(string weather)
    {
        return pawnEvent.ShowWeatherImage(weather);
    }

    internal IEnumerator RemoveWeatherImage()
    {
        return pawnEvent.RemoveWeatherImage();
    }

    internal IEnumerator AddIcon(string iconName)
    {
        return pawnEvent.AddIcon(iconName);
    }

    internal IEnumerator RemoveIcon(string iconName)
    {
        return pawnEvent.RemoveIcon(iconName);
    }

    internal IEnumerator RemoveAllIcon()
    {
        return pawnEvent.RemoveAllIcon();
    }

    internal IEnumerator UseAction()
    {
        return pawnEvent.UseAction();
    }
}
