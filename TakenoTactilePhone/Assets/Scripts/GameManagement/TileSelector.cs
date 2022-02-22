using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
       // private bool _needed;
        public AudioSource soundManager;
        public AudioClip TouchingAudioClip;
        public AudioClip SlidingAudioClip;
        private GameObject savedChild;
        //private bool _selectedTile;

        /*public bool isNeeded()
        {
            return _needed;
        }*/
        
        /*public void ChangeNeeded()
        {
            Debug.Log("CHANGE WAS ASKED FOR TILE SELECTOR");
            _needed = !_needed;
            Debug.Log("FOR TILE SELECTOR NEW VALUE FOR IS NEEDED : " + _needed);
        }*/

        /*public void Update()
        {
            if (!_needed) return;
            DetectTileSelection();
        }*/

        /*private void DetectTileSelection()
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
        }*/

        /*private void SelectTile(string tileName)
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
                        if (tileName == child.name)
                        {
                            if (!_selectedTile)
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
                        else
                        {
                            child.SetActive(false); 
                        }
                }
        }*/

        private void SendResult()
        {
                GameObject.FindWithTag(TagManager.MobileClient.ToString())
                        .GetComponent<MobileClient>().SendChosenTile(savedChild.name);
        }
        
        public void PlaceTiles()
        {
            var coordinates = new [] { -2, 0, 2 };
            var tilesNb = transform.childCount;
            var minTiles = Math.Min(tilesNb, 3); //on peut avoir au maximum 3 tuiles affichées
            for (var i = 0; i < minTiles; i++)
            {
                PlaceTile(coordinates[i], transform.GetChild(i).gameObject);
            }
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

        public void SetChosenTile(GameObject chosenTile)
        {
            Debug.Log("CHOSEN TILE IS : " + chosenTile.name);
            soundManager.PlayOneShot(TouchingAudioClip);
            savedChild = chosenTile;
            HideOtherTiles();
            SendResult();
        }

        private void HideOtherTiles()
        {
            var childrenNumber = transform.childCount;
            for (var i = 0; i < childrenNumber; i++)
            {
                var child = transform.GetChild(i);
                if (savedChild.name != child.name)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        
        public void SlideTile()
        {
            soundManager.PlayOneShot(SlidingAudioClip);
            savedChild.GetComponent<TileSlidingAnimation>().Animate();
        }
}