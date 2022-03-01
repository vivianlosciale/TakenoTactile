using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
    {
        public void Start()
        {
            var result = GameObject.FindWithTag(TagManager.MoveObject.ToString()).GetComponent<MoveObject>()
                .GetResult();
            GetComponent<Text>().text = result;
        }
    }