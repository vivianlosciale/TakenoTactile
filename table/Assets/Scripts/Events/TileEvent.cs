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
        Debug.Log("Coucou je suis ici");
        if (!_objectsValues.Contains(tuioValue))
        {
            Debug.Log("I'm here");
            if (_tableClient.CanPlaceBambooFromRainPower() && _numberOfBamboos < 4)
            {
                _objectsValues.Add(tuioValue);
                _numberOfBamboos++;
                _tableClient.SendBambooPlaced(MessageQuery.WaitingChoseRain, _tile.position);
            }
            if (_tableClient.IsGardener(tuioValue) && _tableClient.CanMoveFarmer(_tile.position))
            {
                Debug.Log("Je suis le fermier");
                _tableClient.SetGardenerPosition(_tile.position);
            }
            else if (_tableClient.CanPlaceBambooFromFarmer(_tile.position) && _numberOfBamboos < 4 && _tableClient.IsBamboo(tuioValue))
            {
                _objectsValues.Add(tuioValue);
                _numberOfBamboos++;
                _tableClient.SendBambooPlaced(MessageQuery.PlaceBamboo, _tile.position);
            }
        }
    }

    public void OnBambooEated(string tuioValue)
    {
        if (_objectsValues.Contains(tuioValue))
        {
            if (_tableClient.CanEatBamboo())
            {
                _objectsValues.Remove(tuioValue);
                _numberOfBamboos--;
                //_tableClient.SendBambooEated(PositionDto.ToString(_tile.position.x, _tile.position.y));
            }
        }
    }

    public bool CanPlaceBamboo()
    {
        return _numberOfBamboos < 4;
    }
}
