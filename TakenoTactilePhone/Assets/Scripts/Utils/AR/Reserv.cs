using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reserv : MonoBehaviour
{
    public int nbBambooG;
    public int nbBambooP;
    public int nbBambooY;

    public GameObject bambooG;
    public GameObject bambooP;
    public GameObject bambooY;

    public GameObject hand;
    public GameObject ARCamera;
    public GameObject UI;
    private bool ARActive;

    public GameObject imageTarget;

    // Start is called before the first frame update
    void Start()
    {
        ARActive = false;

        for (int i = 0; i < nbBambooG; i++)
        {
            var bambooGtmp = Instantiate(bambooG, imageTarget.transform);
            bambooGtmp.transform.localPosition = new Vector3(0, i * 0.035f - 1, 0);
            //bambooGtmp.transform.Rotate(new Vector3(-90, 0, 0));
        }
        for (int i = 0; i < nbBambooP; i++)
        {
            var bambooPtmp = Instantiate(bambooP, imageTarget.transform);
            bambooPtmp.transform.localPosition = new Vector3(0.03f, i * 0.035f - 1,  0.03f);
           // bambooPtmp.transform.Rotate(new Vector3(-90, 0, 0));
        }
        for (int i = 0; i < nbBambooY; i++)
        {
            var bambooYtmp = Instantiate(bambooY, imageTarget.transform);
            bambooYtmp.transform.localPosition = new Vector3(0.03f, i * 0.035f - 1, 0.03f);
            //bambooYtmp.transform.Rotate(new Vector3(-90, 0, 0));
        }
    }

    private void Update()
    {
        ARCamera.SetActive(ARActive);
        hand.SetActive(!ARActive);
        UI.SetActive(!ARActive);
    }

    public void switchAR()
    {
        if (ARActive)
        {
            foreach (Transform transform in imageTarget.transform)
            {
                Destroy(transform.gameObject);
            }
        }
        ARActive = !ARActive;
    }
}
