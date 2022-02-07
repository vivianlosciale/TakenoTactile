using System;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
        
        public void Update()
        {
                if (Input.touchCount > 0)
                {
                        Touch touch = Input.GetTouch(0);
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);  
                        RaycastHit hit;  
                        if (Physics.Raycast(ray, out hit)) {
                                SelectTile(hit.transform.name);
                        }
                        
                }
                
                else if (Input.GetMouseButtonDown(0)) {  
                        Debug.Log("MOUSE CLICKED");
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit)) {  
                                Debug.Log("CLICKED ELEMENT : " +hit.transform.name);
                                SelectTile(hit.transform.name);
                        }  
                }  
        }

        private void SelectTile(String name)
        {
                GameObject tile0 = transform.GetChild(0).gameObject;
                GameObject tile1 = transform.GetChild(1).gameObject;
                GameObject tile2 = transform.GetChild(2).gameObject;
                if(name == tile0.name) {
                        Destroy(tile1);
                        Destroy(tile2);
                        tile0.transform.localPosition = new Vector3(0, 0, 0);
                        Debug.Log("TILE 0 SELECTED");
                        SendResult(name);
                }
                else if(name == tile1.name) {
                        Destroy(tile0);
                        Destroy(tile2);
                        tile1.transform.localPosition = new Vector3(0, 0, 0);
                        Debug.Log("TILE 1 SELECTED");
                        SendResult(name);
                }
                else if(name == tile2.name) {
                        Destroy(tile1);
                        Destroy(tile0);
                        tile2.transform.localPosition = new Vector3(0, 0, 0);
                        Debug.Log("TILE 2 SELECTED");
                        SendResult(name);
                }
        }

        private void SendResult(string name)
        {
                GameObject.FindWithTag(TagManager.MobileClient.ToString())
                        .GetComponent<MobileClient>().SendChosenTile(name);
        }
        public void PlaceTiles()
        {
                PlaceTile(-2,transform.GetChild(0).gameObject);
                PlaceTile(0,transform.GetChild(1).gameObject);
                PlaceTile(2,transform.GetChild(2).gameObject);
        }
        private void PlaceTile(int x, GameObject tile)
        {
                tile.transform.localPosition = new Vector3(x, 0, 0);
        }

        public void DestroyChildren()
        {
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                        Destroy(transform.GetChild(i));
                }
        }
        
}