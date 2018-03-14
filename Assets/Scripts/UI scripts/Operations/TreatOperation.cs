using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreatOperation : MonoBehaviour {
    Game game;
    Player currentPlayer;
    public Button blueButton;
    public Button yellowButton;
    public Button blackButton;
    public Button redButton;
    public Button cancelButton;

    void Start()
    {
        //game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
		game = Game.Instance;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void treatButtonClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        if(currentCity.getCubeNumber(Color.blue) > 0)
        {
            blueButton.GetComponent<Button>().interactable = true;
        }
        if (currentCity.getCubeNumber(Color.yellow) > 0)
        {
            yellowButton.GetComponent<Button>().interactable = true;
        }
        if (currentCity.getCubeNumber(Color.black) > 0)
        {
            blackButton.GetComponent<Button>().interactable = true;
        }
        if (currentCity.getCubeNumber(Color.red) > 0)
        {
            redButton.GetComponent<Button>().interactable = true;
        }
        cancelButton.GetComponent<Button>().interactable = true;

    }

    public void TreatBlueClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
        game.treatDisease(game.getDisease(Color.blue), currentCity);
    }

    public void TreatYellowClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
        game.treatDisease(game.getDisease(Color.yellow), currentCity);
    }

    public void TreatBlackClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
        game.treatDisease(game.getDisease(Color.black), currentCity);
    }

    public void TreatRedClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
        game.treatDisease(game.getDisease(Color.red), currentCity);
    }
}
