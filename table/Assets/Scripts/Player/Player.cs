using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int id;
    private bool _canChoseAction;

    private GameObject board;

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
    }

    public GameObject GetBoard()
    {
        return board;
    }

    internal void ValidateObjective(string objectiveName)
    {
        this.board.GetComponent<PawnEvent>().AddCardToBoard(objectiveName);
    }
}
