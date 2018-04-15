using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioInfectOperation : MonoBehaviour {

    public Game game;
    public Button infectLocallyButton;
    public Button ifectRemotellyButton;
    public Button cancelButton;
    Player currentPlayer;
    City currentCity;

    public void infectButtonClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
        infectLocallyButton.gameObject.GetComponent<Button>().interactable = true;
    }
}
