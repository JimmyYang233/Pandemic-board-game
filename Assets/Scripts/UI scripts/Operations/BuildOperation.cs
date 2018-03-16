using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildOperation : MonoBehaviour {
	public Game game;
	Player currentPlayer;
	City currentCity;
	//Button build;
	// Use this for initialization
	void Start () {

	}

    private void Update()
    {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
    }

    public void buildButtonClicked()
    {
        if(game.getRemainingResearch() == 0)
        {
            //to-do, 再说
        }
        else
        {
            game.Build(currentCity.getCityName().ToString());
        }
	}
}
