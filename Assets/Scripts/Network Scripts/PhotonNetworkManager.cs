using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PhotonNetworkManager : MonoBehaviour {
	public InputField roomName;
	//public InputField maxCount;
	public InputField playerName;
	public GameObject roomPrefab;
    public GameObject loadPanel;

    private System.Random _rnd = new System.Random();
	private List<GameObject> roomPrefabs = new List<GameObject> ();
	private List<string> listOfSavedGame = new List<string>();

	#region private methods
	void Awake(){
		DirectoryInfo levelDirectoryPath = new DirectoryInfo(Application.persistentDataPath);
		FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);
		foreach (FileInfo file in fileInfo) {
			if (file.Extension == ".pandemic") {
				Debug.Log (file.Name);
				listOfSavedGame.Add (Path.GetFileNameWithoutExtension(file.Name));
			}
		}
		
	}

	void Start(){
	}

	void RefreshRoomList(){
		//destroy old rooms
		if (roomPrefabs.Count > 0) {
			for (int i = 0; i < roomPrefabs.Count; i++) {
				Destroy(roomPrefabs[i]);
			}

			roomPrefabs.Clear();
		}
		//find new rooms
		for (int i = 0; i < PhotonNetwork.GetRoomList().Length; i++) {
			Debug.Log (PhotonNetwork.GetRoomList()[i]);
			GameObject newRoom = Instantiate(roomPrefab);
			newRoom.transform.SetParent (GameObject.Find("Rooms").transform);
			newRoom.GetComponent<RectTransform> ().localScale = roomPrefab.GetComponent<RectTransform> ().localScale;
			newRoom.GetComponent<RectTransform> ().position = 
				new Vector3 (roomPrefab.GetComponent<RectTransform> ().position.x,
					roomPrefab.GetComponent<RectTransform> ().position.y - (i*50),
					roomPrefab.GetComponent<RectTransform> ().position.z);
			newRoom.transform.Find ("Room_Name_Text").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].Name;
			newRoom.transform.Find ("Room_Count_Info_Text").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].PlayerCount + "/" + PhotonNetwork.GetRoomList()[i].MaxPlayers;
			newRoom.transform.Find ("Join").GetComponent<Button> ().onClick.AddListener (() => { JoinRoom(newRoom.transform.Find("Room_Name_Text").GetComponent<Text>().text); });
			newRoom.SetActive (true);
			roomPrefabs.Add (newRoom);
		}
	}

	void JoinRoom(string roomName){
		PhotonNetwork.JoinRoom (roomName);
		Debug.Log ("Join room " + roomName);
		SceneManager.LoadScene ("Room");
	}

	void OnGUI(){
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString());
	}

	void OnJoinedLobby(){
		PhotonNetwork.automaticallySyncScene = true;
		Invoke ("RefreshRoomList",0.1f);
	}
		
	void OnPhotonJoinRoomFailed(){
		Debug.Log ("Join room failed");
	} 

	void OnJoinedRoom(){
		Debug.Log ("Joined Room");
	}

	void OnCreatedRoom(){
		Debug.Log ("Created Room");
	}


	#endregion

	#region public methods
	public void ButtonEvents(string EVENT){

		switch (EVENT) {
		case "CreateRoom":
			if (PhotonNetwork.JoinLobby ()) {
				RoomOptions RO = new RoomOptions ();
				RO.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable (){ { "Challenge",Challenge.Nochallenge.ToString() } };
				RO.MaxPlayers = 5;
				PhotonNetwork.CreateRoom (roomName.text, RO, TypedLobby.Default);

				SceneManager.LoadScene ("Room");
			}
			break;
		case "Refresh":
			if (PhotonNetwork.JoinLobby ()) {
				RefreshRoomList ();
			}
			break;
		//debug purpose

		//case "Load":
			//GameData data = SaveAndLoadManager.LoadGameData (roomName.text);
			/*
			Debug.Log ("when loaded, we have ");
			foreach (PlayerCardList pcl in data.playerCardList) {
				foreach (string pc in pcl.playerHand) {
					Debug.Log ("Player card " + pc);
				}
			}

			foreach (RoleKind rk in data.roleKindList) {
				Debug.Log ("RoleKind is " + rk.ToString());
			}*/
			/*
			PlayerNetwork.Instance.isNewGame = false;
			PlayerNetwork.Instance.savedGameJson =  JsonUtility.ToJson(data);
			//Debug.Log ("Saved GameJson is : " + PlayerNetwork.Instance.savedGameJson);
			GameData savedGame = JsonUtility.FromJson<GameData>(PlayerNetwork.Instance.savedGameJson);
			/*
			Debug.Log ("After loaded, we have ");
			foreach (string s in savedGame.infectionCardDeck) {
				Debug.Log ("Infection card: "+ s);
			}

			foreach (RoleKind rk in savedGame.roleKindList) {
				Debug.Log ("RoleKind is " + rk.ToString());
			}*/
			/*
			RoomOptions testRo = new RoomOptions ();
			testRo.MaxPlayers = 5;
			PhotonNetwork.CreateRoom (roomName.text, testRo, TypedLobby.Default);
			SceneManager.LoadScene ("Room");
			break;*/
		}
	}

    public void LoadButtonClicked()
    {
        loadPanel.gameObject.SetActive(true);
        int count = listOfSavedGame.Count;
        for(int i = 0; i< count; i++)
        {
            loadPanel.transform.GetChild(i).gameObject.SetActive(true);
            string loadGameName = listOfSavedGame[i];
            loadPanel.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = loadGameName;
            loadPanel.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(() => onClick(loadGameName));

        }
    }

    public void onClick(string loadName)
    {
		GameData data = SaveAndLoadManager.LoadGameData (loadName);
		Debug.Log (loadName);
		PlayerNetwork.Instance.isNewGame = false;
		PlayerNetwork.Instance.savedGameJson =  JsonUtility.ToJson(data);
		Debug.Log (PlayerNetwork.Instance.savedGameJson);
		GameData savedGame = JsonUtility.FromJson<GameData>(PlayerNetwork.Instance.savedGameJson);
		RoomOptions testRo = new RoomOptions ();
		testRo.MaxPlayers = 5;
		PhotonNetwork.CreateRoom (loadName, testRo, TypedLobby.Default);
		SceneManager.LoadScene ("Room");
    }
    #endregion

}
