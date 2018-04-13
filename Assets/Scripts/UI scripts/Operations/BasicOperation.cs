using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicOperation : MonoBehaviour {

    public GameObject basicOperationPanel;
    public GameObject bioTerroristPanel;
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
		me = game.FindPlayer(PhotonNetwork.player);
        currentPlayer = game.getCurrentPlayer();
        if(me.getRoleKind() == RoleKind.BioTerrorist)
        {
            bioTerroristPanel.gameObject.SetActive(true);
        }
        else
        {
            basicOperationPanel.gameObject.SetActive(true);
            if (me.getRoleKind() == RoleKind.ContingencyPlanner)
            {
                contingencyPlannerSkillButton.gameObject.SetActive(true);
                roleOnlyButton = contingencyPlannerSkillButton;
            }
            else if (me.getRoleKind() == RoleKind.Archivist)
            {
                archivistSkillButton.gameObject.SetActive(true);
                roleOnlyButton = archivistSkillButton;
            }
            else if (me.getRoleKind() == RoleKind.FieldOperative)
            {
                epidemiologistSkillButton.gameObject.SetActive(true);
                roleOnlyButton = epidemiologistSkillButton;
            }
            else if (me.getRoleKind() == RoleKind.Epidemiologist)
            {
                fieldOperativeSkillButton.gameObject.SetActive(true);
                roleOnlyButton = epidemiologistSkillButton;
            }
            else if (me.getRoleKind() == RoleKind.Dispatcher)
            {
                dispatcherSkillButton.gameObject.SetActive(true);
                roleOnlyButton = dispatcherSkillButton;
            }
            if ((currentPlayer == me) && (game.getCurrentPhase() == GamePhase.PlayerTakeTurn))
            {
                currentCity = me.getPlayerPawn().getCity();
                if (me.getRemainingAction() != 0)
                {
                    moveButton.GetComponent<Button>().interactable = true;
                    if (!currentCity.getHasResearch() && (currentPlayer.containsSpecificCityCard(currentCity) || (currentPlayer.getRoleKind() == RoleKind.OperationsExpert)))
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
                    else if (game.containsSpecificCityCardInDiscardPile(currentCity) && (!me.getOncePerTurnAction()))
                    {
                        archivistSkillButton.GetComponent<Button>().interactable = true;
                    }
                    else if (currentCity.hasCubes())
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
