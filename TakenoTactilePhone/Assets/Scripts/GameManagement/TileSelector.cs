using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
        private bool _needed;

        public void Start()
        {
            _needed = false;
        }

        public void ChangeNeeded()
        {
        _needed = !_needed;
        }
 
        public void Update()
        {
            if (_needed)
            {
                Vector3 v = default(Vector3);
                if (Input.touchCount > 0)
                {
                    v = Input.GetTouch(0).position;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    v = Input.mousePosition;
                }
                if (v != default)
                {
                    Debug.Log("------------------------TOUCH IN HAND MANAGEMENT");
                    Ray ray = Camera.main.ScreenPointToRay(v);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log("---------------------------CLICKED ELEMENT : " + hit.transform.name);
                        SelectTile(hit.transform.name);
                    }
                }
            }
                
        }

        private void SelectTile(String tileName)
        {
                List<GameObject> childrenTiles = new List<GameObject>();
                int childrenNumber = transform.childCount;
                for (int i = 0; i < childrenNumber; i++)
                {
                        childrenTiles.Add(transform.GetChild(i).gameObject);
                }

                bool selectedTile = false;
                foreach (var child in childrenTiles)
                {
                        if (tileName == child.name && !selectedTile)
                        {
                                child.transform.localPosition = new Vector3(0, 0, 0);  
                                Debug.Log("TILE SELECTED : " + tileName);
                                selectedTile = true;
                                SendResult(tileName);
                        }
                        else
                        {
                               child.SetActive(false); 
                        }
                }
        }

        private void SendResult(string tileName)
        {
                GameObject.FindWithTag(TagManager.MobileClient.ToString())
                        .GetComponent<MobileClient>().SendChosenTile(tileName);
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
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
}