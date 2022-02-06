using System;
using UnityEngine;

class BoardTileMovement : MonoBehaviour
{
    Vector3 pointPosition;
    private float speed = 9.5f;
    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointPosition, step);
        if(Vector3.Distance(transform.position, pointPosition) < 0.1f)
        {
            Destroy(this);
        }
    }

    public void SetPosition(Vector3 pointPosition)
    {
        this.pointPosition = pointPosition;
    }
}
