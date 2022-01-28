using System;
using System.Collections;
using UnityEngine;

public class TestEvent : MonoBehaviour
{

    // Start is called before the first frame update
    public void test()
    {
        Debug.Log("This is a onclick donw");
    }
    public void pickCard(GameObject board)
    {
        Transform pointPosition = board.transform.GetChild(0).transform;
        StartCoroutine(TranslateCard(pointPosition));
    }

    private IEnumerator TranslateCard(Transform pointPosition)
    {
        int nbOfCard = 3;
        GameObject prefab = Resources.Load("Models/Tiles") as GameObject;
        for (int i = 0; i < nbOfCard; i++)
        {
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.Euler(90, 0, 60));
            instance.AddComponent<TileMovement>().setPosition(pointPosition.position);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
