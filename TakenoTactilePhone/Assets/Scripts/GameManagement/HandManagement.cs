using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HandManagement : MonoBehaviour
{
        public GameObject validateButton;
        public GameObject cancelButton;
        private readonly List<Vector3> _localPositions = new List<Vector3>();
        private List<string> _childrenNames = new List<string>();
        private string _cardToSubmit = "";

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
                Vector3 v = default(Vector3);
                if (Input.touchCount > 0 )
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
                        if (Physics.Raycast(ray, out hit)) {
                                Debug.Log("---------------------------CLICKED ELEMENT : " +hit.transform.name);
                                DisplayCard(hit.transform.name);
                        }
                }
        }
        
        public void UpdateCardsPosition()
        {
                _childrenNames = new List<string>();
                int children = transform.childCount;
                validateButton.SetActive(false);
                cancelButton.SetActive(false);
                for (int i = 0; i < children; i++)
                {
                        GameObject child = transform.GetChild(i).gameObject;
                        child.SetActive(true);
                        child.transform.localPosition = _localPositions[i];
                        _childrenNames.Add(child.name);
                }
        }

        private void DisplayCard(string elementName)
        {
                if (_childrenNames.Contains(elementName))
                {
                        //on récupère le corresponding child
                        GameObject child = GetChildByName(elementName);
                        _cardToSubmit = elementName;
                        //on sauvegarde sa position précédente
                        var previousPosition = child.transform.position;
                        //on le place au centre
                        child.transform.position = new Vector3(0, previousPosition.y, 0);
                        //on fait apparaître les boutons de validation et d'annulation
                        validateButton.SetActive(true);
                        cancelButton.SetActive(true);
                        // les autres cartes deviennent invisibles
                        int children = transform.childCount;
                        for (int i = 0; i < children; i++)
                        {
                                GameObject sibling = transform.GetChild(i).gameObject;
                                if (sibling.name != elementName)
                                {
                                        sibling.SetActive(false);
                                }
                        }
                }
        }

        private GameObject GetChildByName(string cardName)
        {
                int children = transform.childCount;
                for (int i = 0; i < children; i++)
                {
                        GameObject child = transform.GetChild(i).gameObject;
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
                int children = transform.childCount;
                for (int i = 0; i < children; i++)
                {
                        GameObject child = transform.GetChild(i).gameObject;
                        if (child.name == objectiveName)
                        {
                                Destroy(child);
                        }
                }
                UpdateCardsPosition();
        }
}