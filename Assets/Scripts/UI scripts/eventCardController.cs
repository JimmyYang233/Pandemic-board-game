using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class eventCardController : MonoBehaviour {
    string eventCardName;
    public Game game;
    City currentCity;
    public GameObject informEvent;
    Player currentPlayer;
	public GameObject otherPlayers;
    public playerSelectionPanel playerSelect;
	public agreePanelController agreeController;
	public MoveOperation move;
	public GameObject informResultPanel;
	public enum Status {REEXAMINEDRESEARCH,NEWASSIGNMENT,AIRLIFT,SPECIALORDERS};
	public Status status=Status.NEWASSIGNMENT;
	public string requestSource;
    public Button deploymentButton;
    Player me;

    //Resilient zone
    public GameObject infectionDiscardPile;
    //ForeCastPanel
    public GameObject forecastPanel;
    //newAssignment
    public GameObject newAssignmentPanel;
    //for reExamineResearch
    public playerDiscardPileUI playerDiscard;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
    }
	//---------------------------------REQUEST HANDLE-------------------------
	public void informResult(bool response){

		//rpc call this method to show result.
		informResultPanel.SetActive (true);
		if (response) {
			informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "He accept the eventCard";
			if (status == Status.NEWASSIGNMENT) {
				game.DisplayNewAssignment (selectNAPlayer);
			} else if (status == Status.REEXAMINEDRESEARCH) {
				game.DisplayReExaminedResearch (selectRERPlayer);
			} else if (status == Status.AIRLIFT) {
				chooseDirection ((selectALPlayer));
			}
		}
		else{
			informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "He reject the EventCard";
		}
	}
	public void showAgreePanel(string request,string source){
		requestSource = source;
		if (this.status == Status.NEWASSIGNMENT) {
			agreeController.status = agreePanelController.Status.NEWASSIGNMENT;
		} else if (this.status == Status.REEXAMINEDRESEARCH) {
			agreeController.status = agreePanelController.Status.REEXAMINEDRESEARCH;
		} else if (this.status == Status.AIRLIFT) {
			agreeController.status = agreePanelController.Status.AIRLIFT;
		}
		agreeController.agreePanel.gameObject.SetActive (true);
		agreeController.agreePanel.transform.GetChild (0).GetComponent<Text> ().text = request;
	}
	public void rejectTheRequest(){
		agreeController.agreePanel.gameObject.SetActive (false);
		game.InformEventCardPermissionResult (false,requestSource);
	}
	public void acceptTheRequest(){
		agreeController.agreePanel.gameObject.SetActive (false);
		game.InformEventCardPermissionResult (true,requestSource);
	}
    #region borrowed Time
    //-----------------------------small event cards------------------------------------
    private void borrowedTime() {
        currentPlayer = game.getCurrentPlayer();
        game.BorrowedTime();
    }
    public void oneQuietNight() {
        game.OneQuietNight();
    }
    #endregion
    #region reExaminate search
    //--------------------------------for reExamination--------------------------------
    string selectRERPlayer;
    public void reExaminedResearch() {
		this.status = Status.REEXAMINEDRESEARCH;
		otherPlayers.SetActive (false);
        playerSelect.gameObject.SetActive(true);
        playerSelect.setReExaminedResearch();
        playerSelect.displayAllPlayerForEventCard();
    }
    public void selectReExaminedResearchPlayer(string n) {
        selectRERPlayer = n;
        playerSelect.gameObject.SetActive(false);
		otherPlayers.SetActive (true);
		if (n.Equals (game.FindPlayer (PhotonNetwork.player).getRoleKind ().ToString ())) {
			doReExamineResearch ();
		} else {
			game.AskForEventCardPermission ("Do you want to accept event card reExainedResearch?", selectRERPlayer, game.FindPlayer (PhotonNetwork.player).getRoleKind ().ToString ());
		}
    }
    public void doReExamineResearch() {
        foreach (Transform t in playerDiscard.transform.GetChild(0).GetChild(0)) {
			Debug.Log ("Here");
			if (t.gameObject.GetComponent<Button>() == null && game.findCity(t.GetChild(0).GetComponent<Text>().text)!=null) {
				Debug.Log ("I am here");
                t.gameObject.AddComponent<Button>();
                t.GetComponent<Button>().interactable = true;
                Button b = t.GetComponent<Button>();
                b.onClick.AddListener(reExaminedResearchSelectCard);
            }
        }

        playerDiscard.eventCardTime = true;
        playerDiscard.gameObject.SetActive(true);
    }
    public void reExaminedResearchSelectCard() {
        string cardSelect=EventSystem.current.currentSelectedGameObject.name;
        playerDiscard.eventCardTime = false;
		game.ReExaminedResearch (game.FindPlayer(PhotonNetwork.player).getRoleKind().ToString(), cardSelect);

    }
