using System;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject PopUp;
    public GameObject ARPopUp;
    private string _helpMessage = "Bienvenue dans Takenoko !";

    private void Start()
    {
        var _mobileClient = GameObject.FindWithTag(TagManager.MobileClient.ToString()).GetComponent<MobileClient>();
        _mobileClient.SetHelper(this);
    }

    public void GetHelpMessage()
    {
        PopUp.GetComponent<PopUpSystem>().HelpPopUp(_helpMessage, "C'est compris");
    }

    public void UpdateHelpMessage(string message)
    {
        _helpMessage = message;
    }

    public void DiceResultExplanation(DiceFaces face)
    {
        switch (face)
        {
            case DiceFaces.Rain: //conserver ce message tant que action pas effectuée
                _helpMessage = "Action météo\nIl pleut ! Vous pouvez faire pousser un bambou sur la tuile de votre choix.";
                break;
            case DiceFaces.Cloud: //pas implémenté
                _helpMessage = "Action météo\nLe temps est nuageux... Vous pouvez piocher un aménagement.";
                break;
            case DiceFaces.Sun: //conserver ce message
                _helpMessage = "Action météo\nIl fait beau ! Vous pouvez sélectionner 3 actions différentes.";
                break;
            case DiceFaces.Thunder: //pas implémenté
                _helpMessage = "Action météo\nSacré orage ! Déplacez le panda sur la tuile de votre choix, et croquez un bambou.";
                break;
            case DiceFaces.Wind: //conserver ce message
                _helpMessage = "Action météo\nVent frais, vent du matin... Vous pouvez sélectionner deux fois la même action.";
                break;  
            case DiceFaces.Questionmark:
                _helpMessage = "ha ha ha ha ha ha it's a prank bro";
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