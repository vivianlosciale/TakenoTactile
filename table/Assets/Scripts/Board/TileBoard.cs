using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public List<Tile> tilesPositions;
    public Vector2Int pandaPosition;
    public Vector2Int gardenerPosition;

    private void Awake()
    {
        pandaPosition = new Vector2Int(0, 0);
        gardenerPosition = new Vector2Int(0, 0);
        tilesPositions = new List<Tile>();
        tilesPositions.Add(new Tile(new Vector2Int(0, 0), transform)); //castle tile

    }

    public void ActivatePandaNeighborsSlot()
    {
        ActivateNeighborsSlot(pandaPosition);
    }

    public void ActivateGardenerNeighborsSlot()
    {
        ActivateNeighborsSlot(gardenerPosition);
    }

    private void ActivateNeighborsSlot(Vector2Int pawn)
    {
        List<Tile> neighbors = GetPawnActiveSlot(pawn);
        List<Tile> toDeactivate = new List<Tile>(tilesPositions);
        toDeactivate.RemoveAll(e => neighbors.Contains(e));
        foreach (Tile t in toDeactivate)
        {
            Debug.Log(t.position);
            t.GameObject.AddComponent<TileMaterial>();
        }
    }

    private List<Tile> GetPawnActiveSlot(Vector2Int pawn)
    {
        Tile origin = tilesPositions.Find(e => e.position == pawn);
        List<Tile> foundActiveSlot = new List<Tile>();
        foundActiveSlot.Add(origin);
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
}
