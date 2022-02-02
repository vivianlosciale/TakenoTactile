using System;
using UnityEngine;

class TileMovement : MonoBehaviour
{
    Vector3 pointPosition;
    private void FixedUpdate()
    {
        float step = 6.0f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointPosition, step);
        if(Vector3.Distance(transform.position, pointPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void SetPosition(Vector3 pointPosition)
    {
        this.pointPosition = pointPosition;
    }
}
