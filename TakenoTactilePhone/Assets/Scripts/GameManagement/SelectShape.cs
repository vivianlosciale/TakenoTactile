using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectShape : MonoBehaviour
{
    public Text text;

    int selectedPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectPlayer(int player)
    {
        selectedPlayer = player;
        text.text = "Select a player shape : " + selectedPlayer;
    }
}
