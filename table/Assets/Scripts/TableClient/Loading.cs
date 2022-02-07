using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    int nbPlayers = 0;

    public Image loadingCircle;


    // Update is called once per frame
    void Update()
    {
        loadingCircle.fillAmount = 0.25f * nbPlayers;
    }
}
