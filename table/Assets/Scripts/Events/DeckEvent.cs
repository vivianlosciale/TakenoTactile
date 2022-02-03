using System.Collections;
using UnityEngine;

public class DeckEvent : MonoBehaviour
{

    public TableClient tableClient;

    // Start is called before the first frame update
    public void test()
    {
        Debug.Log("This is a onEnter event");
    }

    public void Start()
    {
        tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        //tableClient.SetDeckEvent(this);
    }

    public void PickCard()
    {
        if (tableClient.CanPickCard())
        {
            Debug.Log("Player : " + tableClient.GetCurrentPlayer());
            GameObject board = GameObject.Find("BoardP1");
            Transform pointPosition = board.transform.GetChild(0).transform;
            StartCoroutine(TranslateCard(pointPosition));
            tableClient.PickCard();
        }
    }

    public void StartGame()
    {
        tableClient.StartGame();
    }

    private IEnumerator TranslateCard(Transform pointPosition)
    {
        int nbOfCard = 3;
        GameObject prefab = Resources.Load("Models/Tiles") as GameObject;
        for (int i = 0; i < nbOfCard; i++)
        {
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.Euler(90, 0, 60));
            instance.AddComponent<TileMovement>().SetPosition(pointPosition.position);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
