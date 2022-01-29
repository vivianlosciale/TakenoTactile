using UnityEngine;

public class PawnEvent : MonoBehaviour
{
    private int maxPawn = 2;

    public void OnActionBox()
    {
        if(maxPawn == 0)
        {
            Debug.LogError("Can't place more than 2 pawns");
        }
        else
        {
            maxPawn--;
        }
    }

    public void LeaveActionBox()
    {
        maxPawn++;
    }
    
}
