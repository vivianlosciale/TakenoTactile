using UnityEngine;

public class PlaceHolderEvent : MonoBehaviour
{
    public void PlaceTileToBoard()
    {
        transform.parent.GetComponent<PlaceHolderBoard>().DeactivateAllSlot();
        GameObject tile = Instantiate(Resources.Load<GameObject>("Prefabs/Tiles"), GameObject.Find("TileBoard").transform);
        tile.transform.position = transform.position + new Vector3(0, 11, 0);
        tile.AddComponent<BoardTileMovement>().SetPosition(transform.position);
        PlaceHolder p = transform.parent.GetComponent<PlaceHolderBoard>().placeHolderPositions.Find(e => e.GameObject == gameObject);
        p.used = true;
    }
}
