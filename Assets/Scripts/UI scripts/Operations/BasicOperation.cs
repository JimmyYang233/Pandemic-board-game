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

    Game game;
    Player me;
    City currentCity;
    // Use this for initialization
    void Start () {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        //for test only
        
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(game.getCurrentPlayer().getUsername());
        List<Player> players = game.getPlayers();
        foreach(Player player in players)
        {
            if (player.getUsername() == "Jack")
            {
                me = player;
            }
        }
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
                        if(p.getPlayerPawn() == pawn&& p.containsSpecificCityCard(currentCity))
                        {
                            shareButton.GetComponent<Button>().interactable = true;
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
