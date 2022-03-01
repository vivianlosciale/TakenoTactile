using UnityEngine;
using UnityEngine.UI;

public class GameActions : MonoBehaviour
{
    //SCENE OBJECTS
    public AudioSource soundManager;
    public AudioClip displayTilesSound;
    public AudioClip startTurn;
    public AudioClip errorSound;
    public AudioClip cardSound;
    
    public GameObject diceRoller;
    public GameObject diceChecker;
    public GameObject uiElements;
    public GameObject ARCanvas;
    public GameObject endTurn;
    public GameObject hand;
    public GameObject tileSelector;
    public Text playerName;
    public GameObject handBackground;
    public GameObject chooseDiceFace;
    private PopUpSystem _popUpSystem;
    
    public bool turnStarted;
    
    // PRIVATE ELEMENTS
    private DiceChecker _checker; 
    private MobileClient _mobileClient;
    
    //Prefabs
    public GameObject cardPrefab;
    public GameObject tilePrefab;


    void Start()
    {
        _mobileClient = GameObject.FindWithTag(TagManager.MobileClient.ToString()).GetComponent<MobileClient>();
        _mobileClient.SetGameActions(this);
        playerName.text = _mobileClient.GetPlayerName();
        _popUpSystem = GameObject.FindWithTag(TagManager.PopUpManager.ToString()).GetComponent<PopUpSystem>();
    }

    void Update()
    {
        if (turnStarted)
        {
            DiceFaces result = RetrieveResult();
            if (!result.Equals(DiceFaces.None))
            {
                turnStarted = false;
                SendDiceResultToServer(result);
            }
        }
    }

    public void DisplayError(string error)
    {
        soundManager.PlayOneShot(errorSound);
        _popUpSystem.PopUp("Erreur : " + error);
    }

    public void QuestionMark()
    {
        _popUpSystem.NewPopUp("Action météo\nLe temps vous sourit ! Choisissez une face de dé.",
            () =>
            {
                chooseDiceFace.SetActive(true);
                uiElements.SetActive(false);
                ARCanvas.SetActive(false);
                hand.SetActive(false);
            },
            "D'accord");
    }

    public void SendDiceResultToServer(DiceFaces result)
    {
        uiElements.SetActive(true);
        ARCanvas.SetActive(true);
        hand.SetActive(true);
        diceRoller.SetActive(false);
        chooseDiceFace.SetActive(false);
        _checker.ResetDice();
        _mobileClient.SendDiceResult(result);
    }

    public void StartTurn()
    {
        soundManager.PlayOneShot(startTurn);
        hand.GetComponent<HandManagement>().UpdateCardsPosition();
        _popUpSystem.StartTurnPopUp();
    }

    public void InvokeDice()
    {
        uiElements.SetActive(false);
        hand.SetActive(false);
        diceRoller.SetActive(true);
        ARCanvas.SetActive(false);
        diceChecker = GameObject.FindWithTag(TagManager.DiceCheckerFloor.ToString());
        _checker = diceChecker.GetComponent<DiceChecker>();
    }

    private DiceFaces RetrieveResult()
    {
        if (!_checker.wasTriggered) return DiceFaces.None;
        Debug.Log("Dice result was : " + _checker.result );
        return _checker.result;
    }

    public void PlaceBamboo(bool canPlaceBamboo)
    {
    /*
       true -> the player should place a bamboo at the gardener position
       false -> no bamboo is able to grow here
     */
    if (canPlaceBamboo)
    {
        _popUpSystem.PopUp("Placez un bambou sur la case du jardinier.");
    }
    else
    {
        soundManager.PlayOneShot(errorSound);
        _popUpSystem.PopUp("Vous ne pouvez pas placer de bambou ici !");
    }
    }
    
    public void AddCardToHand(string cardName)
    {
        soundManager.PlayOneShot(cardSound);
        CreateMaterial(cardPrefab, hand.transform, "Cards", cardName );
        hand.GetComponent<HandManagement>().UpdateCardsPosition();
    }

    public void DisplayTilesToChoose(string tileNames)
    {
        Debug.Log("Displaying tiles to choose");
        soundManager.PlayOneShot(displayTilesSound);
        Handheld.Vibrate();
        hand.GetComponent<HandManagement>().UpdateCardsPosition();
        HideHand();
        ARCanvas.SetActive(false);
        var tiles = MultiNames.ToNames(tileNames);
        foreach (var tile in tiles)
        {
            CreateTile(tile);
        }
        tileSelector.GetComponent<TileSelector>().PlaceTiles();
    }

    public void TilePlaced()
    {
        tileSelector.GetComponent<TileSelector>().SlideTile();
        ShowHand();
        ARCanvas.SetActive(true);
    }

    public void ValidateChoice(bool b)
    {
        if (b)
        {
            _popUpSystem.ValidateYourActionsPopUp();
        }
        else
        {
            _popUpSystem.HidePopUp();
        }
    }

    public void ValidateObjective(string objectiveName)
    {
        hand.GetComponent<HandManagement>().ObjectiveWasValidated(objectiveName);
    }

    public void InvalidObjective()
    {
        soundManager.PlayOneShot(errorSound);
        _popUpSystem.PopUp("Vous ne pouvez pas (encore) valider cette carte !");
    }
    
    private void CreateTile(string tileName)
    {
        CreateMaterial(tilePrefab, tileSelector.transform, "Tiles", tileName );
    }

    public void EndTurn()
    {
        _mobileClient.EndTurn();
        endTurn.SetActive(false);
    }

    public void WaitingEndTurn()
    {
        endTurn.SetActive(true);
    }

    private void CreateMaterial(GameObject prefab, Transform parentTransform, string type, string objectName)
    {
        GameObject newObject = Instantiate(prefab, parentTransform);
        newObject.name = objectName;
        string face = type.Equals("Tiles") ? "tiles_face" : "card_face";
        Material newMat = new Material(Resources.Load<Material>("Models/Material/"+face));
        var texture = Resources.Load<Texture2D>("Images/"+ type +"/" + objectName);
        newMat.mainTexture = texture;
        newMat.SetTexture("_EmissionMap", texture);
        var materials = newObject.GetComponent<MeshRenderer>().materials;
        materials[1] = newMat;
        newObject.GetComponent<MeshRenderer>().materials = materials;
        newObject.AddComponent<BoxCollider>();
        if (type.Equals("Tiles"))
        {
            newObject.AddComponent<TileSlidingAnimation>();
        }
        else if(type.Equals("Cards"))
        {
            newObject.AddComponent<CardSlidingAnimation>();
        }
    }

    private void HideHand()
    {
        hand.SetActive(false);
        handBackground.SetActive(false);
    }
    
    private void ShowHand()
    {
        hand.SetActive(true);
        handBackground.SetActive(true);
    }
}