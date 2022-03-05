using UnityEngine;

public class SelectDiceFace : MonoBehaviour
{

    public GameObject GameManager;
    private GameActions _gameActions;
    
    public void Start()
    {
        _gameActions = GameManager.GetComponent<GameActions>();
    }

    public void SendSelectedDiceFace()
    {
        _gameActions.SendDiceResultToServer(DiceFacesMethods.ToDiceFace(gameObject.name));
    }
    
}