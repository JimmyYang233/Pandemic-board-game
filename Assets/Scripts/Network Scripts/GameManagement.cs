using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour {

	public static GameManagement Instance;
	private Game game;
	public PhotonView PhotonView;

	private void Awake(){
		Instance = this;
		game = Game.Instance;
		PhotonView = PhotonView = GetComponent<PhotonView>();
		if (PhotonNetwork.isMasterClient)
			PhotonView.RPC ("RPC_InitializeGame",PhotonTargets.All);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	[PunRPC]
	private void RPC_InitializeGame(){
		game.InitializeGame ();
	}
}
