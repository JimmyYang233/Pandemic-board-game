using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpidemiologistOperation : MonoBehaviour {
	public GameObject epidemiologistOnlyPanel;
	public Game game;
	public playerSelectionPanel playerSelect;
	public otherPlayerCardSelection cardSelection;
	string characterName;
	string cardName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void useSkill(){
		playerSelect.displayPlayerNecessary ();
	}	


	public void characterSelect(string name){
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
		game.share (characterName, cardName);
		cardSelection.clear ();
	}


}
