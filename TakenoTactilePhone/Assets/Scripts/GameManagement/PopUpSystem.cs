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
        Handheld.Vibrate();
    }

    public void HidePopUp()
    {
        popUpBox.SetActive(false);
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(HidePopUp);
    }

    public void StartTurnPopUp()
    {
        NewPopUp("A vous de jouer !",
            () =>
            {
                _gameActions.turnStarted = true;
                _gameActions.InvokeDice();
                HidePopUp();
            },
            "Oui !"
            );
    }

    public void ValidateYourActionsPopUp()
    {
        NewPopUp("Veuillez confirmer ou modifier vos actions.",
            () =>
            { 
                _mobileClient.ValidateChoice();
                HidePopUp();
            },
            "Confirmer"
            );
    }

    public void HelpPopUp(string message, string buttonText)
    {
        NewHelpPopUp(message, () =>
        {
            HidePopUp();
        }, 
            buttonText);
    }

    private void NewPopUp(string message, UnityAction action, string buttonText)
    {
        Handheld.Vibrate();
        popUpBox.SetActive(true);
        popUpText.text = message;
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(action);
        popUpButton.GetComponentInChildren<Text>().text = buttonText;
    }
    
    private void NewHelpPopUp(string message, UnityAction action, string buttonText)
    {
        popUpBox.SetActive(true);
        popUpText.text = message;
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(action);
        popUpButton.GetComponentInChildren<Text>().text = buttonText;
    }
}
        
    