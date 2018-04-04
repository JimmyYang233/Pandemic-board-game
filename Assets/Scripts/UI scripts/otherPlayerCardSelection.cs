using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class otherPlayerCardSelection : MonoBehaviour {
	public Game game;
	private City currentCity;
	private enum Status {RESEARCHER,OTHER};
	public ShareOperation share;
	private Status selectStatus = Status.RESEARCHER; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void loadOtherPlayerCard(string name){
		Player player = game.findPlayer (name);
		List<PlayerCard> cards = player.getHand ();
		int i = 0;
		foreach (PlayerCard p in cards) {
			if (p.getType () == CardType.CityCard) {
				transform.GetChild (i).gameObject.SetActive (true);
				transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = p.getName ();
				transform.GetChild (i).GetComponent<Image> ().color = ((CityCard)p).getColor ();
				transform.GetChild (i).name = p.getName ();
				i++;
			}

		}

	}
	//for test use only
	public void loadOtherPlayerCard(){
		loadOtherPlayerCard("ContingencyPlanner");
	}

	public void cardSelect(){
		if (selectStatus == Status.RESEARCHER) {
			share.takeFromResearcher (EventSystem.current.currentSelectedGameObject.name);
			clear ();
		}
	}
	public void clear(){
		foreach (Transform t in this.transform) {
			t.gameObject.SetActive (false);
		}
	}
}
