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
    public GameObject bioTerroristPanel;
    public GameObject bioMovePanel;
    public GameObject basicOperationPanel;
    public GameObject movePanel;
    Player me;
    Player currentPlayer;
    City currentCity;

    // Update is called once per frame
    void Update() {
        me = game.FindPlayer(PhotonNetwork.player);
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
        me = game.FindPlayer(PhotonNetwork.player);
        currentPlayer = game.getCurrentPlayer();
        
        if (game.getChallenge() == Challenge.BioTerroist || game.getChallenge() == Challenge.BioTerroistAndVirulentStrain)
        {
            BioTerrorist bioTerrorist = game.getBioTerrorist();
            Pawn bioPawn = bioTerrorist.getPawn();
            
            if (me.getRoleKind() == RoleKind.BioTerrorist)
            {
                bioTerroristPanel.gameObject.SetActive(true);
                bioMovePanel.gameObject.SetActive(true);
                me.getPlayerPawn().gameObject.SetActive(true);
            }
            else
            {
                basicOperationPanel.gameObject.SetActive(true);
                movePanel.gameObject.SetActive(true);
                if (bioTerrorist.getIsSpotted())
                {
                    bioPawn.gameObject.SetActive(true);
                    if (currentPlayer == me)
                    {
                        bioPawn.gameObject.GetComponent<Button>().onClick.AddListener(() => capture());
                    }
                    else
                    {
                        bioPawn.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                    }
                }
                else
                {
                    bioPawn.gameObject.SetActive(false);
                }
            }
            if (bioTerrorist.getIsCaptured())
            {
                //Debug.Log("Captured bioterrorist");
                bioPawn.gameObject.SetActive(true);
                bioPawn.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                bioPawn.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            basicOperationPanel.gameObject.SetActive(true);
            movePanel.gameObject.SetActive(true);
        }
        if (me.getRoleKind() == RoleKind.BioTerrorist && (me == currentPlayer) && (game.getCurrentPhase() == GamePhase.PlayerTakeTurn))
        {
            BioTerrorist bio = game.getBioTerrorist();
            if (((me.getRemainingAction() > 0 )|| !(bio.getBioTerroristExtraDriveUsed()))&&!bio.getIsCaptured())
            {
                moveButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                moveButton.GetComponent<Button>().interactable = false;
            }
            if (me.getRemainingAction() > 0)
            {
                drawButton.GetComponent<Button>().interactable = true;
                if (!bio.getIsCaptured())
                {
                    infectButton.GetComponent<Button>().interactable = true;
                    if (currentCity.getHasResearch())
                    {
                        foreach(PlayerCard card in currentPlayer.getHand())
                        {
                            if(card.getType() == CardType.CityCard)
                            {
                                CityCard cityCard = (CityCard)card;
                                if (cityCard.getColor() == currentCity.getColor())
                                {
                                    sabotageButton.GetComponent<Button>().interactable = true;
                                }
                            }
                        }
                    }
                    escapeButton.GetComponent<Button>().interactable = false;
                }
                else if (bio.getIsCaptured())
                {
                    infectButton.GetComponent<Button>().interactable = false;
                    sabotageButton.GetComponent<Button>().interactable = false;
                    if (currentPlayer.containsInfectionCard())
                    {
                        escapeButton.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        escapeButton.GetComponent<Button>().interactable = false;
                    }
                }
            }
            passButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            resetAll();
        }
	}

    public void drawButtonClicked()
    {
        game.BioterroristDraw();
        resetAll();
    }


    public void capture()
    {
        Debug.Log("Bioterrorist is captured!");
        game.BioTerroristCapture();
        resetAll();
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
