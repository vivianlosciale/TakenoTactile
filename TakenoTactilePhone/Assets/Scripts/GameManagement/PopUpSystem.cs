using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    //public Animator animator;
    public TMP_Text popUpText;
    

    public void PopUp(string textToDisplay)
    {
        popUpBox.SetActive(true);
        //Screen.height;
        popUpText.text = textToDisplay;
        //animator.SetTrigger("pop");
    }

    public void HidePopUp()
    {
        popUpBox.SetActive(false);
    }

}
        
    