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
        transform.parent.GetComponent<PlaceHolderBoard>().DeactivateAllSlot();
        GameObject tile = Instantiate(Resources.Load<GameObject>("Prefabs/Tiles"), GameObject.Find("TileBoard").transform);
        Material newMat = new Material(Resources.Load<Material>("Models/Material/tiles_face"));
        Texture2D text = Resources.Load<Texture2D>("Tiles/" + texture);
        newMat.mainTexture = text;
        var materials = tile.GetComponent<MeshRenderer>().materials;
        materials[1] = newMat;
        tile.GetComponent<MeshRenderer>().materials = materials;
        tile.transform.position = transform.position + new Vector3(0, 11, 0);
        tile.AddComponent<BoardTileMovement>().SetPosition(transform.position);
        PlaceHolder p = transform.parent.GetComponent<PlaceHolderBoard>().placeHolderPositions.Find(e => e.GameObject == gameObject);
        TileEvent tileEvent = tile.GetComponent<TileEvent>();
        tileEvent.SetPlaceHolder(p);
        p.used = true;
        _tableClient.SendTilePosition(tile);
    }
}
