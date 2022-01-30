using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderBoard : MonoBehaviour
{
    private int sizeColumn = 5;
    private int sizeLine = 10;
    List<PlaceHolder> placeHolderPositions;
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
        activateNeighborsSlot(getActiveSlot());
    }

    private void GeneratePlaceHolder(int i, int j)
    {
        PlaceHolder p = new PlaceHolder(new Vector2Int(i, j), transform);
        placeHolderPositions.Add(p);
    }

    private void resetProcessed()
    {
        foreach (PlaceHolder p in placeHolderPositions)
            p.processed = false;
    }

    private List<PlaceHolder> getActiveSlot()
    {
        resetProcessed();
        Queue<PlaceHolder> toProcess = new Queue<PlaceHolder>();
        PlaceHolder origin = placeHolderPositions.Find(e => e.position == new Vector2Int(0, 0));
        toProcess.Enqueue(origin);
        origin.processed = true;
        List<PlaceHolder> foundNeighbors = new List<PlaceHolder>();
        while (toProcess.Count > 0)
        {
            PlaceHolder placeHolder = toProcess.Dequeue();
            List<PlaceHolder> neighbors = getNeighbors(placeHolder);
            for (var i = 0; i < neighbors.Count; i++)
            {
                if (!neighbors[i].processed)
                {
                    if (!neighbors[i].used)
                        foundNeighbors.Add(neighbors[i]);
                    else
                        toProcess.Enqueue(neighbors[i]);
                    neighbors[i].processed = true;
                }
            }
        }
        return foundNeighbors;
    }

    private void activateNeighborsSlot(List<PlaceHolder> neighbors)
    {
        foreach (PlaceHolder p in neighbors)
            p.placeHolder.SetActive(true);
    }

    private List<PlaceHolder> getNeighbors(PlaceHolder placeHolder)
    {
        List<PlaceHolder> neighbors = new List<PlaceHolder>();
        Vector2Int position = placeHolder.position;
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x + 1, position.y + 1)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x - 1, position.y)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x - 1, position.y - 1)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x, position.y + 1)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x, position.y - 1)));
        neighbors.Add(placeHolderPositions.Find(e => e.position == new Vector2Int(position.x + 1, position.y)));
        Debug.Log(neighbors.Count);
        return neighbors;
    }
}