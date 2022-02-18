using System;
using System.Collections;
using UnityEngine;

public class PawnEvent : MonoBehaviour
{
    private TableClient _tableClient;
    public Player player;
    public int position;

    public void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        player = _tableClient.GetPlayerFromPosition(position);
        if (player == null)
        {
            gameObject.SetActive(false);
        } else
        {
            player.SetBoard(gameObject);
        }
    }

    public void OnActionBox(string actionName)
    {
        Debug.Log("Action : " + ActionsMethods.ToActions(actionName));
        Actions action = ActionsMethods.ToActions(actionName);
        _tableClient.SendChoseActionToServer(action, player);
    }

    public void LeaveActionBox(string actionName)
    {
        Debug.Log("Action : " + ActionsMethods.ToActions(actionName));
        Actions action = ActionsMethods.ToActions(actionName);
        _tableClient.SendRemoveActionToServer(action, player);
    }

    public void AddCardToBoard(string cardName)
    {
        GameObject prefab = Resources.Load("Models/Cards") as GameObject;
        GameObject instance = Instantiate(prefab);
        Material newMat = new Material(Resources.Load<Material>("Models/Material/card_face"));
        Texture2D text = Resources.Load<Texture2D>("Cards/" + cardName);
        newMat.SetTexture("_EmissionMap", text);
        var materials = instance.GetComponent<MeshRenderer>().materials;
        materials[1] = newMat;
        instance.GetComponent<MeshRenderer>().materials = materials;
        instance.transform.parent = transform.GetChild(0);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.Euler(0, 0, 0);
        instance.AddComponent<CardMovement>();
    }

}
