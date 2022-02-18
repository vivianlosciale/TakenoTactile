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
        private List<string> _childrenNames = new List<string>();
        private string _cardToSubmit = "";
        private bool _cardWasSelected = false;
        private GameObject _selectedCard;

        public void Start()
        {
                _localPositions.Add(new Vector3(-2,0,0));
                _localPositions.Add(new Vector3(-1,0,0));
                _localPositions.Add(new Vector3(0,0,0));
                _localPositions.Add(new Vector3(1,0,0));
                _localPositions.Add(new Vector3(2,0,0));
                _cardToSubmit = "";
        }

        public void Update()
        {
                DetectTouch();
        }

        private void DetectTouch()
        {
                Vector3 v = default(Vector3);
                if (Input.touchCount > 0 )
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
                if (Physics.Raycast(ray, out hit))
                {
                        if (_cardWasSelected) return;
                        DisplayCard(hit.transform.name);
                }  
        }
        
        public void UpdateCardsPosition()
        {
                _childrenNames = new List<string>();
                var children = transform.childCount;
                validateButton.SetActive(false);
                cancelButton.SetActive(false);
                for (var i = 0; i < children; i++)
                {
                        var child = transform.GetChild(i).gameObject;
                        child.SetActive(true);
                        child.GetComponent<CardSlidingAnimation>().SetCardPosition(_localPositions[i]);
                        child.transform.localPosition = _localPositions[i];
                        _childrenNames.Add(child.name);
                }
        }

        public void CancelSelectedCard()
        {
                soundManager.PlayOneShot(cardFlip);
                _selectedCard.GetComponent<CardSlidingAnimation>().AnimateBack();
                _cardWasSelected = false; 
        }
        
        private void DisplayCard(string elementName)
        {
                if (!_childrenNames.Contains(elementName))
                {
                        if (!_cardWasSelected) return;
                        _selectedCard.GetComponent<CardSlidingAnimation>().AnimateBack();
                        _cardWasSelected = false;
                }
                else
                {
                        _selectedCard = GetChildByName(elementName);
                        _cardWasSelected = true;
                        _cardToSubmit = elementName;
                        soundManager.PlayOneShot(cardFlip);
                        CardSlidingAnimation animation = _selectedCard.GetComponent<CardSlidingAnimation>();
                        animation.SetCardPosition(_selectedCard.transform.position);
                        DisplaySingleCardUI(elementName);
                        animation.AnimateToCenter(); 
                }
        }

        private void DisplaySingleCardUI(string elementName)
        {
                validateButton.SetActive(true);
                cancelButton.SetActive(true);
                var children = transform.childCount;
                for (var i = 0; i < children; i++)
                {
                        GameObject sibling = transform.GetChild(i).gameObject;
                        if (sibling.name != elementName)
                        {
                                sibling.SetActive(false);
                        }
                } 
        }

        private GameObject GetChildByName(string cardName)
        {
                var children = transform.childCount;
                for (var i = 0; i < children; i++)
                {
                        var child = transform.GetChild(i).gameObject;
                        if (child.name == cardName)
                        {
                                return child;
                        }
                }
                return null;
        }

        public void SendResult()
        {
                Debug.Log("SENDING RESULT : " + _cardToSubmit);
                GameObject.FindWithTag(TagManager.MobileClient.ToString())
                        .GetComponent<MobileClient>().SendChosenObjective(_cardToSubmit);
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