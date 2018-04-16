using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveOperation : MonoBehaviour {
    public Button driveButton;
    public Button directFlightButton;
    public Button shuttleFlightButton;
    public Button charterFlightButton;
    public Button cancelButton;

    public Button bioDriveButton;
    public Button bioDirectFlightButton;
    public Button bioCharterFlightButton;
    public Button bioCancelButton;

    public playerSelectionPanel playerSelect;
	public PlayerPanelController ppc;
	public agreePanelController agreeController;
	public GameObject result;
    public GameObject playerCardPanel;
	public GameObject movePanel;
	public GameObject basicOperationPanel;
	public GameObject waitingForResponse;
    Game game;

    Player currentPlayer;
    Player playerToMove;

    private enum Status {DRIVE, DIRECTFLIGHT, SHUTTLEFLIGHT, CHARTERFLIGHT, NULL, DISPATCHER, DISPATCHERPAWN, BIODIRECTFLIGHT, BIOCHATTERFLIGHT};

    private Status moveStatus = Status.NULL; 

    void Start()
    {
        //game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
		game = Game.Instance;
    }
	//---------------------------------Request Handle-----------------------------------//
	public void askPermission(string request){
		agreeController.agreePanel.transform.GetChild (0).GetComponent<Text> ().text = request;
		agreeController.status = agreePanelController.Status.DISPATCHER;
		agreeController.agreePanel.gameObject.SetActive (true);
	}
	string targetPlayer;
	public void roleSelectForMove(string target){
		targetPlayer = target;
		this.moveStatus = Status.DISPATCHER;
		currentPlayer = game.getCurrentPlayer ();
		if (target.Equals (currentPlayer.getRoleKind ().ToString ())) {
			changePlayerToMove (target);
			showMove ();
			setActivePpc ();

			playerSelect.selectStatus = playerSelectionPanel.Status.SHARE;
			ppc.gameObject.SetActive (true);
			playerSelect.clear();
			playerSelect.gameObject.SetActive(false);
		} else {
			waitingForResponse.SetActive (true);
			game.AskPermissionDispatcher(target,currentPlayer.getRoleKind().ToString()+" wants to move instead of you.");
		}
	}
	public void roleSelectForPawn(string target){
		targetPlayer = target;
		this.moveStatus = Status.DISPATCHERPAWN;
		currentPlayer = game.getCurrentPlayer ();
		if (target.Equals (currentPlayer.getRoleKind ().ToString ())) {
			changePlayerToMove (target);
			selectCityWithPawn ();
			setActivePpc ();

			playerSelect.selectStatus = playerSelectionPanel.Status.SHARE;
			ppc.gameObject.SetActive (true);
			playerSelect.clear();
			playerSelect.gameObject.SetActive(false);

		} else {
			waitingForResponse.SetActive (true);
			game.AskPermissionDispatcher(target,"Dispatcher wants to move your pawn to another city with another pawn.");

		}

	}

	public void informResult(bool t){
		waitingForResponse.SetActive (false);
		if (t) {
			result.SetActive (true);
			result.transform.GetChild (0).GetComponent<Text> ().text = "He accepts!";
			if (this.moveStatus == Status.DISPATCHER) {
				changePlayerToMove (targetPlayer);
				showMove ();
				setActivePpc ();
			} else if (this.moveStatus == Status.DISPATCHERPAWN) {
				changePlayerToMove (targetPlayer);
				selectCityWithPawn ();
				setActivePpc ();
			}
			playerSelect.selectStatus = playerSelectionPanel.Status.SHARE;
			ppc.gameObject.SetActive (true);
			playerSelect.clear();
			playerSelect.gameObject.SetActive(false);
		} else {
			result.SetActive (true);
			result.transform.GetChild(0).GetComponent<Text>().text="He rejects!";
			playerSelect.selectStatus = playerSelectionPanel.Status.SHARE;
			ppc.gameObject.SetActive (true);
			playerSelect.clear();
			playerSelect.gameObject.SetActive(false);
		}

	}

	public void acceptTheRequest(){
		game.InformDispatcher (true);
	}

	public void declineTheRequest(){
		game.InformDispatcher (false);
	}

	public void setActivePpc(){
		ppc.gameObject.SetActive (true);
	}
    public void moveButtonClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        playerToMove = currentPlayer;
        if (currentPlayer.getRoleKind() == RoleKind.Dispatcher)
        {
			forDispatcherShow ();
        }
        else
        {
            showMove();
        }
    }

	public void forDispatcherShow(){
		playerSelect.selectStatus = playerSelectionPanel.Status.DISPATCHER;
		ppc.gameObject.SetActive (false);
		playerSelect.displayAllPlayerForEventCard ();
		playerSelect.gameObject.SetActive(true);
	}
    public void showMove()
    {   
        driveButton.GetComponent<Button>().interactable = true;
        City currentCity = playerToMove.getPlayerPawn().getCity();

        if (currentPlayer.containsCityCard())
        {
            directFlightButton.GetComponent<Button>().interactable = true;
        }
        if (currentPlayer.containsSpecificCityCard(currentCity))
        {
            charterFlightButton.GetComponent<Button>().interactable = true;
        }
        if (currentCity.getHasResearch())
        {
            if (game.getRemainingResearch() < 5)
            {
                shuttleFlightButton.GetComponent<Button>().interactable = true;
            }
            else if (currentPlayer.getRoleKind() == RoleKind.OperationsExpert && currentPlayer.containsCityCard())
            {
                shuttleFlightButton.GetComponent<Button>().interactable = true;
            }

        }
        cancelButton.GetComponent<Button>().interactable = true;
    }

    public void cancelButtonClicked()
    {
        disableAllCities();
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = false;
    }
   // public City tmpCity;
    public void driveButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = playerToMove.getPlayerPawn().getCity();
        //Debug.Log(currentCity.getCityName());
        foreach (City neighbor in currentCity.getNeighbors())
        {
            //Debug.Log(neighbor.getCityName());
            neighbor.displayButton();
        }
        moveStatus = Status.DRIVE;
    }

    public void directFlightButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = playerToMove.getPlayerPawn().getCity();
        List<PlayerCard> cards = currentPlayer.getHand();
        foreach(PlayerCard card in cards)
        {
            if(card.getType() == CardType.CityCard)
            {
                CityCard aCard = (CityCard)card;
                City city = aCard.getCity();
                if (city != currentCity)
                {
                    city.displayButton();
                }
            }
            
        }

        moveStatus = Status.DIRECTFLIGHT;
    }

    public void charterFlightButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = playerToMove.getPlayerPawn().getCity();
        foreach (City city in game.getCities())
        {
            if (city != currentCity)
            {
                city.displayButton();
            }
        }

        moveStatus = Status.CHARTERFLIGHT;
    }

    public void shuttleFlightButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = playerToMove.getPlayerPawn().getCity();
        foreach (City city in game.getCities())
        {
            if (((city != currentCity)&&(city.getHasResearch())||(currentPlayer.getRoleKind() == RoleKind.OperationsExpert && currentPlayer.containsSpecificCityCard(city))))
            {
                city.displayButton();
            }
        }

        moveStatus = Status.SHUTTLEFLIGHT;
    }

    public static void disableAllCities()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("City");
        foreach (GameObject aObject in tmp)
        {
            Button button = aObject.GetComponent<Button>();
            if (button.interactable)
            {
                button.interactable = false;
            }
        }
    }
    

    public void cityButtonClicked(City destinationCity)
    {
        currentPlayer = game.getCurrentPlayer();
        if(moveStatus == Status.DRIVE)
        {
			game.Drive(playerToMove.getRoleKind().ToString(), destinationCity.cityName.ToString());
        }
        else if(moveStatus == Status.DIRECTFLIGHT)
        {
			game.TakeDirectFlight(playerToMove.getRoleKind().ToString(), currentPlayer.getCard(destinationCity).getCity().getCityName().ToString());
        }
        else if(moveStatus == Status.CHARTERFLIGHT)
        {
            game.TakeCharterFlight(playerToMove.getRoleKind().ToString(), destinationCity.cityName.ToString());
        }
        else if(moveStatus == Status.SHUTTLEFLIGHT)
        {
            game.TakeShuttleFlight(playerToMove.getRoleKind().ToString(), destinationCity.cityName.ToString());
        }
        else if(moveStatus == Status.BIODIRECTFLIGHT)
        {
            game.BioTerroristDirectFlight(destinationCity.cityName.ToString());
        }
        else if(moveStatus == Status.BIOCHATTERFLIGHT)
        {
            game.BioTerroristCharterFlight(playerToMove.getPlayerPawn().getCity().getCityName().ToString(), destinationCity.getCityName().ToString());
        }
        moveStatus = Status.NULL;
        disableAllCities();
    }

    public void changePlayerToMove(string rolekind)
    {
        playerToMove = game.findPlayer(rolekind);
        playerSelect.selectStatus = playerSelectionPanel.Status.SHARE;
    }

    public void dispatcherMove()
    {
		playerSelect.selectStatus = playerSelectionPanel.Status.DISPATCHERMOVEPAWN;
		ppc.gameObject.SetActive (false);
		playerSelect.displayAllPlayerForEventCard ();
		playerSelect.gameObject.SetActive(true);
    }

    public void selectCityWithPawn()
    {
        moveStatus = Status.DRIVE;
        foreach(Player player in game.getPlayers())
        {
            City city = player.getPlayerPawn().getCity();
            city.displayButton();
        }
    }


    List<UnityEngine.Events.UnityAction> operationExpertCalls = new List<UnityEngine.Events.UnityAction>();
    List<GameObject> children = new List<GameObject>();
    public void operationsExpertchooseCity()
    {
        Player currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        List<City> cities = game.getCities();
        for (int i = 0; i < cities.Count; i++)
        {
            City thisCity = cities[i];
            UnityEngine.Events.UnityAction thisCall = () => operationExpertChoosePlayerCard(thisCity);
            thisCity.gameObject.GetComponent<Button>().onClick.AddListener(thisCall);
            operationExpertCalls.Add(thisCall);
            if (thisCity != currentCity)
            {
                thisCity.gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void operationExpertChoosePlayerCard(City city)
    {
        List<City> cities = game.getCities();
        for (int i = 0; i < cities.Count; i++)
        {
            cities[i].gameObject.GetComponent<Button>().onClick.RemoveListener(operationExpertCalls[i]);
        }
        operationExpertCalls.Clear();
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            child.GetComponent<Button>().interactable = true;
            child.GetComponent<Button>().onClick.AddListener(() => operationsExpertChooseMove(city, name));
            child.GetComponent<Button>().onClick.AddListener(() => child.GetComponent<Button>().interactable = false);
        }
    }

    public void operationsExpertChooseMove(City destinationCity, string cardName)
    {
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            //Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            child.GetComponent<Button>().interactable = false;
            child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        Debug.Log("Operations Expert use " + cardName + " to move to " + destinationCity.getCityName().ToString());
        game.OperationsExpertMove(destinationCity.getCityName().ToString(), cardName);
    }
    //bio-Terrorist Operations
    public void bioMoveButtonClicked()
    {
        BioTerrorist bioterrorist = game.getBioTerrorist();
        currentPlayer = game.getCurrentPlayer();
        playerToMove = currentPlayer;
        Debug.Log(playerToMove.getRoleKind());
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        bioDriveButton.gameObject.GetComponent<Button>().interactable = true;
        if (currentPlayer.containsInfectionCard()&&(currentPlayer.getRemainingAction()>0))
        {
            bioDirectFlightButton.gameObject.GetComponent<Button>().interactable = true;
        }

        if (currentPlayer.containsSpecificInfectionCard(currentCity)&&(currentPlayer.getRemainingAction() > 0))
        {
            bioCharterFlightButton.gameObject.GetComponent<Button>().interactable = true;
        }
        bioCancelButton.gameObject.GetComponent<Button>().interactable = true;
    }

    public void bioDriveButtonClicked()
    {
        bioDriveButton.GetComponent<Button>().interactable = false;
        bioDirectFlightButton.GetComponent<Button>().interactable = false;
        bioCharterFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        foreach (City neighbor in currentCity.getNeighbors())
        {
            neighbor.displayButton();
        }
        moveStatus = Status.DRIVE;
    }

    public void bioDirectFlightButtonClicked()
    {
        bioDriveButton.GetComponent<Button>().interactable = false;
        bioDirectFlightButton.GetComponent<Button>().interactable = false;
        bioCharterFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        foreach (PlayerCard card in currentPlayer.getHand())
        {
            if (card.getType() == CardType.InfectionCard)
            {
                InfectionCard infectionCard = (InfectionCard)card;
                City infectionCity = infectionCard.getCity();
                if (infectionCity != currentCity)
                {
                    infectionCity.displayButton();
                }
            }
        }
        moveStatus = Status.BIODIRECTFLIGHT;
    }

    public void bioTerroristCharterFlightButtonClicked()
    {
        foreach(City city in game.getCities())
        {
            city.displayButton();
        }
        moveStatus = Status.BIOCHATTERFLIGHT;
    }

    public void bioCancelButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = false;
        foreach (City city in game.getCities())
        {
            city.undisplayButton();
        }
    }
}
