using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image loadingCircle;

    // Update is called once per frame
    public void AddPlayer()
    {
        loadingCircle.fillAmount += 0.25f;
    }

    public void RemovePlayer()
    {
        loadingCircle.fillAmount -= 0.25f;
    }
}
