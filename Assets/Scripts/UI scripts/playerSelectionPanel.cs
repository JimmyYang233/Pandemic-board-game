using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class playerSelectionPanel : MonoBehaviour {
	Maps map;
	//TODO- other operation
	public Game game;
	private City currentCity;
	private Player currentPlayer;
	private enum Status {SHARE,OTHER};
	public ShareOperation share;
	private Status selectStatus = Status.SHARE; 
	public bool[] showOrNot;//to remember whether the spot is used or not  
	void Awake(){
		map = Maps.getInstance ();
		showOrNot = new bool[4];
	}
	void Update (){
		currentPlayer = game.getCurrentPlayer();
		currentCity = currentPlayer.getPlayerPawn().getCity();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	public void setShareStatus(){
		selectStatus = Status.SHARE;
	}
	//todo other status reimplement
	public void setOtherStatus(){
		selectStatus = Status.OTHER;
	}

    public void addOtherPlayer(RoleKind k)
    {
		int i = 0;

        foreach (Transform t in this.transform)
        {
			
            if (!t.gameObject.activeSelf)
            {
				showOrNot [i] = true;
                t.gameObject.SetActive(true);
				t.GetChild(0).GetComponent<Text>().text = k.ToString();
				t.name = k.ToString ();
				if (map.getRoleColor(k) != null)
				{
					

					t.GetComponent<Image>().color = map.getRoleColor(k);
				}

                break;
            }
			i++;
        }

        // apply it on current object's material
    }
	public void characterSelect(){
		if (selectStatus == Status.SHARE) {
			share.selectRole(EventSystem.current.currentSelectedGameObject.name);
		}
	}
	public void cancelButtonClick(){
		if (selectStatus == Status.SHARE) {
			share.cancel ();
		}
	}
	//only display player who is in the same city
	public void displayPlayerNecessary(){
		foreach (Transform t in transform) {
			if (t.gameObject.activeSelf)
			{
				string role = t.GetChild (0).GetComponent<Text> ().text;
				Player p = game.findPlayer (role);
				if(!p.getPlayerPawn ().getCity ().cityName.ToString().Equals("Atlanta")){
					t.gameObject.SetActive (false);

				}
			}
		}
	}

	public void reset(){
		foreach (Transform t in transform) {
			if (!t.name.Equals ("noUse")) {
				t.gameObject.SetActive (true);
				Debug.Log ("setactive");
			}
		}
	}
}
