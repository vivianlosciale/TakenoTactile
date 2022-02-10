using UnityEngine;

public class PawnEvent : MonoBehaviour
{
    private int maxPawn = 2;
    bool error = false;
    private TableClient _tableClient;
    public Player player;
    public int position;

    public void Start()
    {
        /*_tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
        player = _tableClient.GetPlayerFromPosition(position);
        if (player == null)
        {
            gameObject.SetActive(false);
        } else
        {
            player.SetBoard(gameObject);
        }*/
        AddCardToBoard("card_bamboo_1gf");
    }

    public void OnActionBox()
    {
        if (maxPawn == 0)
        {
            Debug.LogError("Can't put more ActionBox");
            error = true;
        }
        else
        {
            maxPawn--;
        }
    }

    public void OnActionBoxTest(string actionName)
    {
        if (maxPawn == 0)
        {
            Debug.LogError("Can't put more ActionBox");
            error = true;
        }
        else
        {
            Debug.Log("Action : " + ActionsMethods.ToActions(actionName));
            Actions action = ActionsMethods.ToActions(actionName);
            _tableClient.SendChoseActionToServer(action, player);
            maxPawn--;
        }
    }

    public void LeaveActionBox()
    {
        if (!error)
            maxPawn++;
        else
            error = !error;
    }

    public void LeaveActionBoxTest(string actionName)
    {
        if (!error)
        {
            Debug.Log("Action : " + ActionsMethods.ToActions(actionName));
            Actions action = ActionsMethods.ToActions(actionName);
            _tableClient.SendRemoveActionToServer(action, player);
            maxPawn++;
        }
        else
            error = !error;
    }

    public void AddCardToBoard(string cardName)
    {
        GameObject prefab = Resources.Load("Models/Cards") as GameObject;
        GameObject instance = Instantiate(prefab);
        Material newMat = new Material(Resources.Load<Material>("Models/Material/card_face"));
        Texture2D text = Resources.Load<Texture2D>("Cards/" + cardName);
        newMat.mainTexture = text;
        var materials = instance.GetComponent<MeshRenderer>().materials;
        materials[1] = newMat;
        instance.GetComponent<MeshRenderer>().materials = materials;
        instance.transform.parent = transform.GetChild(0);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.Euler(0, 0, 0);
        instance.AddComponent<CardMovement>();
    }

}
