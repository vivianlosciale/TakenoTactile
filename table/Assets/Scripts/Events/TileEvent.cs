using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEvent : MonoBehaviour
{
    private TableClient _tableClient;
    private int _numberOfBamboos;
    private Tile _tile;
    private List<string> _objectsValues;

    void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        _numberOfBamboos = 0;
        _objectsValues = new List<string>();
    }

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }

    public void OnBambooPlaced(string tuioValue)
    {
        if (!_objectsValues.Contains(tuioValue))
        {
            if (_tableClient.IsGardener(tuioValue) && _tableClient.CanMoveFarmer(_tile))
            {
                _tableClient.SetGardenerPosition(_tile.position);
            }
            if (_tableClient.IsPanda(tuioValue) && _tableClient.CanMovePanda(_tile))
            {
                _tableClient.SetPandaPosition(_tile.position);
            }
            if (_numberOfBamboos < 4 && _tableClient.IsBamboo(tuioValue))
            {
                if (_tableClient.CanPlaceBambooFromFarmer(_tile.position))
                {
                    _objectsValues.Add(tuioValue);
                    _numberOfBamboos++;
                    _tableClient.SendBambooAction(MessageQuery.PlaceBamboo, _tile.position);
                } else if (_tableClient.CanPlaceBambooFromRainPower(_tile))
                {
                    _objectsValues.Add(tuioValue);
                    _numberOfBamboos++;
                    _tableClient.SendBambooAction(MessageQuery.WaitingChoseRain, _tile.position);
                }
            }
        }
    }

    public void OnBambooEated(string tuioValue)
    {
        if (_objectsValues.Contains(tuioValue))
        {
            if (_tableClient.CanEatBamboo(_tile.position))
            {
                _objectsValues.Remove(tuioValue);
                _numberOfBamboos--;
                _tableClient.SendBambooAction(MessageQuery.EatBamboo, _tile.position);
            }
        }
    }

    public bool CanPlaceBamboo()
    {
        return _numberOfBamboos < 4;
    }

    public bool CanEatBamboo()
    {
        return _numberOfBamboos > 0;
    }
}
