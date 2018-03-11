using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : MonoBehaviour {

	public static PhotonNetworkManager instance = null;

	public InputField roomName;
	public InputField maxCount;
	public GameObject roomPrefab;

	private string _gameVersion = "0.1";
	private List<GameObject> roomPrefabs = new List<GameObject> ();

	#region private methods
	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject.transform);
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void Start(){
		PhotonNetwork.ConnectUsingSettings (_gameVersion);
		PhotonNetwork.playerName = "Player #" + Random.Range (10000, 99999);
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
		SceneManager.LoadScene ("Room");
		PhotonNetwork.JoinRoom (roomName);
	}

	void OnGUI(){
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString());
	}

	void OnJoinedLobby(){
		Debug.Log ("Lobby");
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
				RO.MaxPlayers = byte.Parse (maxCount.text);
				PhotonNetwork.CreateRoom (roomName.text, RO, TypedLobby.Default);
				SceneManager.LoadScene ("Room");
			}
			break;
		case "Refresh":
			if (PhotonNetwork.JoinLobby ()) {
				RefreshRoomList ();
			}
			break;
		}
	}
	#endregion

}
