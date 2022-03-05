using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEvent : MonoBehaviour
{
    private TableClient _tableClient;
    private int _numberOfBamboos;
    private Tile _tile;
    private List<string> _objectsValues;
    private string _gardenerValue;
    private string _pandaValue;


    void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        _numberOfBamboos = 0;
        _objectsValues = new List<string>();
        _gardenerValue = "";
        _pandaValue = "";
    }

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }

    private void AddToPlaceError(string tuioValue)
    {
        if (_tableClient.IsBamboo(tuioValue))
        {
            _tableClient.AddNewError(tuioValue, "Il manque un bambou ! Rajoutez-en un !", ErrorInGameType.BambooToPlace, _tile);
        }
        else if (_tableClient.IsGardener(tuioValue))
        {
            _tableClient.AddNewError(tuioValue, "Vous ne pouvez pas enlever le jardinier maintenant ! Reposez le !", ErrorInGameType.FarmerToPlace, _tile);
        }
        else
        {
            _tableClient.AddNewError(tuioValue, "Vous ne pouvez pas enlever le panda maintenant ! Reposez le !", ErrorInGameType.PandaToPlace, _tile);
        }
    }

    private void AddToRemoveError(string tuioValue)
    {
        if (_tableClient.IsBamboo(tuioValue))
        {
            _tableClient.AddNewError(tuioValue, "Un bambou est en trop ! Enlevez-en un !", ErrorInGameType.BambooToRemove, _tile);
        }
        else if (_tableClient.IsGardener(tuioValue))
        {
            _tableClient.AddNewError(tuioValue, "Vous ne pouvez pas poser le jardinier ici ! Enlevez le !", ErrorInGameType.FarmerToRemove, _tile);
        }
        else
        {
            _tableClient.AddNewError(tuioValue, "Vous ne pouvez pas poser le panda ici ! Enlevez le !", ErrorInGameType.PandaToRemove, _tile);
        }
    }

    public void OnObjectPlacedEvent(string tuioValue)
    {
        if (_tableClient.HasErrorsInGame())
        {
            OnErrorObjectPlaced(tuioValue);
        }
        else
        {
            OnObjectPlaced(tuioValue);
        }
    }

    public void OnObjectRemoveEvent(string tuioValue)
    {
        if (_tableClient.HasErrorsInGame())
        {
            OnErrorObjectRemoved(tuioValue);
        }
        else
        {
            OnObjectRemove(tuioValue);
        }
    }

    private void OnObjectPlaced(string tuioValue)
    {
        if (_tableClient.IsGardener(tuioValue) && _tableClient.CanMoveFarmer(_tile))
        {
            _tableClient.SetGardenerPosition(_tile.position);
            _gardenerValue = tuioValue;
        }
        else if (_tableClient.IsPanda(tuioValue) && _tableClient.CanMovePanda(_tile))
        {
            _tableClient.SetPandaPosition(_tile.position);
            _pandaValue = tuioValue;
        }
        else if (_numberOfBamboos < 4 && _tableClient.CanPlaceBambooFromFarmer(_tile.position))
        {
            _objectsValues.Add(tuioValue);
            _numberOfBamboos++;
            _tableClient.SendBambooAction(MessageQuery.PlaceBamboo, _tile.position);
        }
        else if (_numberOfBamboos < 4 && _tableClient.CanPlaceBambooFromRainPower(_tile))
        {
            _objectsValues.Add(tuioValue);
            _numberOfBamboos++;
            _tableClient.SendBambooAction(MessageQuery.WaitingChoseRain, _tile.position);
        } else
        {
            AddToRemoveError(tuioValue);
        }
    }

    private void OnObjectRemove(string tuioValue)
    {
        if (_tableClient.HasErrorsInGame()) return;
        if (_tableClient.IsBamboo(tuioValue) && _tableClient.CanRemoveBamboo())
        {
            _objectsValues.Remove(tuioValue);
            _numberOfBamboos--;
            _tableClient.SendBambooAction(MessageQuery.EatBamboo, _tile.position);
        }
        else if (_tableClient.IsGardener(tuioValue) && _tableClient.CanRemoveFarmer()) return;
        else if (_tableClient.IsPanda(tuioValue) && _tableClient.CanRemovePanda()) return;
        else
        {
            Debug.Log("BAMBOO : " + _tableClient.CanRemoveBamboo());
            Debug.Log("Gardener : " + _tableClient.CanRemoveFarmer());
            Debug.Log("Panda : " + _tableClient.CanRemovePanda());
            AddToPlaceError(tuioValue);
        }
    }

    private void OnErrorObjectPlaced(string tuioValue)
    {
        if (_tableClient.HasErrorsInGame())
        {
            List<ErrorInGame> errorsInGame = new List<ErrorInGame>(_tableClient.GetErrors());
            Debug.Log("PLACED : J'ai " + errorsInGame.Count + " erreurs à traiter !");
            foreach (ErrorInGame errorInGame in errorsInGame)
            {
                Debug.Log("PLACED : " + errorInGame.message);
                Debug.Log("PLACED : tile : " + errorInGame.errorTile.Equals(_tile));
                if (!errorInGame.errorTile.Equals(_tile)) continue;
                switch (errorInGame.errorType)
                {
                    case ErrorInGameType.FarmerToPlace:
                    case ErrorInGameType.PandaToPlace:
                        if (errorInGame.tuioValue.Equals(tuioValue))
                        {
                            _tableClient.ResolveNextError(errorInGame); return;
                        }
                        break;
                    case ErrorInGameType.BambooToPlace:
                        if (_tableClient.IsBamboo(tuioValue))
                        {
                            _tableClient.ResolveNextError(errorInGame); return;
                        }
                        break;
                }
            }
            AddToRemoveError(tuioValue);
        }
    }

    private void OnErrorObjectRemoved(string tuioValue)
    {
        if (_tableClient.HasErrorsInGame())
        {
            List<ErrorInGame> errorsInGame = new List<ErrorInGame>(_tableClient.GetErrors());
            Debug.Log("REMOVE : J'ai " + errorsInGame.Count + " erreurs à traiter !");
            foreach (ErrorInGame errorInGame in errorsInGame)
            {
                Debug.Log("REMOVE : " + errorInGame.message);
                Debug.Log("REMOVE : tile : " + !errorInGame.errorTile.Equals(_tile));
                if (!errorInGame.errorTile.Equals(_tile)) continue;
                switch (errorInGame.errorType)
                {
                    case ErrorInGameType.FarmerToRemove:
                    case ErrorInGameType.PandaToRemove:
                        if (errorInGame.tuioValue.Equals(tuioValue))
                        {
                            _tableClient.ResolveNextError(errorInGame); return;
                        }
                        break;
                    case ErrorInGameType.BambooToRemove:
                        if (_tableClient.IsBamboo(tuioValue))
                        {
                            _tableClient.ResolveNextError(errorInGame); return;
                        }
                        break;
                }
            }
            AddToPlaceError(tuioValue);
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