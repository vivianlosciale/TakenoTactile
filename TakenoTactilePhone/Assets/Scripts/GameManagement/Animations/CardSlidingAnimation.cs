 using System;
 using UnityEngine;

 public class CardSlidingAnimation : MonoBehaviour
 {

     private bool moveToCenter = false;
     private bool moveToTable = false;
     private bool returnToPosition = false;
     private Vector3 initialPosition = default;

     private Vector3 _centerPosition;

     private void Start()
     {
         initialPosition = transform.position;
         _centerPosition = new Vector3(0, initialPosition.y, 0);
     }

     public void SetCardPosition(Vector3 v)
     {
         initialPosition = v;
         _centerPosition = new Vector3(0, initialPosition.y, 0);
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

     public void AnimateToCenter()
     {
         moveToCenter = true;
     }

     public void AnimateToTable()
     {
         moveToTable = true;
     }

     public void AnimateBack()
     {
         returnToPosition = true;
     }

     private void MoveToCenter()
     {
         if (transform.position == _centerPosition)
         {
             moveToCenter = false;
             //GetComponentInParent<HandManagement>().DisplaySingleCardUI(gameObject.name);
         }
         else
         {
             var _step = 4 * Time.deltaTime;
             transform.position = Vector3.MoveTowards(transform.position, _centerPosition, _step);
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
         if (transform.position == initialPosition)
         {
             returnToPosition = false;
             GetComponentInParent<HandManagement>().UpdateCardsPosition();
         }
         else
         {
             var _step = 4 * Time.deltaTime;
             transform.position = Vector3.MoveTowards(transform.position, initialPosition, _step);
         }
     }
 }