#endregion
    #region New Assignment
    //---------------------------------NewAssignment zone------------------------------
    public void doNewAssignment() {
        List<string> roles = game.getUnusedRole();
        newAssignmentPanel.SetActive(true);
        Transform t = newAssignmentPanel.transform.GetChild(0);
        for (int i = 0; i < roles.Count; i++) {
            t.GetChild(i).gameObject.SetActive(true);
            t.GetChild(i).GetComponent<Image>().color = Maps.getInstance().getRoleColor(game.findRoleKind(roles[i]));
            t.GetChild(i).gameObject.name = roles[i];
            t.GetChild(i).GetChild(0).GetComponent<Text>().text = roles[i];
        }
		for (int i = roles.Count; i < 13; i++) {
			t.GetChild(i).gameObject.SetActive(false);

		}
    }
    string selectNAPlayer;
    public void NewAssignment() {
		this.status = Status.NEWASSIGNMENT;
		otherPlayers.SetActive (false);
        playerSelect.gameObject.SetActive(true);
        playerSelect.setNewAssignmentStatus();
        playerSelect.displayAllPlayerForEventCard();

    }
    public void selectNewAssignmentPlayer(string n) {
        selectNAPlayer = n;
        playerSelect.gameObject.SetActive(false);
		otherPlayers.SetActive (true);
		if (n.Equals (game.FindPlayer (PhotonNetwork.player).getRoleKind ().ToString ())) {
			doNewAssignment ();
		} else {
			game.AskForEventCardPermission ("Do you want to accept event card newAssignment?", selectNAPlayer, game.FindPlayer (PhotonNetwork.player).getRoleKind ().ToString ());
		}
    }

    string newAssignmentName;
    public void newAssignmentCardSelect() {
        newAssignmentName = EventSystem.current.currentSelectedGameObject.name;
		Debug.Log (newAssignmentName);
		Transform t = newAssignmentPanel.transform.GetChild(0);
		foreach (Transform d in t) {
			Debug.Log (d.name);
			if (!d.name.Equals (newAssignmentName)) {
				d.gameObject.SetActive (false);
			}
		}
    }
		
    public void newAssignmentClickYes() {
       
        Transform t = newAssignmentPanel.transform.GetChild(0);
        foreach (Transform d in t) {
            d.gameObject.SetActive(false);
        }
        newAssignmentPanel.SetActive(false);
		game.NewAssignment (game.FindPlayer(PhotonNetwork.player).getRoleKind().ToString(),newAssignmentName);
    }
