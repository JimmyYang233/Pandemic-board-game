﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildOperation : MonoBehaviour {
	Game g;
	Player currentPlayer;
	City currentCity;
	Button build;
	// Use this for initialization
	void Start () {
		//g =GameObject.FindGameObjectWithTag ("GameController").GetComponent<Game>();
		g = Game.Instance;
		currentPlayer = g.getCurrentPlayer();
		currentCity = currentPlayer.getRole ().getPawn ().getCity ();
		build = this.GetComponent<Button> ();
	}
	
	public void buildReseachCenter(){
		//Game.build ();
	}
}
