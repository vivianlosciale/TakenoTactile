using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEvent : MonoBehaviour
{
    private TableClient _tableClient;

    private void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        _tableClient.SetEndGameButton(gameObject);
        gameObject.SetActive(false);
    }

    public void EndGame()
    {
        _tableClient.GoHomeWithSave();
    }
}
