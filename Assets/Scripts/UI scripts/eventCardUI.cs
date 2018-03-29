using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eventCardUI : MonoBehaviour {
	GameObject eventCardPanel;
	// Use this for initialization
	void Start () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (click);
	}

	// Update is called once per frame
	void Update () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (click);
	}
	public void setEventCardPanel(GameObject t){
		eventCardPanel = t;
	}

	public void click(){
		eventCardPanel.SetActive (true);
		eventCardPanel.transform.GetChild (0).GetComponent<Text> ().text = "Use eventCard "+this.transform.GetChild (0).GetComponent<Text> ().text+"?";
	}

}
