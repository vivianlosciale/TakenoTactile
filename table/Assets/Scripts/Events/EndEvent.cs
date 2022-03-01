using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEvent : MonoBehaviour
{
    private TableClient _tableClient;

    private void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
    }

    public void EndGame()
    {
        _tableClient.GoHomeWithSave();
    }
}
