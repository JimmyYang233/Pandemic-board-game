using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eventCardController : MonoBehaviour {
	string name;
	public Game game;
	Player currentPlayer;
	City currentCity;
	public GameObject informEvent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
	}

	private void borrowedTime(){
		currentPlayer = game.getCurrentPlayer();
		game.BorrowedTime ();
	}
	public void useEvent(){
		name = this.transform.GetChild (1).GetComponent<Text> ().text;
		Debug.Log (name);
		switch (name)
		{
		case "BorrowedTime":
			borrowedTime ();
			break;
		case "":
			
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
