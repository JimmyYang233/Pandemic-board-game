using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioMoveOperation : MonoBehaviour {

    public Button driveButton;
    public Button directFlightButton;
    public Button charterFlightButton;
    public Button cancelButton;
    public Game game;


    public void moveButtonClicked()
    {
        BioTerrorist bioterrorist = game.getBioTerrorist();
        Player currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        driveButton.gameObject.GetComponent<Button>().interactable = true;
        if (currentPlayer.containsInfectionCard())
        {
            directFlightButton.gameObject.GetComponent<Button>().interactable = true;
        }

        if (currentPlayer.containsSpecificInfectionCard(currentCity))
        {
            charterFlightButton.gameObject.GetComponent<Button>().interactable = true;
        }

        cancelButton.gameObject.GetComponent<Button>().interactable = true;
    }
}
