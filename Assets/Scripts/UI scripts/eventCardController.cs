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
    }

    public void doReExamineResearch() {
        foreach (Transform t in playerDiscard.transform.GetChild(0).GetChild(0)) {
            if (t.gameObject.GetComponent<Button>() == null) {
                t.gameObject.AddComponent<Button>();
                t.GetComponent<Button>().interactable = true;
                Button b = t.GetComponent<Button>();
                //Debug.log("add listener");
                b.onClick.AddListener(reExaminedResearchSelectCard);
            }
        }

        playerDiscard.eventCardTime = true;
        playerDiscard.gameObject.SetActive(true);
    }
    public void reExaminedResearchSelectCard() {
        //game.ResilientPopulation (EventSystem.current.currentSelectedGameObject.name);
        playerDiscard.eventCardTime = false;
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
    }
    //todo response 


    string newAssignmentName;
    public void newAssignmentCardSelect() {
        newAssignmentName = EventSystem.current.currentSelectedGameObject.name;
    }
    public void newAssignmentClickYes() {
        //game.NewAssignment (newAssignmentName);

        Transform t = newAssignmentPanel.transform.GetChild(0);
        foreach (Transform d in t) {
            d.gameObject.SetActive(false);
        }
        newAssignmentPanel.SetActive(false);
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
    //---------------------------------MobileHospital zone------------------------------
    public void mobileHospital()
    {
        currentPlayer = game.getCurrentPlayer();
        game.MobileHospital(currentPlayer.getRoleKind().ToString());
    }
    //---------------------------------airLift zone-----------------------------
    public void airLift()
    {
        //TO-DO maybe later
    }
    //---------------------------------Government Grant zone-----------------------------
    public City cityToBuild = null;
    public void governmentGrant()
    {
        foreach(City city in game.getCities()){
            UnityEngine.Events.UnityAction call = ()=>selectCity(city);
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
    }

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
