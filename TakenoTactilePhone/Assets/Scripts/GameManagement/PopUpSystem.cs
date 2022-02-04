using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    //public Animator animator;
    public TMP_Text popUpText;
    public GameObject popUpButton;

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
        popUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        popUpButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            HidePopUp();
        });
    }

}
        
    