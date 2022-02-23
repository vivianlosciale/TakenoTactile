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
                _tableClient.SendBambooPlaced(MessageQuery.WaitingChoseRain, PositionDto.ToString(_tile.position.x, _tile.position.y));
            }
            if (_tableClient.CanPlaceBambooFromFarmer() && _numberOfBamboos < 4)
            {
                Debug.Log("Je peux bouger le fermier");
                if (_tableClient.IsGardener(tuioValue))
                {
                    Debug.Log("Je suis le fermier");
                    _tableClient.SetGardenerPosition(_tile.position);
                } else
                {
                    _objectsValues.Add(tuioValue);
                    _numberOfBamboos++;
                    _tableClient.SendBambooPlaced(MessageQuery.WaitingMoveFarmer, PositionDto.ToString(_tile.position.x, _tile.position.y));
                }
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
