using System;
using UnityEngine;

public class DiceChecker : MonoBehaviour
{
    //lecture de valeur
    public GameObject Dice;
    public DiceFaces result = DiceFaces.None;
    public bool wasTriggered = false;

    private void Start()
    {
        wasTriggered = false;
    }

    public void ResetDice()
    {
        wasTriggered = false;
        result = DiceFaces.None;
        ReactivateDiceTrigger();
        Debug.Log("Reset");
    }

    private void OnTriggerStay(Collider other)
    {
        if (Dice.GetComponent<Rigidbody>().velocity != Vector3.zero) return;
        DeactivateDiceTrigger();
        switch (other.gameObject.name)
        {
            case "Side1":
                result = DiceFaces.Cloud;
                break;
            case "Side2":
                result = DiceFaces.Questionmark;
                break;
            case "Side3":
                result = DiceFaces.Rain;
                break;
            case "Side4":
                result = DiceFaces.Thunder;
                break;
            case "Side5":
                result = DiceFaces.Sun;
                break;
            case "Side6":
                result = DiceFaces.Wind;
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
    
    private void ReactivateDiceTrigger()
    {
        Dice.transform.position = new Vector3(0, -1.31f, 0);
        Dice.GetComponent<Rigidbody>().isKinematic = false;
        Dice.GetComponent<MeshCollider>().enabled = true;
        Dice.GetComponent<Rigidbody>().useGravity = false;
        var interactions = Dice.GetComponent<DiceInteraction>();
        interactions.stopShaking = false;
        interactions.shookOnce = false;
    }




}
