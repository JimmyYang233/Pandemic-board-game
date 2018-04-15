using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioSabotageOperation : MonoBehaviour {

    public Game game;
    Player currentPlayer;
    City currentCity;
	
	// Update is called once per frame
	void Update () {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
	}
}
