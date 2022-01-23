using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MobileClient MobileClient;
    public Text playerName;

    void Start()
    {
        MobileClient = GameObject.FindWithTag(TagManager.MobileClient.ToString()).GetComponent<MobileClient>();
        //playerName.text = MobileClient.tampon;
    }
}