using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderBoard : MonoBehaviour
{
    private readonly int sizeColumn = 5;
    private readonly int sizeLine = 10;
    public List<PlaceHolder> placeHolderPositions;
    public TableClient _tableClient;

    public void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        _tableClient.SetPlaceHolderBoard(this);
    }

    void Awake()
    {
        placeHolderPositions = new List<PlaceHolder>();
        bool even = true;
        for (int i = -sizeColumn / 2; i < sizeColumn / 2 + 1; i++)
        {
            if (even)
            {
                for (int j = -sizeLine / 2 + 1 + (i / 2); j < sizeLine / 2 + (i / 2) + 1; j++)
                    GeneratePlaceHolder(i, j);
            }
            else
            {
                for (int j = -sizeLine / 2 + 1 + ((i + 1) / 2); j < sizeLine / 2 + ((i + 1) / 2); j++)
                    GeneratePlaceHolder(i, j);
            }
            even = !even;
        }
        placeHolderPositions.Find(e => e.position == new Vector2Int(0, 0)).used = true;
    }

    private void GeneratePlaceHolder(int i, int j)
    {
        PlaceHolder p = new PlaceHolder(new Vector2Int(i, j), transform);
        placeHolderPositions.Add(p);
    }

    private void ResetProcessed()
    {
        foreach (PlaceHolder p in placeHolderPositions)
            p.processed = false;
    }

    private List<PlaceHolder> GetActiveSlot()
    {
        ResetProcessed();
        Queue<PlaceHolder> toProcess = new Queue<PlaceHolder>();
        PlaceHolder origin = placeHolderPositions.Find(e => e.position == new Vector2Int(0, 0));
        toProcess.Enqueue(origin);
        origin.processed = true;
        List<PlaceHolder> foundNeighbors = new List<PlaceHolder>();
        while (toProcess.Count > 0)
        {
            PlaceHolder placeHolder = toProcess.Dequeue();
            List<PlaceHolder> neighbors = GetNeighbors(placeHolder);
            for (var i = 0; i < neighbors.Count; i++)
            {
                if (!neighbors[i].processed)
                {
                    List<PlaceHolder> tmpNeighbors = GetNeighbors(neighbors[i]);
                    if (!neighbors[i].used && IsValid(tmpNeighbors))
                        foundNeighbors.Add(neighbors[i]);
                    else
                        toProcess.Enqueue(neighbors[i]);
                    neighbors[i].processed = true;
                }
            }
        }
        return foundNeighbors;
    }

    private bool IsValid(List<PlaceHolder> tmpNeighbors)
    {
        int neighborsPosed = 0;
        foreach (PlaceHolder p in tmpNeighbors)
        {
            if (p.position == new Vector2Int(0, 0))
                return true;
            if (p.used)
                neighborsPosed++;
        }
        return neighborsPosed >= 2;
    }

    public void DeactivateAllSlot()
    {
        foreach (PlaceHolder p in placeHolderPositions)
            p.GameObject.SetActive(false);
    }

    public void ActivateNeighborsSlot(string texture)
    {
        List<PlaceHolder> neighbors = GetActiveSlot();
        foreach (PlaceHolder p in neighbors)
        {
            p.GameObject.SetActive(true);
            p.GameObject.GetComponent<PlaceHolderEvent>().texture = texture;
        }

    }

    private List<PlaceHolder> GetNeighbors(PlaceHolder placeHolder)
    {
        List<PlaceHolder> neighbors = new List<PlaceHolder>();
        Vector2Int position = placeHolder.position;
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x + 1, position.y + 1)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x - 1, position.y)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x - 1, position.y - 1)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x, position.y + 1)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x, position.y - 1)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x + 1, position.y)));
        neighbors.RemoveAll(e => e == null);
        return neighbors;
    }
}