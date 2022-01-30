using System;
using UnityEngine;

public class DiceChecker : MonoBehaviour
{
    //lecture de valeur
    public GameObject Dice;
    public DiceFaces result = DiceFaces.NONE;
    public bool wasTriggered = false;

    private void Start()
    {
        wasTriggered = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Dice.GetComponent<Rigidbody>().velocity != Vector3.zero) return;
        DeactivateDiceTrigger();
        switch (other.gameObject.name)
        {
            case "Side1":
                result = DiceFaces.CLOUD;
                break;
            case "Side2":
                result = DiceFaces.QUESTIONMARK;
                break;
            case "Side3":
                result = DiceFaces.RAIN;
                break;
            case "Side4":
                result = DiceFaces.THUNDER;
                break;
            case "Side5":
                result = DiceFaces.SUN;
                break;
            case "Side6":
                result = DiceFaces.WIND;
                break;
            default:
                Debug.LogError("Face not Found with name:" + other.gameObject.name);
                break;
        }
        wasTriggered = true;
    }
    private void DeactivateDiceTrigger()
    {
        Dice.GetComponent<Rigidbody>().isKinematic = true;
        Dice.GetComponent<MeshCollider>().enabled = false;
    }




}