#endregion
    #region Resilient Population
    //---------------------------------Resilient zone-----------------------------------
    public void ResilientPopulation() {
        foreach (Transform t in infectionDiscardPile.transform.GetChild(0).GetChild(0)) {
            if (t.gameObject.GetComponent<Button>() == null) {
                t.gameObject.AddComponent<Button>();
                t.GetComponent<Button>().interactable = true;
                Button b = t.GetComponent<Button>();
                //Debug.log("add listener");
                b.onClick.AddListener(resilientSelectCard);
            }
        }
        infectionDiscardPile.GetComponent<infectionDiscardPileUI>().eventCardTime = true;
        infectionDiscardPile.SetActive(true);
    }
    public void resilientSelectCard() {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        game.ResilientPopulation(EventSystem.current.currentSelectedGameObject.name);
        infectionDiscardPile.GetComponent<infectionDiscardPileUI>().eventCardTime = false;
    }
    #endregion
    #region Forecast
    //------------------------------ForeCast-----------------------------------
    public void Forecast() {
        List<string> infectionCards = game.getInfectionDeckString();
		if (infectionCards.Count >= 6) {
			for (int i = 0; i < 6; i++) {
				string tstr = infectionCards [i];
				Transform target = forecastPanel.transform.GetChild (i);
				target.gameObject.SetActive (true);
				target.name = tstr;
				target.GetComponent<Image> ().color = game.findCity (tstr).getColor ();
				target.GetChild (0).GetComponent<Text> ().text = tstr;
				target.GetComponent<Button> ().interactable = true;
			}

			for (int i = 6; i < 12; i++) {
				Transform target = forecastPanel.transform.GetChild (i);
				target.gameObject.SetActive (true);
				target.GetComponent<Image> ().color = Color.gray;
				target.GetChild (0).GetComponent<Text> ().text = (i - 5).ToString ();
				target.GetComponent<Button> ().interactable = true;
			}
		} else {
			for (int i = 0; i < infectionCards.Count; i++) {
				string tstr = infectionCards [i];
				Transform target = forecastPanel.transform.GetChild (i);
				target.gameObject.SetActive (true);
				target.name = tstr;
				target.GetComponent<Image> ().color = game.findCity (tstr).getColor ();
				target.GetChild (0).GetComponent<Text> ().text = tstr;
				target.GetComponent<Button> ().interactable = true;
			}
			for (int i = infectionCards.Count; i <6; i++) {
				Transform target = forecastPanel.transform.GetChild (i);
				target.gameObject.SetActive (false);
			}

			for (int i = 6+infectionCards.Count; i < 12; i++) {
				Transform target = forecastPanel.transform.GetChild (i);
				target.gameObject.SetActive (false);
			}
		}
        forecastPanel.SetActive(true);

    }
    string foreCastName="";
    public void forecastSelectCard() {
        foreCastName = EventSystem.current.currentSelectedGameObject.name;
    }
    public void foreCastOrderSelect() {
		if (foreCastName != "") {
			int order = int.Parse (EventSystem.current.currentSelectedGameObject.name);
			foreach (Transform t in forecastPanel.transform) {
				if (t.name.Equals (foreCastName)) {
					t.gameObject.SetActive (false);
				}
			}
			Transform target = forecastPanel.transform.GetChild (5 + order);
			target.GetComponent<Image> ().color = game.findCity (foreCastName).getColor ();
			target.GetChild (0).GetComponent<Text> ().text = foreCastName;
			target.GetComponent<Button> ().interactable = false;
			foreCastName = "";
		}
    }
    public void forecastYes() {
        List<string> str = new List<string>();
        for (int i = 6; i < 12; i++) {
            Transform target = forecastPanel.transform.GetChild(i);
			if (target.gameObject.activeSelf) {
				str.Add (target.GetChild (0).GetComponent<Text> ().text);
			}
        }
        foreach (string t in str) {
            Debug.Log(t);
        }
        game.Forecast(str);
        forecastPanel.SetActive(false);
    }
    #endregion
    #region Mobile Hospital
    //---------------------------------MobileHospital zone------------------------------
    public void mobileHospital()
    {
        me = game.FindPlayer(PhotonNetwork.player);
        Debug.Log(me.getRoleKind().ToString());
        game.MobileHospital(me.getRoleKind().ToString());
    }
    #endregion
    #region Special Orders
    //---------------------------------Special Orders------------------------------------

    public void SpecialOrders(){
		move.movePanel.gameObject.SetActive (true);
		move.basicOperationPanel.SetActive (false);
		move.forDispatcherShow ();
	}
	#endregion
    #region Airlift
    //---------------------------------airLift zone----------------------------------------
    List<UnityEngine.Events.UnityAction> airLiftCalls = new List<UnityEngine.Events.UnityAction>();
    public void airLift()
    {
		this.status = Status.AIRLIFT;
		otherPlayers.SetActive (false);
		playerSelect.gameObject.SetActive(true);
		playerSelect.selectStatus=playerSelectionPanel.Status.AIRLIFT;
		playerSelect.displayAllPlayerForEventCard();
    }
	string selectALPlayer;
	public void selectAirLiftPlayer(string n) {
		selectALPlayer = n;
		playerSelect.gameObject.SetActive (false);
		otherPlayers.SetActive (true);
		if (n.Equals (game.FindPlayer (PhotonNetwork.player).getRoleKind ().ToString ())) {
			chooseDirection (selectALPlayer);
			Debug.Log ("myself");
		} else {
			game.AskForEventCardPermission ("Do you want to accept event card airLift?", selectALPlayer, game.FindPlayer (PhotonNetwork.player).getRoleKind ().ToString ());
		}
	}

    public void chooseDirection(string roleKind)
    {
		Debug.Log ("choose direction");
        Player player = game.findPlayer(roleKind);
        City currentCity = player.getPlayerPawn().getCity();
        List<Player> players = game.getPlayers();
        foreach(City city in game.getCities())
        {
            UnityEngine.Events.UnityAction thisCall = () => movePawn(player, city);
            city.gameObject.GetComponent<Button>().onClick.AddListener(thisCall);
            airLiftCalls.Add(thisCall);
            if (currentCity!=city)
            {
                
                city.displayButton();
                
            }
        }
    }

    public void movePawn(Player player, City city)
    {
        game.Airlift(player.getRoleKind().ToString(), city.getCityName().ToString());
        List<City> allCities = game.getCities();
        for(int i = 0; i<allCities.Count; i++)
        {
            allCities[i].gameObject.GetComponent<Button>().onClick.RemoveListener(airLiftCalls[i]);
            allCities[i].undisplayButton();
        }
    }
    #endregion 
    #region Government Grant
    //---------------------------------Government Grant zone-----------------------------
    private City cityToBuild = null;
    List<UnityEngine.Events.UnityAction> calls = new List<UnityEngine.Events.UnityAction>();
    public void governmentGrant()
    {
        List<City> cities = game.getCities();
        for (int i = 0; i <cities.Count; i++){
            City city = cities[i];
            UnityEngine.Events.UnityAction call = () => selectCity(city);
            calls.Add(call);
            if (!city.getHasResearch())
            {
                city.displayButton();
                city.GetComponent<Button>().onClick.AddListener(call);
                
            }
        }
    }

    public void selectCity(City city)
    {
        List<City> citiesWithResearch = new List<City>();
        if (game.getRemainingResearch() == 0)
        {
            foreach (City initialCity in game.getCities())
            {
                if (city.getHasResearch())
                {
                    citiesWithResearch.Add(city);
                }
            }
            foreach (City initialCity in citiesWithResearch)
            {
                foreach (Transform child in initialCity.transform)
                {
                    if (child.tag == "researchStation")
                    {
                        Debug.Log("addListener in button");
                        Button button = child.gameObject.GetComponent<Button>();
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(delegate { game.GovernmentGrant(initialCity.getCityName().ToString(), city.getCityName().ToString()); });
                    }
                }
            }
        }
        else
        {
            game.GovernmentGrant(String.Empty, city.getCityName().ToString());
        }

        List<City> cities = game.getCities();
        for (int i = 0; i < cities.Count; i++)
        {
            City aCity = cities[i];
            aCity.undisplayButton();
            aCity.GetComponent<Button>().onClick.RemoveListener(calls[i]);
        }
    }
    #endregion
    #region Remote Treatement
    //---------------------------------Remote Treatement zone-----------------------------
    List<Transform> cubes = new List<Transform>();
    public void remoteTreatment()
    {
        Debug.Log("Remote Treatment clicked");
        foreach(City city in game.getCities())
        {
            if (city.hasCubes())
            {
                foreach (Transform child in city.transform)
                {
                    if (child.tag != "researchStation")
                    {
                        Color color = Color.blue;
                        if (child.tag == "blue Cube")
                        {
                            color = Color.blue;
                        }
                        else if (child.tag == "black Cube")
                        {
                            color = Color.black;
                        }
                        else if (child.tag == "red Cube")
                        {
                            color = Color.red;
                        }
                        else if (child.tag == "yellow Cube")
                        {
                            color = Color.yellow;
                        }
                        else if (child.tag == "purple Cube")
                        {
                            color = Color.magenta;
                        }
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerClick;
                        entry.callback.AddListener((eventData) => { selectRemoteCube(city, color); });
                        child.gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
                        cubes.Add(child);
                    }
                }
            }
        }
    }


    private City[] cities = new City[2];
    private Color[] colors = new Color[2];
    private int remoteCount = 0;
    public void selectRemoteCube(City city, Color color)
    {
        Debug.Log("Choose this city for remote treatment" + city.getCityName().ToString());
        cities[remoteCount] = city;
        colors[remoteCount] = color;
        if(remoteCount == 1)
        {
            game.RemoteTreatment(cities[0].getCityName().ToString(), cities[1].getCityName().ToString(), colors[0], colors[1]);
            foreach(Transform child in cubes){
                Destroy(child.gameObject.GetComponent<EventTrigger>());
            }
            remoteCount = 0;
            Debug.Log("two cubes selected");
        }
        else
        {
            remoteCount++;
            Debug.Log("one cube selected");
        }
    }
    #endregion
    #region Commercial Travel Ban
    //---------------------------------Commercial Travel Ban zone-----------------------------
    public void commercialTravelBan()
    {
        Debug.Log("Commercial travel ban clicked");
        me = game.FindPlayer(PhotonNetwork.player);
        game.CommercialTravelBan(me.getRoleKind().ToString());
    }
    #endregion
    #region RapidVaccineDeployment
    private Color deployColor;
    List<City> citiesToDeploye = new List<City>();
    List<City> cityWithCalls = new List<City>();
    Dictionary<City, int> cityCubeNumber = new Dictionary<City, int>();
    List<UnityEngine.Events.UnityAction> deployCalls = new List<UnityEngine.Events.UnityAction>();

    public void testRapidVaccine()
    {
        askForRapidVaccineDeployment(Color.blue);
    }

    public void askForRapidVaccineDeployment(Color aColor)
    {
        this.gameObject.SetActive(true);
        this.transform.GetChild(1).GetComponent<Text>().text = "RapidVaccineDeployment";
        deployColor = aColor;
    }

   
    public void rapidVaccineDeployment()
    {
        deploymentButton.gameObject.SetActive(true);
        List<City> allCities = game.getCities();
        foreach(City city in allCities)
        {
            if (city.hasCubesOfSpecificColor(deployColor))
            {
                city.displayButton();
                UnityEngine.Events.UnityAction call = () => deployCity(city);
                city.gameObject.GetComponent<Button>().onClick.AddListener(call);
                cityWithCalls.Add(city);
                cityCubeNumber.Add(city, city.getCubeNumber(deployColor));
                deployCalls.Add(call);
            }
        }
    }

    public void deployCity(City city)
    {
        citiesToDeploye.Add(city);
        int count = cityCubeNumber[city];
        cityCubeNumber.Remove(city);
        cityCubeNumber.Add(city, count - 1);
        foreach(City otherCity in game.getCities())
        {
            city.undisplayButton();
        }
        if (citiesToDeploye.Count >= 5)
        {
            for(int i= 0; i< cityWithCalls.Count; i++)
            {
                cityWithCalls[i].gameObject.GetComponent<Button>().onClick.RemoveListener(deployCalls[i]);
            }
        }
        else
        {
            if (cityCubeNumber[city]>0)
            {
                city.displayButton();
            }
            foreach(City otherCity in city.getNeighbors())
            {
                if (otherCity.hasCubesOfSpecificColor(deployColor))
                {
                    if (cityCubeNumber[otherCity] > 0)
                    {
                        otherCity.displayButton();
                    }
                }
            }
        }
    }

    public void deployVaccineButtonClicked()
    {
        Debug.Log("Calling functions rapidVaccineDeployment");
        foreach(City city in citiesToDeploye)
        {
            Debug.Log(city.getCityName().ToString() + " deployed");
        }
        deploymentButton.gameObject.SetActive(false);
        game.RapidVaccineDeployment(deployColor, citiesToDeploye);
        for (int i = 0; i < cityWithCalls.Count; i++)
        {
            cityWithCalls[i].undisplayButton();
            cityWithCalls[i].gameObject.GetComponent<Button>().onClick.RemoveListener(deployCalls[i]);
        }
        cityWithCalls.Clear();
        cityCubeNumber.Clear();
        citiesToDeploye.Clear();
    }
    #endregion
    //--------------------------
    public void useEvent(){
		eventCardName = this.transform.GetChild (1).GetComponent<Text> ().text;
		Debug.Log (eventCardName);
		switch (eventCardName)
		{
		    case "BorrowedTime":
			    borrowedTime ();
			    break;
		    case "ResilientPopulation":
			    ResilientPopulation ();
			    break;
		    case "Forecast":
			    Forecast();
			    break;
		    case "NewAssignment":
			    NewAssignment();
			    break;
		    case "OneQuietNight":
			    oneQuietNight();
			    break;
		    case "ReExaminedResearch":
			    reExaminedResearch();
			    break;
            case "MobileHospital":
                mobileHospital();
                break;
            case "RemoteTreatment":
                remoteTreatment();
                break;
            case "Airlift":
                airLift();
                break;
            case "GovernmentGrant":
                governmentGrant();
                break;
            case "CommercialTravelBan":
                commercialTravelBan();
                break;
            case "RapidVaccineDeployment":
                rapidVaccineDeployment();
                break;
			case "SpecialOrders":
				SpecialOrders();
				break;
		    default:
			    break;
		}
		/*
		Airlift
		ResilientPopulation
		OneQuietNight
		Forecast
		GovernmentGrant
		CommercialTravelBan
		ReExaminedResearch
		RemoteTreatment
		BorrowedTime
		MobileHospital
		NewAssignmentRapidVaccineDeployment*/
	}


}
