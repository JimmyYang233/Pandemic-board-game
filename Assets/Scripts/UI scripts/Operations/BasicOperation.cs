using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicOperation : MonoBehaviour {

    public Button moveButton;
    public Button treatButton;
    public Button cureButton;
    public Button buildButton;
    public Button shareButton;
    public Button RoleOnlyButton;
    public Button passButton;
	public Button giveButton;
	public Button takeButton;

    Game game;
    Player me;
    City currentCity;
    // Use this for initialization
    void Start () {
       // game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
		game = Game.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		me = game.FindPlayer(PhotonNetwork.player);
        if (game.getCurrentPlayer() == me)
        {
            currentCity = me.getPlayerPawn().getCity();
            if (me.getRemainingAction() != 0)
            {
                moveButton.GetComponent<Button>().interactable = true;
                if (currentCity.hasCubes())
                {
                    treatButton.GetComponent<Button>().interactable = true;
                }
                foreach(Pawn pawn in me.getPlayerPawn().getCity().getPawns())
                {
                    foreach(Player p in game.getPlayers())
                    {
						if (p.getPlayerPawn () == pawn && p.containsSpecificCityCard (currentCity)) {
							shareButton.GetComponent<Button> ().interactable = true;
							if (me.Equals (p)) {
								takeButton.GetComponent<Button> ().interactable = false;
								giveButton.GetComponent<Button> ().interactable = true;
							} else {
								takeButton.GetComponent<Button> ().interactable = true;
								giveButton.GetComponent<Button> ().interactable = false;
							}
						}

                    }
                }
                passButton.GetComponent<Button>().interactable = true;
            }
            else if (me.getRemainingAction() == 0)
            {
                moveButton.GetComponent<Button>().interactable = false;
                treatButton.GetComponent<Button>().interactable = false;
                shareButton.GetComponent<Button>().interactable = false;
                passButton.GetComponent<Button>().interactable = true;
            }
        }
        else if(game.getCurrentPlayer()!= me)
        {
            moveButton.GetComponent<Button>().interactable = false;
            treatButton.GetComponent<Button>().interactable = false;
            shareButton.GetComponent<Button>().interactable = false;
            passButton.GetComponent<Button>().interactable = false;
        }
	}
}
