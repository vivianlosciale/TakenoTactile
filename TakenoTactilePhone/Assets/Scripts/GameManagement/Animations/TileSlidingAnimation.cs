 using UnityEngine;

 public class TileSlidingAnimation : MonoBehaviour
 {
     
     private bool _animate = false;
     private bool _moveToCenter = false;
     private bool _wasSelected = false;
     private Vector3 _initialPosition;
     private Vector3 _centerPosition;
     
        private void Start()
        {
            _initialPosition = transform.position;
            _centerPosition = new Vector3(0, _initialPosition.y, 0);
        }
        
        private void Update()
        {
            if (_animate)
            {
                SlideUp();
            }
            if (_moveToCenter)
            {
                MoveToCenter();
            }
        }

        public void Animate()
        {
            _animate = true;
        }

        private void SlideUp()
        {
            if (transform.position.z > 3)
            {
                _animate = false;
                GetComponentInParent<TileSelector>().DestroyChildren();
                return;
            }
            transform.position += new Vector3(0, 0, 4 * Time.deltaTime);
        }

        private void OnMouseUpAsButton()
        {
            //The selected tile was chosen
            if (_wasSelected) return;
            GetComponentInParent<TileSelector>().SetChosenTile(gameObject);
            _moveToCenter = true;
            _wasSelected = true;
        }

        private void MoveToCenter()
        {
            if (transform.position == _centerPosition)
            {
                _moveToCenter = false;
            }
            else
            {
                var _step = 4 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _centerPosition, _step);
            }
        }
 }