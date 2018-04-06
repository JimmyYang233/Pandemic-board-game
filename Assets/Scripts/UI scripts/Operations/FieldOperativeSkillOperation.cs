using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldOperativeSkillOperation : MonoBehaviour {

    public Game game;
    public Button takeBlueButton;
    public Button takeRedButton;
    public Button takeBlackButton;
    public Button takeYellowButton;
    public Button cancelButton;

    Player currentPlayer = null;
    City currentCity = null;

    private void Update()
    {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
    }

    public  void removeACubeButtonClicked()
    {
        if (currentCity.getCubeNumber(Color.blue) > 0)
        {
            takeBlueButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            takeBlueButton.GetComponent<Button>().interactable = false;
        }
        if(currentCity.getCubeNumber(Color.black) > 0)
        {
            takeBlackButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            takeBlackButton.GetComponent<Button>().interactable = false;
        }
        if (currentCity.getCubeNumber(Color.red) > 0)
        {
            takeRedButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            takeRedButton.GetComponent<Button>().interactable = false;
        }
        if (currentCity.getCubeNumber(Color.yellow) > 0)
        {
            takeYellowButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            takeYellowButton.GetComponent<Button>().interactable = false;
        }
        cancelButton.GetComponent<Button>().interactable = true;
    }

    public void takeBlueButtonClicked()
    {
        removeCube(Color.blue);
    }

    public void takeRedButtonClicked()
    {
        removeCube(Color.red);
    }

    public void takeBlackButtonClicked()
    {
        removeCube(Color.black);
    }

    public void takeYellowButtonClicked()
    {
        removeCube(Color.yellow);
    }

    public void returnBlueClicked()
    {
        returnACube(Color.blue);
    }

    public void returnBlackClicked()
    {
        returnACube(Color.black);
    }

    public void returnRedClicked()
    {
        returnACube(Color.red);
    }

    public void returnYellowClicked()
    {
        returnACube(Color.yellow);
    }

    private void returnACube(Color color)
    {
        //TO-DO I need backend function
    }

    private void removeCube(Color color)
    {
        game.FieldOperativeSample(color);
    }
}
