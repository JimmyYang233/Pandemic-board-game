using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioTerroristOperation : MonoBehaviour {

    public Game game;
    public Button drawButton;
    public Button moveButton;
    public Button infectButton;
    public Button sabotageButton;
    public Button escapeButton;
    public Button passButton;
    public MoveOperation moveOperation;
    Player me;
    Player currentPlayer;

	
	// Update is called once per frame
	void Update () {
        me = game.FindPlayer(PhotonNetwork.player);
        currentPlayer = game.getCurrentPlayer();
        if(me.getRoleKind() == RoleKind.BioTerrorist&&(me == currentPlayer)&&(game.getCurrentPhase() == GamePhase.PlayerTakeTurn))
        {
            BioTerrorist bio = game.getBioTerrorist();
            if (me.getRemainingAction() != 0)
            {
                drawButton.GetComponent<Button>().interactable = true;
                if (!bio.getIsCaptured())
                {
                    moveButton.GetComponent<Button>().interactable = true;
                }
                infectButton.GetComponent<Button>().interactable = true;
                sabotageButton.GetComponent<Button>().interactable = true;
                escapeButton.GetComponent<Button>().interactable = true;
                passButton.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            resetAll();
        }
	}

    public void resetAll()
    {
        drawButton.GetComponent<Button>().interactable = false;
        moveButton.GetComponent<Button>().interactable = false;
        infectButton.GetComponent<Button>().interactable = false;
        sabotageButton.GetComponent<Button>().interactable = false;
        escapeButton.GetComponent<Button>().interactable = false;
        passButton.GetComponent<Button>().interactable = false;
    }
}
