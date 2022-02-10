using UnityEngine;

class CardMovement : MonoBehaviour
{
    private readonly float speed = 9.5f;

    private Vector3 pos = new Vector3(0, 13, 0);
    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pos, step);
        if (Vector3.Distance(transform.position, pos) < 0.1f)
        {
            Destroy(this);
        }
    }
}