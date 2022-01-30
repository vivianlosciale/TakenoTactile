using UnityEngine;
using UnityEngine.UI;

public class GameActions : MonoBehaviour
{
    //public MobileClient MobileClient;
    public Text playerName;
    public GameObject diceRoller;
    public GameObject diceChecker;
    public GameObject uiElements;
    private bool turnStarted = false;
    private DiceChecker checker;
    
    void Start()
    {
        //MobileClient = GameObject.FindWithTag(TagManager.MobileClient.ToString()).GetComponent<MobileClient>();
        //playerName.text = MobileClient.tampon;
    }

    void Update()
    {
        if (turnStarted)
        {
            RetrieveResult();
        }
    }

    public void StartTurn()
    {
        turnStarted = true;
        InvokeDice();
    }

    private void InvokeDice()
    {
        uiElements.SetActive(false);
        diceRoller.SetActive(true);
        diceChecker = GameObject.FindWithTag(TagManager.DiceCheckerFloor.ToString());
        checker = diceChecker.GetComponent<DiceChecker>();
    }

    private void RetrieveResult()
    {
        if (!checker.wasTriggered) return;
        Debug.Log("Dice result was : " + checker.result );
        turnStarted = false;
    }
}