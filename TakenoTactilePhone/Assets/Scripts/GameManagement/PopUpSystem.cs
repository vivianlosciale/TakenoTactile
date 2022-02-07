using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public TMP_Text popUpText;
    public GameObject popUpButton;
    private MobileClient _mobileClient;

    private GameActions _gameActions;
    
    void Start()
    {
        _mobileClient = GameObject.FindWithTag(TagManager.MobileClient.ToString()).GetComponent<MobileClient>();
        _mobileClient.SetPopUpSystem(this);
        _gameActions = GameObject.FindWithTag(TagManager.GameManager.ToString()).GetComponent<GameActions>();
    }
    
    public void PopUp(string textToDisplay)
    {
        popUpBox.SetActive(true);
        popUpText.text = textToDisplay;
    }

    public void HidePopUp()
    {
        popUpBox.SetActive(false);
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            HidePopUp();
        });
    }

    public void StartTurnPopUp()
    {
        NewPopUp("It is now your turn.",
            () =>
            {
                _gameActions.turnStarted = true;
                Debug.Log("TURN STARTED ON CLICK");
                _gameActions.InvokeDice();
                HidePopUp();
            },
            "Got it!"
            );
        /*popUpBox.SetActive(true);
        popUpText.text = "It is now your turn.";
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                _gameActions.turnStarted = true;
                Debug.Log("TURN STARTED ON CLICK");
                _gameActions.InvokeDice();
                HidePopUp();
            }
        );*/
    }

    public void ValidateYourActionsPopUp()
    {
        NewPopUp("Please validate your actions.",
            () =>
            { 
                _mobileClient.ValidateChoice();
                HidePopUp();
            },
            "Validate"
            );
        /*popUpBox.SetActive(true);
        popUpText.text = "Please validate your actions.";
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                _mobileClient.ValidateChoice();
                HidePopUp();
            }
        );*/
    }

    private void NewPopUp(string message, UnityAction action, string buttonText)
    {
        popUpBox.SetActive(true);
        popUpText.text = message;
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(action);
        popUpButton.GetComponentInChildren<Text>().text = buttonText;
    }
}
        
    