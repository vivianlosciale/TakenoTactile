using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int id;
    private bool _canChoseAction;
    private int boardPosition;

    private GameObject board;

    public Player(int id, int boardPosition)
    {
        this.id = id;
        this.boardPosition = boardPosition;
    }

    internal bool CanChoseAction()
    {
        return _canChoseAction;
    }

    internal void ChangeChoseAction()
    {
        _canChoseAction = !_canChoseAction;
    }

    public int GetBoardPosition()
    {
        return boardPosition;
    }

    public void SetBoard(GameObject board)
    {
        this.board = board;
    }

    public GameObject GetBoard()
    {
        return board;
    }
}
