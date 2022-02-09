using System.Collections;
using UnityEngine;

public class DeckEvent : MonoBehaviour
{

    private TableClient _tableClient;

    public void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        //tableClient.SetDeckEvent(this);
    }

    public void PickTile()
    {
        Debug.Log("Can : " + _tableClient.CanPickTile());
        if (_tableClient.CanPickTile())
        {
            Debug.Log("Player : " + _tableClient.GetCurrentPlayer().id);
            GameObject board = _tableClient.GetCurrentPlayer().GetBoard(); //GameObject.Find("BoardP" + _tableClient.GetCurrentPlayer().id);
            Transform pointPosition = board.transform.GetChild(0).transform;
            StartCoroutine(TranslateTile(pointPosition));
            _tableClient.PickTile();
        }
    }

    public void PickCard(string cardType)
    {
        Debug.Log("Can : " + _tableClient.CanPickCard());
        if (_tableClient.CanPickCard())
        {
            Debug.Log("Player : " + _tableClient.GetCurrentPlayer().id);
            GameObject board = _tableClient.GetCurrentPlayer().GetBoard(); //GameObject.Find("BoardP" + _tableClient.GetCurrentPlayer().id);
            Transform pointPosition = board.transform.GetChild(0).transform;
            Material mat = Resources.Load<Material>("Models/Material/card_back_" + cardType);
            TranslateCard(pointPosition, mat);
            _tableClient.PickCard(cardType);
        }
    }

    private void TranslateCard(Transform pointPosition, Material mat)
    {
        GameObject prefab = Resources.Load("Models/Cards") as GameObject;
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.Euler(90, 0, -90));
        instance.GetComponent<MeshRenderer>().material = mat;
        instance.AddComponent<DeckTileMovement>().SetPosition(pointPosition.position);
    }

    public void StartGame()
    {
        _tableClient.StartGame();
    }

    private IEnumerator TranslateTile(Transform pointPosition)
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
