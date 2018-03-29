using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class saveController : MonoBehaviour {
	public GameObject input;
	public Game game;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void save(){
		Debug.Log (input.GetComponent<InputField> ().text);
		game.save (input.GetComponent<InputField>().text);
	}
}
