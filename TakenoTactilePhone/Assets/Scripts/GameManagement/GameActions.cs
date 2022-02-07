
using System.Collections.Generic;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    //SCENE OBJECTS
    public GameObject diceRoller;
    public GameObject diceChecker;
    public GameObject uiElements;
    public GameObject endTurn;
    public GameObject hand;
    public GameObject tileSelector;
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
        _popUpSystem = GameObject.FindWithTag(TagManager.PopUpManager.ToString()).GetComponent<PopUpSystem>();
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
        CreateMaterial(cardPrefab, hand.transform, "Tiles", cardName );
        /*GameObject newCard = Instantiate(cardPrefab, hand.transform);
        Material newMat = new Material(Resources.Load<Material>("Models/Material/card_face"));
        var texture = Resources.Load<Texture2D>("Images/Cards/" + cardName);
        newMat.mainTexture = texture;
        newMat.SetTexture("_EmissionMap", texture);
        var materials = newCard.GetComponent<MeshRenderer>().materials;
        materials[1] = newMat;
        newCard.GetComponent<MeshRenderer>().materials = materials;
        endTurn.SetActive(true);*/
    }

    public void DisplayTilesToChoose(string tileNames)
    {
        tileSelector.SetActive(true);
        List<string> tiles = MultiNames.ToNames(tileNames);
        foreach (var tile in tiles)
        {
            CreateTile(tile);
        }
        tileSelector.GetComponent<TileSelector>().PlaceTiles();
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
    
    private void CreateTile(string tileName)
    {
        CreateMaterial(tilePrefab, tileSelector.transform, "Tiles", tileName );
        /*GameObject newTile = Instantiate(tilePrefab, tileSelector.transform);
        newTile.name = tileName;
        Material newMat = new Material(Resources.Load<Material>("Models/Material/tiles_face"));
        var texture = Resources.Load<Texture2D>("Images/Tiles/" + tileName);
        newMat.mainTexture = texture;
        newMat.SetTexture("_EmissionMap", texture);
        var materials = newTile.GetComponent<MeshRenderer>().materials;
        materials[1] = newMat;
        newTile.GetComponent<MeshRenderer>().materials = materials;*/
    }

    public void EndTurn()
    {
        _mobileClient.EndTurn();
        endTurn.SetActive(false);
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