using UnityEngine;

public class DiceChecker : MonoBehaviour
{
    public GameObject Dice;
    public static DiceFaces result = DiceFaces.NONE;

    private void OnTriggerStay(Collider other)
    {
        if (Dice.GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
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
        }
    }
    private void DeactivateDiceTrigger()
    {
        Dice.GetComponent<Rigidbody>().isKinematic = true;
        foreach (Transform child in Dice.transform)
        {
            child.GetComponent<BoxCollider>().enabled = false;
        }
    }




}
