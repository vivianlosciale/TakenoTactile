using UnityEngine;

public class PawnEvent : MonoBehaviour
{
    private int maxPawn = 2;
    bool error = false;

    public void OnActionBox()
    {
        if (maxPawn == 0)
        {
            Debug.LogError("Can't put more ActionBox");
            error = true;
        }
        else
            maxPawn--;
    }

    public void LeaveActionBox()
    {
        if (!error)
            maxPawn++;
        else
            error = !error;
    }

}
