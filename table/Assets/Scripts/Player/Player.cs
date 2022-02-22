using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int id;

    private bool _canChoseAction;
    private List<Actions> actions;

    private GameObject board;
    private PawnEvent pawnEvent;

    public Player(int id)
    {
        this.id = id;
        this.actions = new List<Actions>();
    }

    internal bool CanChoseAction(DiceFaces diceFaces, Actions action)
    {
        bool actionIsContained = actions.Contains(action);
        return actions.Count < (diceFaces.Equals(DiceFaces.Sun) ? actionIsContained ? -1 : 3 : diceFaces.Equals(DiceFaces.Wind) ? 2 : actionIsContained ? -1 : 2) && _canChoseAction;
    }

    internal bool CanRemoveAction()
    {
        return _canChoseAction;
    }

    internal void ChoseAction(Actions action)
    {
        actions.Add(action);
    }

    internal void RemoveAction(Actions action)
    {
        actions.Remove(action);
    }

    internal void ChangeChoseAction()
    {
        _canChoseAction = !_canChoseAction;
        actions = new List<Actions>();
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
