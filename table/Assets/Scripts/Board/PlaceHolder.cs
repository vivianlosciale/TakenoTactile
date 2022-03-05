using UnityEngine;

public class PlaceHolder
{
    public GameObject GameObject { get; private set; }
    public bool processed;
    public bool used;
    public Vector2Int position;

    public PlaceHolder(Vector2Int position, Transform transform)
    {
        this.position = position;
        processed = false;
        used = false;
        GameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/placeHolder"), transform);
        GameObject.name = $"({position.x},{position.y})";
        GameObject.transform.localPosition = new Vector3(1.9f * (position.y - (float)position.x / 2), -position.x * 1.6f, 0);
        GameObject.SetActive(false);
    }
}
