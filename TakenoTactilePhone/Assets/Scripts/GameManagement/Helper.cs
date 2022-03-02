using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject PopUp;
    public GameObject ARPopUp;
    private string _helpMessage = "Bienvenue dans Takenoko !";
    private bool shouldSave = false;

    private void Start()
    {
        var mobileClient = GameObject.FindWithTag(TagManager.MobileClient.ToString()).GetComponent<MobileClient>();
        mobileClient.SetHelper(this);
    }

    public void GetHelpMessage()
    {
        PopUp.GetComponent<PopUpSystem>().HelpPopUp(_helpMessage, "C'est compris");
    }

    public void UpdateHelpMessage(string message)
    {
        if (shouldSave)
        {
            _helpMessage += "\n" + message;
            shouldSave = false;
        }
        else
        {
            _helpMessage = message;
        }
    }

    public void DiceResultExplanation(DiceFaces face)
    {
        shouldSave = false;
        switch (face)
        {
            case DiceFaces.Rain: //conserver ce message tant que action pas effectuée
                _helpMessage = "Action météo\nIl pleut ! Vous pouvez faire pousser un bambou sur la tuile de votre choix.";
                shouldSave = true;
                break;
            case DiceFaces.Cloud: //pas implémenté
                _helpMessage = "Action météo\nLe temps est nuageux... Vous pouvez piocher un aménagement.";
                shouldSave = false;
                break;
            case DiceFaces.Sun: //conserver ce message
                _helpMessage = "Action météo\nIl fait beau ! Vous pouvez sélectionner 3 actions différentes.";
                shouldSave = true;
                break;
            case DiceFaces.Thunder: //pas implémenté
                _helpMessage = "Action météo\nSacré orage ! Déplacez le panda sur la tuile de votre choix, et croquez un bambou.";
                shouldSave = true;
                break;
            case DiceFaces.Wind: //conserver ce message
                _helpMessage = "Action météo\nVent frais, vent du matin... Vous pouvez sélectionner deux fois la même action.";
                shouldSave = true;
                break;  
            case DiceFaces.Questionmark:
                _helpMessage = "ha ha ha ha ha ha it's a prank bro";
                shouldSave = false;
                break;
        }   
    }

    public void ARExplanation()
    {
        ARPopUp.SetActive(true);
    }

    public void ExitARExplanation()
    {
        ARPopUp.SetActive(false);
    }
    
}