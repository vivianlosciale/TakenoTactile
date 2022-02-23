using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Reserv : MonoBehaviour
{
    public int nbBambooG;
    public int nbBambooP;
    public int nbBambooY;

    public GameObject bambooG;
    public GameObject bambooP;
    public GameObject bambooY;

    public TextMeshPro nbGreen;
    public TextMeshPro nbPink;
    public TextMeshPro nbYellow;

    public GameObject hand;
    public GameObject ARCamera;
    public GameObject UI;
    private bool ARActive;

    private float offset = 0.035f;

    public GameObject imageTarget;

    // Start is called before the first frame update
    void Start()
    {
        ARActive = false;
        imageTarget.SetActive(ARActive);
        ARCamera.SetActive(ARActive);
        handleUI();

        for (int i = 0; i < nbBambooG; i++)
        {
            var bambooGtmp = Instantiate(bambooG, imageTarget.transform);
            bambooGtmp.transform.localPosition = new Vector3(0, i * offset - 1, 0);
        }
        var nbGreentmp = Instantiate(nbGreen, imageTarget.transform);
        nbGreentmp.text = nbBambooG.ToString();

        if (nbBambooG > 5)
        {
            nbGreentmp.transform.localPosition = new Vector3(0.05f, offset - 1, 0);
        } 
        else
        {
            nbGreentmp.transform.localPosition = new Vector3(0, (nbBambooG + 1) * offset - 1, 0);
        }
        

        for (int i = 0; i < nbBambooP; i++)
        {
            var bambooPtmp = Instantiate(bambooP, imageTarget.transform);
            bambooPtmp.transform.localPosition = new Vector3(-0.05f, i * offset - 1,  0.05f);
        }
        var nbPinktmp = Instantiate(nbPink, imageTarget.transform);
        nbPinktmp.text = nbBambooP.ToString();

        if (nbBambooP > 5)
        {
            nbPinktmp.transform.localPosition = new Vector3(0, offset - 1, 0.05f);
        }
        else
        {
            nbPinktmp.transform.localPosition = new Vector3(-0.05f, (nbBambooP + 1) * offset - 1, 0.05f);
        }

        for (int i = 0; i < nbBambooY; i++)
        {
            var bambooYtmp = Instantiate(bambooY, imageTarget.transform);
            bambooYtmp.transform.localPosition = new Vector3(0.05f, i * offset - 1, 0.05f);
        }
        var nbYellowtmp = Instantiate(nbYellow, imageTarget.transform);
        nbYellowtmp.text = nbBambooY.ToString();

        if (nbBambooY > 5)
        {
            nbYellowtmp.transform.localPosition = new Vector3(0.09f, offset - 1, 0.05f);
        }
        else
        {
            nbYellowtmp.transform.localPosition = new Vector3(0.05f, (nbBambooY + 1) * offset - 1, 0.05f);
        }

    }

    private void handleUI()
    {
        ARCamera.SetActive(ARActive);
        hand.SetActive(!ARActive);
        UI.SetActive(!ARActive);
    }

    public void switchAR()
    {
        imageTarget.SetActive(!ARActive);
        ARActive = !ARActive;
        handleUI();
    }
}
