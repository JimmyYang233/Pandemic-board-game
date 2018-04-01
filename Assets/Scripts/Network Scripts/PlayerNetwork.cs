using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork Instance;
	private PhotonView PhotonView;
	private int PlayersInGame = 0;
	public bool isNewGame = true;
	public string savedGameJson;

	private void Awake(){
		if (Instance == null) {
			Instance = this;
			PhotonView = GetComponent<PhotonView>();
			SceneManager.sceneLoaded += OnSceneFinishedLoading;
		}
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Main Game")
		{
			if (PhotonNetwork.isMasterClient) {
				MasterLoadedGame ();
			}
				
			else
				NonMasterLoadedGame();
		}
	}
		
	private void MasterLoadedGame()
	{
		PhotonView.RPC ("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
		PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
	}

	private void NonMasterLoadedGame()
	{
		PhotonView.RPC ("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
	}

	[PunRPC]
	private void RPC_LoadGameOthers()
	{
		PhotonNetwork.LoadLevel(3);
	}

	[PunRPC]
	private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
	{
		PlayerManagement.Instance.AddPlayer (photonPlayer);
		PlayersInGame++;
		if (PlayersInGame == PhotonNetwork.playerList.Length)
		{
			//print("All players are in the game scene.");
		}
	}
}
