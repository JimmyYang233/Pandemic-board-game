using System.Collections;
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
			foreach (List<PlayerCard> playerCards in data.playerCardList) {
				foreach (PlayerCard pc in playerCards) {
					if (pc.getType().Equals(CardType.CityCard)){
						CityCard cityCard = (CityCard)pc;
						Debug.Log ("City Card: " + cityCard.getName());
					}
					else if (pc.getType().Equals(CardType.EventCard)){
						EventCard eventCard = (EventCard)pc;
						Debug.Log ("Event Card: " + eventCard.getEventKind());
					}
				}
			}

			foreach (RoleKind rk in data.roleKindList) {
				Debug.Log ("RoleKind is " + rk.ToString());
			}
			//-------------------------
			stream.Close ();
			return data;
		} else
			return null;
	}
}

[Serializable]
/*
	this class will be used to save game data
	the player's role will be saved in a list and their 


*/
public class GameData{
	//list of player's hand cards
	public List<List<PlayerCard>> playerCardList = new List<List<PlayerCard>>();
	public Challenge challenge;
	public List<RoleKind> roleKindList = new List<RoleKind>();
	public GameData(Game game){
		challenge = game.getChallenge();
		foreach (Player player in game.getPlayers()) {
			playerCardList.Add (player.getHand ());
			roleKindList.Add (player.getRoleKind ());
			//debug purpose
			/*
			foreach (PlayerCard pc in player.getHand()) {
				if (pc.getType().Equals(CardType.CityCard)){
					CityCard cityCard = (CityCard)pc;
					Debug.Log ("City Card: " + cityCard.getName());
				}
				else if (pc.getType().Equals(CardType.EventCard)){
					EventCard eventCard = (EventCard)pc;
					Debug.Log ("Event Card: " + eventCard.getEventKind());
				}
			}*/
		}
	}
}