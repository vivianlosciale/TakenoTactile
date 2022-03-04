using System;
using UnityEngine;

class BoardTileMovement : MonoBehaviour
{
    Vector3 pointPosition;
    private readonly float speed = 9.5f;

    private void Awake()
    {
        gameObject.GetComponent<MeshCollider>().enabled = false;
    }
    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointPosition, step);
        if(Vector3.Distance(transform.position, pointPosition) < 0.1f)
        {
            gameObject.GetComponent<MeshCollider>().enabled = true;
            Destroy(this);
        }
    }

    public void SetPosition(Vector3 pointPosition)
    {
        this.pointPosition = pointPosition;
    }
}
