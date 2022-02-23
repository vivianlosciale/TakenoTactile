using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnEvent : MonoBehaviour
{
    private TableClient _tableClient;
    public Player player;
    public int position;
    private List<Actions> _actions;

    public void Start()
    {
        //StartCoroutine(example());
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        player = _tableClient.GetPlayerFromPosition(position);
        if (player == null)
        {
            gameObject.SetActive(false);
        } else
        {
            player.SetBoard(gameObject);
        }
        _actions = new List<Actions>();
    }


    internal IEnumerator ShowWeatherImage(string weather)
    {
        yield return null;
        transform.GetChild(7).GetComponent<WeatherMaterial>().showWeatherImage(weather);
    }

    internal IEnumerator RemoveWeatherImage()
    {
        yield return null;
        transform.GetChild(7).GetComponent<WeatherMaterial>().removeWeatherImage();
    }

    internal IEnumerator AddIcon(string iconName)
    {
        yield return null;
        transform.GetChild(6).GetComponent<ListAction>().AddIcon(iconName);
    }

    internal IEnumerator RemoveIcon(string iconName)
    {
        yield return null;
        transform.GetChild(6).GetComponent<ListAction>().RemoveIcon(iconName);
    }

    internal IEnumerator RemoveAllIcon()
    {
        yield return null;
        transform.GetChild(6).GetComponent<ListAction>().removeAllIcon();
        _actions = new List<Actions>();
    }

    internal IEnumerator UseAction()
    {
        yield return null;
        transform.GetChild(6).GetComponent<ListAction>().useAction();
    }

    public void OnActionBox(string actionName)
    {
        Debug.Log("Action : " + ActionsMethods.ToActions(actionName));
        Actions action = ActionsMethods.ToActions(actionName);
        _tableClient.SendChoseActionToServer(action, player);
        _actions.Add(action);
    }

    public void LeaveActionBox(string actionName)
    {
        int numberOfActions = _tableClient.GetActualDice().Equals(DiceFaces.Sun) ? 3 : 2;
        Debug.Log("Action : " + ActionsMethods.ToActions(actionName));
        Actions action = ActionsMethods.ToActions(actionName);
        _actions.Remove(action);
        if ((_actions.Count <= numberOfActions && !_actions.Contains(action)) || (_actions.Count < numberOfActions && _tableClient.GetActualDice().Equals(DiceFaces.Wind)))
        {
            _tableClient.SendRemoveActionToServer(action, player);
        }
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
