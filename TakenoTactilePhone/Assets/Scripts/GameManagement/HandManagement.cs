using System.Collections.Generic;
using UnityEngine;

public class HandManagement : MonoBehaviour
{
        public AudioSource soundManager;
        public AudioClip cardSubmittedSound;
        public AudioClip cardFlip;
        public GameObject validateButton;
        public GameObject cancelButton;
        
        private readonly List<Vector3> _localPositions = new List<Vector3>();
        private GameObject _selectedCard;

        public void Start()
        {
                _localPositions.Add(new Vector3(-2,0,0));
                _localPositions.Add(new Vector3(-1,0,0));
                _localPositions.Add(new Vector3(0,0,0));
                _localPositions.Add(new Vector3(1,0,0));
                _localPositions.Add(new Vector3(2,0,0));
        }

        public void UpdateCardsPosition()
        {
                var children = transform.childCount;
                validateButton.SetActive(false);
                cancelButton.SetActive(false);
                for (var i = 0; i < children; i++)
                {
                        var child = transform.GetChild(i).gameObject;
                        child.SetActive(true);
                        child.GetComponent<CardSlidingAnimation>().SetCardPosition(_localPositions[i]);
                }
        }

        public void CancelSelectedCard()
        {
                soundManager.PlayOneShot(cardFlip);
                validateButton.SetActive(false);
                cancelButton.SetActive(false);
                _selectedCard.GetComponent<CardSlidingAnimation>().AnimateBack();
        }

        public void SetSelectedCard(GameObject card)
        {
                soundManager.PlayOneShot(cardFlip);
                _selectedCard = card;
                DisplaySingleCardUI();
        }

        private void DisplaySingleCardUI()
        {
                validateButton.SetActive(true);
                cancelButton.SetActive(true);
                var children = transform.childCount;
                for (var i = 0; i < children; i++)
                {
                        GameObject sibling = transform.GetChild(i).gameObject;
                        if (sibling.name != _selectedCard.name)
                        {
                                sibling.SetActive(false);
                        }
                } 
        }

        public void SendResult()
        {
                Debug.Log("SENDING RESULT : " + _selectedCard.name);
                GameObject.FindWithTag(TagManager.MobileClient.ToString())
                        .GetComponent<MobileClient>().SendChosenObjective(_selectedCard.name);
        }

        public void ObjectiveWasValidated(string objectiveName)
        {
                Debug.Log("OBJECTIVE WAS VALIDATED : " + objectiveName);
                _selectedCard.GetComponent<CardSlidingAnimation>().AnimateToTable();
                soundManager.PlayOneShot(cardSubmittedSound);
                UpdateCardsPosition();
        }

        public void HideHand()
        {
                UpdateCardsPosition();
                cancelButton.SetActive(false);
                validateButton.SetActive(false);
        }
}