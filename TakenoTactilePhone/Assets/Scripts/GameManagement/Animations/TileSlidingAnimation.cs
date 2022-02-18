 using UnityEngine;

 public class TileSlidingAnimation : MonoBehaviour
 {
     
     private bool animate = false;
        private void Update()
        {
            if (animate)
            {
                SlideUp();
            }
        }

        public void Animate()
        {
            animate = true;
        }

        private void SlideUp()
        {
            if (transform.position.z > 3)
            {
                animate = false;
                GetComponentInParent<TileSelector>().ChangeNeeded();
                GetComponentInParent<TileSelector>().DestroyChildren();
                return;
            }
            transform.position += new Vector3(0, 0, 4 * Time.deltaTime);
        }
    }