 using UnityEngine;

 public class CardSlidingAnimation : MonoBehaviour
 {

     private bool moveToCenter = false;
     private bool moveToTable = false;
     private bool returnToPosition = false;
     private Vector3 initialPosition = default;
     private bool _wasSelected = false;
     private Vector3 _centerPosition;

     private void Start()
     {
         Debug.Log("CARD " + gameObject.name + " POSITION " + transform.localPosition);
         initialPosition = transform.localPosition;
         _centerPosition = new Vector3(0, 0, -0.6f);
     }

     private void Update()
     {
         if (moveToCenter)
         {
             MoveToCenter();
         }
         if (moveToTable)
         {
             MoveToTable();
         }
         if (returnToPosition)
         {
             ReturnToInitialPosition();
         }
     }

     public void AnimateToTable()
     {
         Debug.Log("SENDING CARD TO TABLE");
         moveToTable = true;
     }

     public void AnimateBack()
     {
         Debug.Log("SENDING CARD TO ORIGINAL POSITION");
         returnToPosition = true;
         _wasSelected = false;
     }

     private void MoveToCenter()
     {
         if (transform.localPosition.Equals(_centerPosition))
         {
             Debug.Log("IS IN CENTER");
             moveToCenter = false;
         }
         else
         {
             Debug.Log("MOVING TOWARDS CENTER");
             var _step = 4 * Time.deltaTime;
             transform.localPosition = Vector3.MoveTowards(transform.localPosition, _centerPosition, _step);
         }
     }

     private void MoveToTable()
     {
         if (transform.position.z > 3)
         {
             moveToTable = false;
             Destroy(this);
             return;
         }
         transform.position += new Vector3(0, 0, 4 * Time.deltaTime);
     }

     private void ReturnToInitialPosition()
     {
         if (transform.localPosition.Equals(initialPosition))
         {
             Debug.Log("IS BACK TO INITIAL POSITION");
             returnToPosition = false;
             _wasSelected = false;
             GetComponentInParent<HandManagement>().UpdateCardsPosition();
         }
         else
         {
             var _step = 4 * Time.deltaTime;
             transform.localPosition = Vector3.MoveTowards(transform.localPosition, initialPosition, _step);
             Debug.Log("LOCALPOSITION : " + transform.localPosition + " and initial : " + initialPosition);
             Debug.Log("POSITION : " + transform.position + " and initial : " + initialPosition);
         }
     }

     private void OnMouseUpAsButton()
     {
         if (_wasSelected) return;
         GetComponentInParent<HandManagement>().SetSelectedCard(gameObject);
         moveToCenter = true;
         _wasSelected = true;
     }

     public void ResetCard(Vector3 v)
     {
         initialPosition = v;
         transform.localPosition = v;
         _wasSelected = false;
     }
 }