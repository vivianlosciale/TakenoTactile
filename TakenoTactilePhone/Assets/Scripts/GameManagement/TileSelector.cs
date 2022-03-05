using System;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
        public AudioSource soundManager;
        public AudioClip TouchingAudioClip;
        public AudioClip SlidingAudioClip;
        private GameObject savedChild;

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