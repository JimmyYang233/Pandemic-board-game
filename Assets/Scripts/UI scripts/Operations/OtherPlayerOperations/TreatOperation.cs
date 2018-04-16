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
    public Button purpleButton;
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
        if (game.getChallenge() != Challenge.Nochallenge && game.getChallenge() != Challenge.VirulentStrain)
        {
            purpleButton.gameObject.SetActive(true);
            if (currentCity.getCubeNumber(Color.magenta) > 0)
            {
                purpleButton.GetComponent<Button>().interactable = true;
            }
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
        purpleButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
		game.TreatDisease("blue", currentCity.getCityName().ToString());
    }

    public void TreatYellowClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        purpleButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
		game.TreatDisease("yellow", currentCity.getCityName().ToString());
    }

    public void TreatBlackClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        purpleButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
		game.TreatDisease("black", currentCity.getCityName().ToString());
    }

    public void TreatRedClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        purpleButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
		game.TreatDisease("red", currentCity.getCityName().ToString());
    }

    public void TreatPurpleClicked()
    {
        Debug.Log("Treat Purple used");
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        purpleButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;
        game.TreatDisease("purple", currentCity.getCityName().ToString());
    }

    public void cancelButtonClicked()
    {
        redButton.GetComponent<Button>().interactable = false;
        blueButton.GetComponent<Button>().interactable = false;
        blackButton.GetComponent<Button>().interactable = false;
        yellowButton.GetComponent<Button>().interactable = false;
        purpleButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = false;
    }
}
