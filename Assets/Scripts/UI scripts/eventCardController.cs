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
    public playerSelectionPanel playerSelect;
	public agreePanelController agreeController;
	public GameObject informResultPanel;

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
    }

	public void informResult(bool response){

		//rpc call this method to show result.
		informResultPanel.SetActive (true);
		if (response) {
			informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "He accept the eventCard";
		}
		else{
			informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "He reject the EventCard";
		}
	}
	public void showAgreePanel(string request){
		
	}
	public void rejectTheRequest(){
		//eg rpc call game.RejectInvitation()
	}
    private void borrowedTime() {
        currentPlayer = game.getCurrentPlayer();
        game.BorrowedTime();
    }
    private void oneQuietNight() {
        game.OneQuietNight();
    }
    //--------------------------------for reExamination--------------------------------
    string selectRERPlayer;
    public void reExaminedResearch() {
        playerSelect.gameObject.SetActive(true);
        playerSelect.setReExaminedResearch();
        playerSelect.displayAllPlayerForEventCard();
    }
    public void selectReExaminedResearchPlayer(string n) {
        selectRERPlayer = n;
        playerSelect.gameObject.SetActive(false);
		//need rpc here ask player eg. game.NewAssignmentAskPermission(selectRERPlayer);
    }
	public void showAgreePanelForReExaminedResearch(){
		//rpc call this method to show agree panel
		agreeController.status = agreePanelController.Status.REEXAMINEDRESEARCH;
		agreeController.agreePanel.gameObject.SetActive (true);
		agreeController.agreePanel.transform.GetChild(0).GetComponent<Text>().text="Do you want to accept reExaminedResearch?";

	}
    public void doReExamineResearch() {
        foreach (Transform t in playerDiscard.transform.GetChild(0).GetChild(0)) {
            if (t.gameObject.GetComponent<Button>() == null) {
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
		//game.ReExamined(currentPlayer,cardSelect)

    }
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
    }
    string selectNAPlayer;
    public void NewAssignment() {
        playerSelect.gameObject.SetActive(true);
        playerSelect.setNewAssignmentStatus();
        playerSelect.displayAllPlayerForEventCard();

    }
    public void selectNewAssignmentPlayer(string n) {
        selectNAPlayer = n;
        playerSelect.gameObject.SetActive(false);
		//rpc Example: game.newAssignmentAskPermission(selectNAPlayer);
    }

	public void showAgreePanelForNewAssignment(){
		//rpc call here
		agreeController.status = agreePanelController.Status.NEWASSIGNMENT;
		agreeController.agreePanel.gameObject.SetActive (true);
		agreeController.agreePanel.transform.GetChild(0).GetComponent<Text>().text="Do you want to accept New Assignment?";

	}

    string newAssignmentName;
    public void newAssignmentCardSelect() {
        newAssignmentName = EventSystem.current.currentSelectedGameObject.name;
    }
    public void newAssignmentClickYes() {
       
        Transform t = newAssignmentPanel.transform.GetChild(0);
        foreach (Transform d in t) {
            d.gameObject.SetActive(false);
        }
        newAssignmentPanel.SetActive(false);
		//game.NewAssignment (currentPlayer,newAssignmentName);
    }
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
    //------------------------------ForeCast-----------------------------------
    public void Forecast() {
        List<string> infectionCards = game.getInfectionDeckString();
        if (infectionCards.Count >= 6) {
            for (int i = 0; i < 6; i++) {

                Transform target = forecastPanel.transform.GetChild(i);
                target.gameObject.SetActive(true);
                target.name = infectionCards[i];
                target.GetComponent<Image>().color = game.findCity(infectionCards[i]).getColor();
                target.GetChild(0).GetComponent<Text>().text = infectionCards[i];
                target.GetComponent<Button>().interactable = true;
            }

            for (int i = 6; i < 12; i++) {
                Transform target = forecastPanel.transform.GetChild(i);
                target.gameObject.SetActive(true);
                target.GetComponent<Image>().color = Color.gray;
                target.GetChild(0).GetComponent<Text>().text = (i - 5).ToString();
                target.GetComponent<Button>().interactable = true;
            }
        }
        forecastPanel.SetActive(true);

    }
    string foreCastName;
    public void forecastSelectCard() {
        foreCastName = EventSystem.current.currentSelectedGameObject.name;
    }
    public void foreCastOrderSelect() {
        int order = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        foreach (Transform t in forecastPanel.transform) {
            if (t.name.Equals(foreCastName)) {
                t.gameObject.SetActive(false);
            }
        }
        Transform target = forecastPanel.transform.GetChild(5 + order);
        target.GetComponent<Image>().color = game.findCity(foreCastName).getColor();
        target.GetChild(0).GetComponent<Text>().text = foreCastName;
        target.GetComponent<Button>().interactable = false;
    }
    public void forecastYes() {
        List<string> str = new List<string>();
        for (int i = 6; i < 12; i++) {
            Transform target = forecastPanel.transform.GetChild(i);
            str.Add(target.GetChild(0).GetComponent<Text>().text);
        }
        foreach (string t in str) {
            Debug.Log(t);
        }
        game.Forecast(str);
        forecastPanel.SetActive(false);
    }
    #region Mobile Hospital
    //---------------------------------MobileHospital zone------------------------------
    public void mobileHospital()
    {
        currentPlayer = game.getCurrentPlayer();
        game.MobileHospital(currentPlayer.getRoleKind().ToString());
    }
    #endregion
    #region Airlift
    //---------------------------------airLift zone-----------------------------
    public void airLift()
    {
        //TO-DO maybe later
    }
    #endregion 
    #region Government Grant
    //---------------------------------Government Grant zone-----------------------------
    public City cityToBuild = null;
    public void governmentGrant()
    {
        foreach(City city in game.getCities()){
            UnityEngine.Events.UnityAction call = ()=>selectCity(city);
            city.displayButton();
            city.GetComponent<Button>().onClick.AddListener(call);
            city.GetComponent<Button>().onClick.AddListener(() => city.GetComponent<Button>().onClick.RemoveListener(call));
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
                foreach (Transform child in city.transform)
                {
                    if (child.tag == "researchStation")
                    {
                        Debug.Log("addListener in button");
                        Button button = child.gameObject.GetComponent<Button>();
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(delegate { game.GovernmentGrant(initialCity.getCityName().ToString(), currentCity.getCityName().ToString()); });
                    }
                }
            }
        }
        else
        {
            game.GovernmentGrant(String.Empty, currentCity.getCityName().ToString());
        }

        foreach(City aCity in game.getCities())
        {
            city.undisplayButton();
        }
    }
    #endregion
    #region Remote Treatement
    //---------------------------------Remote Treatement zone-----------------------------
    List<Transform> cubes = new List<Transform>();
    public void remoteTreatment()
    {
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
                        child.gameObject.AddComponent<Button>().onClick.AddListener(() => selectRemoteCube(city, color));
                        child.gameObject.GetComponent<Button>().onClick.AddListener(() => Destroy(child.gameObject.GetComponent<Button>()));
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
            //game.RemoteTreatment(cities[0], cities[1], colors[0], colors[1]); //TO-DO wait RPC done. 
            foreach(Transform child in cubes){
                Destroy(child.gameObject.GetComponent<Button>());
            }
            remoteCount = 0;
        }
        else
        {
            remoteCount++;
            //TO-DO maybe display something?
        }
    }
    #endregion
    #region Commercial Travel Ban
    //---------------------------------Commercial Travel Ban zone-----------------------------
    public void commercialTravelBan()
    {
        game.CommercialTravelBan(currentPlayer);
    }
    #endregion
    //---------------------------
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
