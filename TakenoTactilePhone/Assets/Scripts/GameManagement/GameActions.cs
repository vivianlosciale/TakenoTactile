using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameActions : MonoBehaviour
{
    //SCENE OBJECTS
    public GameObject diceRoller;
    public GameObject diceChecker;
    public GameObject uiElements;
    public GameObject endTurn;
    public GameObject hand;
    public GameObject tileSelector;
    public Text playerName;
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
/*        tileSelector.GetComponent<TileSelector>().ChangeNeeded();
        List<string> tiles = new List<string>();
        tiles.Add("tiles_y1");
        tiles.Add("tiles_g1");
        tiles.Add("tiles_r1");
        foreach (var tile in tiles)
        {
            CreateTile(tile);
        }
        tileSelector.GetComponent<TileSelector>().PlaceTiles();*/
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
                SendDiceResultToServer(result);
            }
        }
    }

/*    public void CreateOtherChildren()
    {
        List<string> tiles = new List<string>();
        tiles.Add("tiles_r2");
        tiles.Add("tiles_y3");
        tiles.Add("tiles_g3");
        foreach (var tile in tiles)
        {
            CreateTile(tile);
        }
        tileSelector.GetComponent<TileSelector>().PlaceTiles();
    }*/

    private void SendDiceResultToServer(DiceFaces result)
    {
        uiElements.SetActive(true);
        hand.SetActive(true);
        diceRoller.SetActive(false);
        _checker.ResetDice();
        _mobileClient.SendDiceResult(result);
    }

    public void StartTurn()
    {
        _popUpSystem.StartTurnPopUp();
    }

    public void InvokeDice()
    {
        uiElements.SetActive(false);
        hand.SetActive(false);
        diceRoller.SetActive(true);
        diceChecker = GameObject.FindWithTag(TagManager.DiceCheckerFloor.ToString());
        _checker = diceChecker.GetComponent<DiceChecker>();
    }

    private DiceFaces RetrieveResult()
    {
        if (!_checker.wasTriggered) return DiceFaces.None;
        Debug.Log("Dice result was : " + _checker.result );
        return _checker.result;
    }

    public void AddCardToHand(string cardName)
    {
        CreateMaterial(cardPrefab, hand.transform, "Cards", cardName );
        hand.GetComponent<HandManagement>().UpdateCardsPosition();
    }

    public void DisplayTilesToChoose(string tileNames)
    {
        tileSelector.GetComponent<TileSelector>().ChangeNeeded();
        hand.SetActive(false);
        List<string> tiles = MultiNames.ToNames(tileNames);
        foreach (var tile in tiles)
        {
            CreateTile(tile);
        }
        tileSelector.GetComponent<TileSelector>().PlaceTiles();
    }

    public void TilePlaced()
    {
        tileSelector.GetComponent<TileSelector>().DestroyChildren();
        tileSelector.GetComponent<TileSelector>().ChangeNeeded();
        Debug.Log("TILE SELECTOR DEACTIVATED");
        hand.SetActive(true);
        Debug.Log("HAND ACTIVATED");
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
    }
}