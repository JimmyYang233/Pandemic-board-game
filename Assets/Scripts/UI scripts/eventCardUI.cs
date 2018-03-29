using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eventCardUI : MonoBehaviour {
	GameObject eventCardPanel;
	Game game;
	// Use this for initialization
	void Start () {
		game= GameObject.FindGameObjectWithTag ("GameController").GetComponent<Game>()	;
	}

	// Update is called once per frame
	void Update () {
		if (this.transform.GetChild (0).GetComponent<Text> ().text == "MobileHospital") {
			
			Player p = game.getCurrentPlayer ();
			if (p != game.findEventCardHolder (EventKind.MobileHospital)) {
				this.GetComponent<Button> ().interactable = true;
			} else {
				this.GetComponent<Button> ().interactable = false;
			}

		} else {
			this.GetComponent<Button> ().interactable = true;
		}
	}
	public void setEventCardPanel(GameObject t){
		eventCardPanel = t;
	}

	public void click(){
		eventCardPanel.SetActive (true);
		eventCardPanel.transform.GetChild (1).GetComponent<Text> ().text = this.transform.GetChild (0).GetComponent<Text> ().text;

	}

}
