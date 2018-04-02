﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;


public static class SaveAndLoadManager{

	public static void SaveGameData(Game game, string name){
		BinaryFormatter bf = new BinaryFormatter ();
		string path = Path.Combine (Application.persistentDataPath, name + ".pandemic");
		if (File.Exists (path)) {
			File.Delete (path);
		}
		FileStream stream = new FileStream (path, FileMode.Create);

		GameData data = new GameData (game);

		bf.Serialize (stream, data);
		stream.Close ();
	}

	public static GameData LoadGameData(string name){
		string path = Path.Combine (Application.persistentDataPath, name + ".pandemic");
		if (File.Exists (path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream stream = new FileStream (path, FileMode.Open);

			GameData data = bf.Deserialize (stream) as GameData;
			//----------debug----------
			/*
			Debug.Log("Show detail of loaded game.....");
			foreach (PlayerCardList pcl in data.playerCardList) {
				foreach (string pc in pcl.playerHand) {
					Debug.Log ("Player card " + pc);
				}
			}

			foreach (RoleKind rk in data.roleKindList) {
				Debug.Log ("RoleKind is " + rk.ToString());
			}*/
			//-------------------------
			stream.Close ();
			return data;
		} else
			return null;
	}
}


/*
	this class will be used to save game data
	the player's role will be saved in a list and their 


*/
[Serializable]
public class GameData{
	//list of player's hand cards
	public List<PlayerCardList> playerCardList = new List<PlayerCardList>();
	public Challenge challenge;
	public List<RoleKind> roleKindList = new List<RoleKind>();
	public RoleKind currentPlayerRoleKind;
	public int infectionRateIndex;
	public int outBreakRate;
	public int remainingResearch;
	public int difficulity;
	public List<string> playerCardDeck = new List<string> ();
	public List<string> playerDiscardPile = new List<string> ();
	public List<string> infectionCardDeck = new List<string> ();
	public List<string> infectionDiscardPile = new List<string>();

	public GameData(Game game){
		challenge = game.getChallenge();
		currentPlayerRoleKind = game.getCurrentPlayer ().getRoleKind ();
		infectionRateIndex = game.getInfectionIndex ();
		outBreakRate = game.getOutbreakRate ();
		remainingResearch = game.getRemainingResearch ();
		List<CityInfo> CityInfoList = new List<CityInfo> ();
		difficulity = game.nEpidemicCard;

		playerCardDeck = game.getPlayerCardDeckString ();
		playerDiscardPile = game.getPlayerDiscardPileString ();
		infectionCardDeck = game.getInfectionDeckString ();
		infectionDiscardPile = game.getInfectionDiscardPileString ();


		foreach(City city in game.getCities()){
			CityInfo cityInfo = new CityInfo (city);
			CityInfoList.Add (cityInfo);
		}

		foreach (Player player in game.getPlayers()) {
			PlayerCardList playerHand= new PlayerCardList();
			roleKindList.Add (player.getRoleKind ());
			foreach (PlayerCard pc in player.getHand()) {
				if (pc.getType().Equals(CardType.CityCard)){
					CityCard cityCard = (CityCard)pc;
					playerHand.playerHand.Add (cityCard.getName());
					//Debug.Log ("City Card: " + cityCard.getName());
				}
				else if (pc.getType().Equals(CardType.EventCard)){
					EventCard eventCard = (EventCard)pc;
					playerHand.playerHand.Add (eventCard.getName());
					//Debug.Log ("Event Card: " + eventCard.getEventKind());
				}
			}
			playerCardList.Add (playerHand);
		}
	}

	public GameData(){
		
	}
}

[Serializable]
public class PlayerCardList
{
	public List<string> playerHand = new List<string>();

	public PlayerCardList(){
		
	}
}

[Serializable]
public class CityInfo{
	public Dictionary<String,int> cubes = new Dictionary<string, int>();
	public string cityName;

	public CityInfo(City city){
		cityName = city.getCityName ().ToString ();
		foreach (KeyValuePair<Color,int> entry in city.getNumOfCubes()) {
			cubes.Add (entry.Key.ToString(),entry.Value);
		}
	}

	public CityInfo(){
		
	}
}