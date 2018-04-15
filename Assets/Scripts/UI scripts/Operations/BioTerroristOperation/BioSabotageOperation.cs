using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioSabotageOperation : MonoBehaviour {

    public Game game;
    Player currentPlayer;
    City currentCity;
    public Button useSabotageButton;
	
	// Update is called once per frame
	void Update () {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
	}

    public void sabotageButtonClicked()
    {

    }
}
