using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public List<Tile> tilesPositions;
    public Vector2Int pandaPosition;
    public Vector2Int gardenerPosition;
    private string pandaId;
    private string gardenerId;

    private void Awake()
    {
        pandaPosition = new Vector2Int(0, 0);
        gardenerPosition = new Vector2Int(0, 0);
        tilesPositions = new List<Tile>();
        tilesPositions.Add(new Tile(new Vector2Int(0, 0), transform)); //castle tile
        TableClient tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        tableClient.SetTileBoard(this);
        gardenerId = "4";//"CE";
        pandaId = "5";// "CF";
    }

    public bool IsGardener(string id)
    {
        return gardenerId.Equals(id);
    }
    
    public bool IsPanda(string id)
    {
        return pandaId.Equals(id);
    }

    public void SetGardenerPosition(Vector2Int newPosition)
    {
        gardenerPosition = newPosition;
    }
    
    public void SetPandaPosition(Vector2Int newPosition)
    {
        pandaPosition = newPosition;
    }

    public List<Tile> TilesWhereCantPlaceBamboo()
    {
        if (tilesPositions.Count == 1) return tilesPositions;
        List<Tile> tilesNotAvailable = new List<Tile>();
        tilesNotAvailable.Add(tilesPositions[0]);
        foreach (Tile tile in tilesPositions)
        {
            TileEvent tileEvent = tile.GameObject.GetComponent<TileEvent>();
            if (!tileEvent.CanPlaceBamboo())
            {
                tilesNotAvailable.Add(tile);
            }
        }
        return tilesNotAvailable;
    }

    public List<Tile> TilesWhereCantEatBamboo()
    {
        List<Tile> tilesNotAvailable = new List<Tile>();
        foreach (Tile t in tilesPositions)
        {
            if (t.position.Equals(pandaPosition))
            {
                tilesNotAvailable.Add(t);
            }
        }
        return tilesNotAvailable;
    }

    public List<Tile> ActivatePandaNeighborsSlot()
    {
        return ActivateNeighborsSlot(pandaPosition);
    }

    public List<Tile> ActivateGardenerNeighborsSlot()
    {
        return ActivateNeighborsSlot(gardenerPosition);
    }

    private List<Tile> ActivateNeighborsSlot(Vector2Int pawn)
    {
        if (tilesPositions.Count == 1) return tilesPositions;
        List<Tile> neighbors = GetPawnActiveSlot(pawn);
        List<Tile> toDeactivate = new List<Tile>(tilesPositions);
        toDeactivate.RemoveAll(e => neighbors.Contains(e));
        foreach (Tile t in toDeactivate)
        {
            t.GameObject.AddComponent<TileMaterial>().DeactivateTile();
        }
        return toDeactivate;
    }

    private List<Tile> GetPawnActiveSlot(Vector2Int pawn)
    {
        Tile origin = tilesPositions.Find(e => e.position == pawn);
        List<Tile> foundActiveSlot = new List<Tile>();
        //foundActiveSlot.Add(origin);
        foundActiveSlot.AddRange(FindLineNeighbors(origin, 0, 1));
        foundActiveSlot.AddRange(FindLineNeighbors(origin, 0, -1));
        foundActiveSlot.AddRange(FindLineNeighbors(origin, 1, 1));
        foundActiveSlot.AddRange(FindLineNeighbors(origin, -1, -1));
        foundActiveSlot.AddRange(FindLineNeighbors(origin, 1, 0));
        foundActiveSlot.AddRange(FindLineNeighbors(origin, -1, 0));
        return foundActiveSlot;
    }

    private List<Tile> FindLineNeighbors(Tile origin, int x, int y)
    {
        List<Tile> line = new List<Tile>();
        Tile tileInProgress = origin;
        while ((tileInProgress = tilesPositions.Find(e => e.position == tileInProgress.position + new Vector2Int(x, y))) != null)
        {
            line.Add(tileInProgress);
        }
        return line;
    }

    internal void ActivateGardenerTile()
    {
        ActivateTile(gardenerPosition);
    }

    internal void ActivatePandaTile()
    {
        ActivateTile(pandaPosition);
    }

    private void ActivateTile(Vector2Int gardenerPosition)
    {
        foreach (Tile t in tilesPositions)
        {
            if (t.position != gardenerPosition)
            {
                t.GameObject.AddComponent<TileMaterial>().DeactivateTile();
            }
        }
    }

    internal void DeactivateGardenerTile()
    {
        DeactivateTile(gardenerPosition);
    }

    internal void DeactivatePandaTile()
    {
        DeactivateTile(pandaPosition);
    }

    private void DeactivateTile(Vector2Int gardenerPosition)
    {
        foreach (Tile t in tilesPositions)
        {
            if (t.position != gardenerPosition)
            {
                Destroy(t.GameObject.GetComponent<TileMaterial>());
            }
        }
    }

    internal bool IsFarmerPosition(Vector2Int position)
    {
        return position.Equals(gardenerPosition);
    }

    internal bool IsPandaPosition(Vector2Int position)
    {
        return position.Equals(pandaPosition);
    }
}
