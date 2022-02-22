using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEvent : MonoBehaviour
{
    private TableClient _tableClient;
    private int _numberOfBamboos;

    void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        _numberOfBamboos = 0;
    }

    public void OnBambooPlaced()
    {
        if (_tableClient.CanPlaceBamboo() && _numberOfBamboos < 4)
        {
            _numberOfBamboos++;
            //_tableClient.SendBambooPlaced(PositionDto.ToString(placeHolder.position.x, placeHolder.position.y));
        }
    }

    public bool CanPlaceBamboo()
    {
        return _numberOfBamboos < 4;
    }
}
