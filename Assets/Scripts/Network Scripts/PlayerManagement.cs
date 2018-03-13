using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour {

	public static PlayerManagement Instance;
	private PhotonView PhotonView;
	public List<Player> Players = new List<Player> ();
	// Use this for initialization
	private void Awake(){
		Instance = this;
		PhotonView = GetComponent<PhotonView> ();
	}

	public void AddPlayer(PhotonPlayer photonPlayer){
		int index = Players.FindIndex (x => x.PhotonPlayer == photonPlayer);
		if (index == -1) {
			Players.Add (new Player(photonPlayer));
		}
	}

	public Player FindLocalPlayer(PhotonPlayer photonPlayer){
		int index = Players.FindIndex (x => x.PhotonPlayer == photonPlayer);
		return Players [index];
	}

}
