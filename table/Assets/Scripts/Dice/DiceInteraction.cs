using UnityEngine;

public class DiceInteraction : MonoBehaviour
{
    private float moveSpeed = 30.0f;
    private float range = 5.0f;
    private float force = 17.5f;
    float cameraZDistance;

    private void Start()
    {
        cameraZDistance = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    /// <summary>
    /// To remove the max default capacity
    /// </summary>
    private void OnMouseDown()
    {
        GetComponent<Rigidbody>().maxAngularVelocity = 15.0f;
    }

    /// <summary>
    /// Used with mouse, should be changed to detect hand
    /// </summary>
    private void OnMouseDrag()
    {
        //follow mouse cursor
        Vector3 ScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        transform.position = Camera.main.ScreenToWorldPoint(ScreenPosition);
        Debug.Log(Camera.main.ScreenToWorldPoint(ScreenPosition));
        //add rotation
        GetComponent<Rigidbody>().angularVelocity += new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        //add velocity from mouse
        GetComponent<Rigidbody>().velocity=new Vector3(Input.GetAxis("Mouse X")* force, 0.0f, Input.GetAxis("Mouse Y") * force);


    }

    private void OnMouseUp()
    {
        GetComponent<Rigidbody>().useGravity = true;
    }

    /// <summary>
    /// When the result of the dice has been detected, put the dice in the middle of the field
    /// </summary>
    private void ZoomOnDice()
    {
        if (DiceChecker.result != DiceFaces.NONE)
        {
            GetComponent<MeshCollider>().enabled = false;
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 10, 0), step);
        }
    }

    void Update()
    {
        ZoomOnDice();
    }
}
