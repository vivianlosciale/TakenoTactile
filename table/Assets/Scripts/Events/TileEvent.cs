using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEvent : MonoBehaviour
{
    private TableClient _tableClient;
    private int _numberOfBamboos;
    private Tile _tile;
    private List<string> objectsValues;

    void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        _numberOfBamboos = 0;
        objectsValues = new List<string>();
    }

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }

    public void OnBambooPlaced()
    {
        if (_tableClient.CanPlaceBamboo() && _numberOfBamboos < 4)
        {
            _numberOfBamboos++;
            _tableClient.SendBambooPlaced(PositionDto.ToString(_tile.position.x, _tile.position.y));
        }
    }

    public bool CanPlaceBamboo()
    {
        return _numberOfBamboos < 4;
    }
}
