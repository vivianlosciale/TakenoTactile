using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderBoard : MonoBehaviour
{
    private int sizeColumn = 5;
    private int sizeLine = 10;
    [SerializeField]
    private GameObject PlaceHolder;
    List<Vector2> tilesPositions;
    void Awake()
    {
        tilesPositions = new List<Vector2>();
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
        tilesPositions.Add(new Vector2(0, 0));

    }

    private void GeneratePlaceHolder(int i, int j)
    {
        GameObject v = Instantiate(PlaceHolder, transform);
        v.name = $"({i},{j})";
        v.transform.localPosition = new Vector3(1.9f * (j - (float)i / 2), -i * 1.6f, 0);
    }
}