﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicOperation : MonoBehaviour {


    public Button moveButton;
    public Button treatButton;
    public Button cureButton;
    public Button buildButton;
    public Button shareButton;
    public Button passButton;
    public Button contingencyPlannerSkillButton;
    public Button archivistSkillButton;
    public Button fieldOperativeSkillButton;
	public Button epidemiologistSkillButton;
    public Button dispatcherSkillButton;
    public Button operationsExpertSkillButton;
    public Button colonelSkillButton;

    Button roleOnlyButton = null;
    Game game;
    Player me;
    City currentCity;
    Player currentPlayer;
    // Use this for initialization
    void Start () {
       // game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
		game = Game.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
        me = game.FindPlayer(PhotonNetwork.player);


		if (me.getRoleKind () == RoleKind.ContingencyPlanner) {
			contingencyPlannerSkillButton.gameObject.SetActive (true);
			roleOnlyButton = contingencyPlannerSkillButton;
		} 
        else if (me.getRoleKind() == RoleKind.Archivist)
        {
            archivistSkillButton.gameObject.SetActive(true);
            roleOnlyButton = archivistSkillButton;
        }
        else if (me.getRoleKind() == RoleKind.FieldOperative)
        {
            fieldOperativeSkillButton.gameObject.SetActive(true);
            roleOnlyButton = fieldOperativeSkillButton;
        }
        else if (me.getRoleKind() == RoleKind.Epidemiologist)
        {
            epidemiologistSkillButton.gameObject.SetActive(true);
            roleOnlyButton = epidemiologistSkillButton;
        }
        else if (me.getRoleKind() == RoleKind.Dispatcher)
        {
            dispatcherSkillButton.gameObject.SetActive(true);
            roleOnlyButton = dispatcherSkillButton;
        }
        else if(me.getRoleKind() == RoleKind.OperationsExpert)
        {
            operationsExpertSkillButton.gameObject.SetActive(true);
            roleOnlyButton = operationsExpertSkillButton;
        }
        else if(me.getRoleKind() == RoleKind.Colonel)
        {
            colonelSkillButton.gameObject.SetActive(true);
            roleOnlyButton = colonelSkillButton;
        }
        if ((currentPlayer == me) && (game.getCurrentPhase() == GamePhase.PlayerTakeTurn))
        {
            currentCity = me.getPlayerPawn().getCity();
            if (me.getRemainingAction() > 0)
            {
                if (!game.isGovernmentInterference())
                {
                    moveButton.GetComponent<Button>().interactable = true;
                }
                if (!currentCity.getHasResearch() && (currentPlayer.containsSpecificCityCard(currentCity) || (currentPlayer.getRoleKind() == RoleKind.OperationsExpert))||
                    (currentCity.getMarker()==0&&(game.getChallenge()!=Challenge.BioTerroist)&&(game.getChallenge()!=Challenge.BioTerroistAndVirulentStrain)))
                {
                    buildButton.GetComponent<Button>().interactable = true;
                }
                if (currentCity.hasCubes())
                {
                    treatButton.GetComponent<Button>().interactable = true;
                }
                List<CityCard> cityCards = new List<CityCard>();
                if (currentPlayer.hasEnoughCard(Color.red) || currentPlayer.hasEnoughCard(Color.blue) || currentPlayer.hasEnoughCard(Color.black) || currentPlayer.hasEnoughCard(Color.yellow))
                {
                    cureButton.GetComponent<Button>().interactable = true;
                }

                List<Player> players = game.getPlayers(currentCity);
                if (players.Count > 1)
                {
                    foreach (Player p in players)
                    {
                        if ((p.getRoleKind() == RoleKind.Researcher && p.containsCityCard()) || p.containsSpecificCityCard(currentCity))
                        {
                            shareButton.GetComponent<Button>().interactable = true;
                        }
                    }
                }

                passButton.GetComponent<Button>().interactable = true;

                //Buttons for different role skills
                if (game.containsEventCardInDiscardPile())
                {
                    contingencyPlannerSkillButton.GetComponent<Button>().interactable = true;
                }
                if (game.containsSpecificCityCardInDiscardPile(currentCity) && (!me.getOncePerTurnAction()))
                {
                    archivistSkillButton.GetComponent<Button>().interactable = true;
                }
                if (currentCity.hasCubes()&&(!me.getOncePerTurnAction()))
                {
                    fieldOperativeSkillButton.GetComponent<Button>().interactable = true;
                }
                foreach (Player otherPlayer in game.getPlayers())
                {
                    if (me.getPlayerPawn().getCity() != otherPlayer.getPlayerPawn().getCity())
                    {
                        dispatcherSkillButton.GetComponent<Button>().interactable = true;
                        break;
                    }
                }
                if (currentCity.getHasResearch() && me.containsCityCard()&&(!me.getOncePerTurnAction()))
                {
                    operationsExpertSkillButton.GetComponent<Button>().interactable = true;
                }
                if (!me.getOncePerTurnAction())
                {
                    foreach (Player otherPlayer in game.getPlayers())
                    {
                        if ((otherPlayer != me) && (otherPlayer.getPlayerPawn().getCity() == currentCity) && (otherPlayer.containsCityCard()))
                        {
                            epidemiologistSkillButton.GetComponent<Button>().interactable = true;
                        }
                    }
                }
                else
                {
                    Debug.Log("It moves to else");
                    epidemiologistSkillButton.GetComponent<Button>().interactable = false;
                }
                
                if (me.containsCityCard())
                {
                    colonelSkillButton.GetComponent<Button>().interactable = true;
                }
            }
            else if (me.getRemainingAction() == 0)
            {
                moveButton.GetComponent<Button>().interactable = false;
                treatButton.GetComponent<Button>().interactable = false;
                cureButton.GetComponent<Button>().interactable = false;
                shareButton.GetComponent<Button>().interactable = false;
                buildButton.GetComponent<Button>().interactable = false;
                if (roleOnlyButton != null)
                {
                    roleOnlyButton.GetComponent<Button>().interactable = false;
                }
                passButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                resetAll();
            }
        }
        else
        {
            resetAll();
        }     
    }


    public void resetAll()
    {
        moveButton.GetComponent<Button>().interactable = false;
        treatButton.GetComponent<Button>().interactable = false;
        cureButton.GetComponent<Button>().interactable = false;
        shareButton.GetComponent<Button>().interactable = false;
        buildButton.GetComponent<Button>().interactable = false;
        if (roleOnlyButton != null)
        {
            roleOnlyButton.GetComponent<Button>().interactable = false;
        }
        passButton.GetComponent<Button>().interactable = false;
    }
}
