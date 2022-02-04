using System;
using TMPro;
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
    private MobileClient mobileClient;
    public GameObject cardPrefab;
    public GameObject endTurn;
    public GameObject popUpManager;
    public GameObject popUpText;
    public GameObject popUpButton;
    public GameObject hand;
    
    
    void Start()
    {
        mobileClient = GameObject.FindWithTag(TagManager.MobileClient.ToString()).GetComponent<MobileClient>();
        mobileClient.SetGameActions(this);
    }

    void Update()
    {
        if (turnStarted)
        {
            DiceFaces result = RetrieveResult();
            if (!result.Equals(DiceFaces.None))
            {
                if (result.Equals(DiceFaces.Questionmark))
                {
                    result = DiceFaces.Cloud;
                }
                turnStarted = false;
                SendResultToServer(result);
            }
        }
    }

    private void SendResultToServer(DiceFaces result)
    {
        uiElements.SetActive(true);
        hand.SetActive(true);
        diceRoller.SetActive(false);
        checker.ResetDice();
        mobileClient.SendDiceResult(result);
    }

    public void StartTurn()
    {
        popUpManager.SetActive(true);
        popUpText.GetComponent<TextMeshProUGUI>().SetText("It is now your turn.");
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                turnStarted = true;
                Debug.Log("TURN STARTED ON CLICK");
                InvokeDice();
                popUpManager.GetComponent<PopUpSystem>().HidePopUp();
            }
            );
    }

    private void InvokeDice()
    {
        uiElements.SetActive(false);
        hand.SetActive(false);
        diceRoller.SetActive(true);
        diceChecker = GameObject.FindWithTag(TagManager.DiceCheckerFloor.ToString());
        checker = diceChecker.GetComponent<DiceChecker>();
    }

    private DiceFaces RetrieveResult()
    {
        if (!checker.wasTriggered) return DiceFaces.None;
        Debug.Log("Dice result was : " + checker.result );
        return checker.result;
    }

    public void AddCardToHand(string cardName)
    {
        GameObject hand = GameObject.FindWithTag(TagManager.Hand.ToString());
        GameObject newCard = Instantiate(cardPrefab, hand.transform);
        Material newMat = new Material(Resources.Load<Material>("Models/Material/card_face"));
        var texture = Resources.Load<Texture2D>("Images/Cards/" + cardName);
        newMat.mainTexture = texture;
        newMat.SetTexture("_EmissionMap", texture);
        var materials = newCard.GetComponent<MeshRenderer>().materials;
        materials[1] = newMat;
        newCard.GetComponent<MeshRenderer>().materials = materials;
        endTurn.SetActive(true);
    }

    public void EndTurn()
    {
        mobileClient.EndTurn();
        endTurn.SetActive(false);
    }
}