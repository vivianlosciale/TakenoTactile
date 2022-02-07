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

}
