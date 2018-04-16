using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpidemiologistOperation : MonoBehaviour {
	public GameObject epidemiologistOnlyPanel;
	public Game game;
	public playerSelectionPanel playerSelect;
	public GameObject playerPanel;
	public otherPlayerCardSelection cardSelection;
	public GameObject waitingForResponse;
	public GameObject basicOperation;
	string characterName;
	string cardName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void useSkill(){
		playerPanel.SetActive (false);
		playerSelect.gameObject.SetActive (true);
		playerSelect.displayPlayerNecessary ();
		playerSelect.selectStatus = playerSelectionPanel.Status.EPIDEMIOLOGIST;
	}	
	public void characterSelect(string name){
		basicOperation.SetActive (false);
		epidemiologistOnlyPanel.SetActive (true);
		playerSelect.gameObject.SetActive (false);
		playerSelect.setShareStatus ();

		//to card selection
		cardSelection.gameObject.SetActive (true);
		cardSelection.setEpidemiologistStatus();
		cardSelection.loadOtherPlayerCard (name);
		characterName = name;
	}

	public void takeCard(string name){
		cardName = name;
	}
	public void takeClick(){
		//game.share (characterName, cardName);
		game.EpidemiologistShare(cardName);
		cardSelection.clear ();
	}


}
