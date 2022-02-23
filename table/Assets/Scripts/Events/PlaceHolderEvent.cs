using UnityEngine;

public class PlaceHolderEvent : MonoBehaviour
{
    private TableClient _tableClient;
    [HideInInspector]
    public string texture;

    public void Start()
    {
        _tableClient = GameObject.FindGameObjectWithTag("TableClient").GetComponent<TableClient>();
    }

    public void PlaceTileToBoard()
    {
        GetComponentInParent<PlaceHolderBoard>().DeactivateAllSlot();
        PlaceHolder p = GetComponentInParent<PlaceHolderBoard>().placeHolderPositions.Find(e => e.GameObject == gameObject);
        Tile tile = new Tile(p.position, GameObject.Find("TileBoard").transform);
        GameObject gameObjectTile = tile.GameObject;
        Material newMat = new Material(Resources.Load<Material>("Models/Material/tiles_face"));
        Texture2D text = Resources.Load<Texture2D>("Tiles/" + texture);
        newMat.mainTexture = text;
        Material[] materials = gameObjectTile.GetComponent<MeshRenderer>().materials;
        materials[1] = newMat;
        gameObjectTile.GetComponent<MeshRenderer>().materials = materials;
        gameObjectTile.transform.position = transform.position + new Vector3(0, 11, 0);
        gameObjectTile.AddComponent<BoardTileMovement>().SetPosition(transform.position);
        gameObjectTile.GetComponent<TileEvent>().SetTile(tile);
        GameObject.Find("TileBoard").GetComponent<TileBoard>().tilesPositions.Add(tile);
        p.used = true;
        _tableClient.SendTilePosition(tile);
    }
}
