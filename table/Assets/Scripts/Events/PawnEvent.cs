using UnityEngine;

public class PawnEvent : MonoBehaviour
{
    private int maxPawn = 2;

    public void OnActionBox()
    {
        Debug.Log("onenter");
    }

    public void LeaveActionBox()
    {
        Debug.Log("onexit");
    }
    
}
