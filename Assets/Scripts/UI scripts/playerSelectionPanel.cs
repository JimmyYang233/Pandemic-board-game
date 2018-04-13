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
	public enum Status {SHARE,EPIDEMIOLOGIST,REEXAMINEDRESEARCH,NEWASSIGNMENT,DISPATCHER};
	public ShareOperation share;
	public MoveOperation move;
	public EpidemiologistOperation epidemiologist;
	public eventCardController eventController;
	public Status selectStatus = Status.SHARE; 

	public otherPlayerCardSelection selectCard;

	void Awake(){
		map = Maps.getInstance ();
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
	public void setEpidemiologistStatus(){
		selectStatus = Status.EPIDEMIOLOGIST;
	}
	public void setReExaminedResearch(){
		selectStatus = Status.REEXAMINEDRESEARCH;
	}
	public void setNewAssignmentStatus(){
		selectStatus = Status.NEWASSIGNMENT;
	}
	//This is for initializsation, for the beginning of the game
    public void addOtherPlayer(RoleKind k)
    {
		int i = 0;

        foreach (Transform t in this.transform)
        {
			
            if (!t.gameObject.activeSelf)
            {
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
		string name = EventSystem.current.currentSelectedGameObject.name;
		if (selectStatus == Status.SHARE) {
			
			if (!name.Equals ("Researcher")) {
				share.selectRole (name);
			} else {
				selectCard.gameObject.SetActive (true);
				selectCard.setResearcherStatus ();
				selectCard.loadOtherPlayerCard ("Researcher");
			}
		}
		//remember to turn into status.share
		else if (selectStatus == Status.EPIDEMIOLOGIST) {
			epidemiologist.characterSelect (name);
		} else if (selectStatus == Status.REEXAMINEDRESEARCH) {
			eventController.selectReExaminedResearchPlayer (name);
		} else if (selectStatus == Status.NEWASSIGNMENT) {
			eventController.selectNewAssignmentPlayer (name);
		} else if (selectStatus == Status.DISPATCHER) {
			move.changePlayerToMove (name);
		}
	}
	public void cancelButtonClick(){
		if (selectStatus == Status.SHARE) {
			share.cancel ();
		}
	}
    public void displayPlayerWithCard()
    {
        
        foreach (Transform t in transform)
        {
            
            if (!t.name.Equals("noUse"))
            {
                string role = t.GetChild(0).GetComponent<Text>().text;
                Player p = game.findPlayer(role);
                currentPlayer = game.getCurrentPlayer();
                currentCity = currentPlayer.getPlayerPawn().getCity();

                if (p.containsSpecificCityCard(currentCity))
                {
                    Debug.Log("find");
                    t.gameObject.SetActive(true);

                }
				else if(role.Equals("Researcher")){
					Debug.Log ("find researcher");
					t.gameObject.SetActive (true);
				}
                else
                {
                    Debug.Log("not find");
                    t.gameObject.SetActive(false);
                }
            }

        }
    }
	//only display player who is in the same city
	public void displayPlayerNecessary(){
		foreach (Transform t in transform) {
			if (!t.name.Equals("noUse"))
			{
				string role = t.GetChild (0).GetComponent<Text> ().text;
				Player p = game.findPlayer (role);
                currentPlayer = game.getCurrentPlayer();
                currentCity = currentPlayer.getPlayerPawn().getCity();
                Debug.Log(currentCity);
				if (p.getPlayerPawn ().getCity () == currentCity) {
					t.gameObject.SetActive (true);

				} else {
					t.gameObject.SetActive (false);
				}
			}

		}
	}

	public void displayAllPlayerForEventCard(){
		foreach (Transform t in transform) {
			if (!t.name.Equals("noUse"))
			{
				t.gameObject.SetActive (true);
			}

		}
	}

	public void clear(){
		foreach (Transform t in transform) {
			if (!t.name.Equals("noUse"))
			{
				t.gameObject.SetActive (false);
			}

		}
	}
		
}
