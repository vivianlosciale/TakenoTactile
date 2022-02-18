using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
        private bool _needed;
        public AudioSource soundManager;
        public AudioClip TouchingAudioClip;
        public AudioClip SlidingAudioClip;
        private GameObject savedChild;
        private bool _selectedTile = false;

        public void Start()
        {
            _needed = false;
        }

        public bool isNeeded()
        {
            return _needed;
        }
        
        public void ChangeNeeded()
        {
        _needed = !_needed;
        }
 
        public void Update()
        {
            if (!_needed) return;
            DetectTileSelection();
        }

        private void DetectTileSelection()
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

            if (v == default) return;
            Ray ray = Camera.main.ScreenPointToRay(v);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return;
            if (_selectedTile) return;
            SelectTile(hit.transform.name);
            
        }

        private void SelectTile(String tileName)
        {
                List<GameObject> childrenTiles = new List<GameObject>();
                var childrenNumber = transform.childCount;
                for (var i = 0; i < childrenNumber; i++)
                {
                        childrenTiles.Add(transform.GetChild(i).gameObject);
                }

                _selectedTile = false;
                foreach (var child in childrenTiles)
                {
                        if (tileName == child.name && !_selectedTile)
                        {
                            savedChild = child;
                            soundManager.PlayOneShot(TouchingAudioClip);
                            child.transform.localPosition = new Vector3(0, 0, 0);  
                            Debug.Log("TILE SELECTED : " + tileName);
                            _selectedTile = true; 
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

        public void SlideTile()
        {
            soundManager.PlayOneShot(SlidingAudioClip);
            savedChild.GetComponent<TileSlidingAnimation>().Animate();
        }
}