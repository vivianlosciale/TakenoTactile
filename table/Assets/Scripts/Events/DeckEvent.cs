using System.Collections;
using UnityEngine;

public class DeckEvent : MonoBehaviour
{

    public TableClient _tableClient;

    public void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        //tableClient.SetDeckEvent(this);
    }

    public void PickTile()
    {
        Debug.Log("Can : " + _tableClient.CanPickCard());
        if (_tableClient.CanPickTile())
        {
            Debug.Log("Player : " + _tableClient.GetCurrentPlayer().id);
            GameObject board = _tableClient.GetCurrentPlayer().GetBoard(); //GameObject.Find("BoardP" + _tableClient.GetCurrentPlayer().id);
            Transform pointPosition = board.transform.GetChild(0).transform;
            StartCoroutine(TranslateCard(pointPosition));
            _tableClient.PickTile();
        }
    }

    public void PickCard()
    {
        Debug.Log("Can : " + _tableClient.CanPickCard());
        if (_tableClient.CanPickCard())
        {
            Debug.Log("Player : " + _tableClient.GetCurrentPlayer().id);
            GameObject board = _tableClient.GetCurrentPlayer().GetBoard(); //GameObject.Find("BoardP" + _tableClient.GetCurrentPlayer().id);
            Transform pointPosition = board.transform.GetChild(0).transform;
            StartCoroutine(TranslateCard(pointPosition));
            _tableClient.PickCard();
        }
    }

    public void StartGame()
    {
        _tableClient.StartGame();
    }

    private IEnumerator TranslateCard(Transform pointPosition)
    {
        int nbOfCard = 3;
        GameObject prefab = Resources.Load("Models/Tiles") as GameObject;
        for (int i = 0; i < nbOfCard; i++)
        {
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.Euler(90, 0, 60));
            instance.AddComponent<DeckTileMovement>().SetPosition(pointPosition.position);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
