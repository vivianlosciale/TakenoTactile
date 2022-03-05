using UnityEngine;

public class Tile
{
    public GameObject GameObject { get; private set; }
    public Vector2Int position;

    public Tile(Vector2Int position, Transform transform)
    {
        this.position = position;
        GameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Tiles"), transform);
        GameObject.GetComponent<TileEvent>().SetTile(this);
        GameObject.name = $"({position.x},{position.y})";
        GameObject.transform.localPosition = new Vector3(1.9f * (position.y - (float)position.x / 2), 0, position.x * 1.6f);
    }
}
