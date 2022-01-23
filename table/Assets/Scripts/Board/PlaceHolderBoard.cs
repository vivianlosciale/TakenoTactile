using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderBoard : MonoBehaviour
{
    private int sizeColumn = 5;
    private int sizeLine = 10;
    [SerializeField]
    private GameObject PlaceHolder;
    // Start is called before the first frame update
    void Awake()
    {
        bool even = true;
        for (int i = 0; i < sizeColumn; i++)
        {
            if (even)
            {
                for (int j = 0; j < sizeLine; j++)
                {
                    GameObject v = Instantiate(PlaceHolder, transform);
                    v.name = (j + i * sizeLine).ToString();
                    v.transform.localPosition = new Vector3(1.9f * j, i * 1.6f, 0);
                }

            }
            else
            {
                for (int j = 0; j < sizeLine - 1; j++)
                {
                    GameObject v = Instantiate(PlaceHolder, transform);
                    v.name = (j + i * sizeLine).ToString();
                    v.transform.localPosition = new Vector3(1.9f * (j + 0.5f), i * 1.6f, 0);
                }
            }
            even = !even;
        }

    }
}
