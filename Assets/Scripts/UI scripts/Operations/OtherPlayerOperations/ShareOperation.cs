using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareOperation : MonoBehaviour {
    public GameObject agreePanel;
    public GameObject playerCardPanel;
	public GameObject informResultPanel;
    public GameObject shareOperationPanel;
    public GameObject waitingPanel;

    public GameObject basicOperationPanel;
	public playerSelectionPanel playerSelect;
	public GameObject otherPlayers;
	public otherPlayerCardSelection otherPlayerSelction;
    public Button giveButton;
    public Button takeButton;
    public Button cancelButton;
    public BasicOperation basicOperation;

    Game game;
    Player currentPlayer;
    City currentCity;
	Status shareStatus = Status.NULL;
    PCPanelController pc;
	string roleSlected;
    GameObject cardToShare;

	private enum Status {GIVE, TAKE, NULL};

    void Start()
    {
        pc= GameObject.FindGameObjectWithTag("PlayerCardController").GetComponent<PCPanelController>();
        //game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
		game = Game.Instance;
    }

	void Update (){
		currentPlayer = game.getCurrentPlayer();
		currentCity = currentPlayer.getPlayerPawn().getCity();

	}

    public void giveButtonClicked()
    {
		/*
        //agreePanel.SetActive(false);
        giveButton.GetComponent<Button>().interactable = false;
        int num = playerCardPanel.transform.GetChild(1).childCount;
        /*for(int i = 0; i< num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            if(currentPlayer.getRoleKind() == RoleKind.Researcher)
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(() => addCardToShare(child));
                child.GetComponent<Button>().onClick.AddListener(check);
            }
            else
            {
                if (name == currentCity.getCityName().ToString())
                {
                    child.GetComponent<Button>().interactable = true;
                    cardToShare = child;
                    child.GetComponent<Button>().onClick.AddListener(check);
                }
                else
                {
                    child.GetComponent<Button>().interactable = false;
                }
            }
            
        }
		playerSelect.selectStatus = playerSelect.share;
		playerSelect.displayPlayerNecessary ();
        basicOperationPanel.SetActive(false);
		otherPlayers.SetActive (false);
        //load all players current in the city
        //load card for the current city
        isTake = false;*/
		giveButton.GetComponent<Button>().interactable = false;
		otherPlayers.SetActive (false);
		basicOperationPanel.SetActive(false);

		playerSelect.selectStatus = playerSelectionPanel.Status.SHARE;
		playerSelect.displayPlayerNecessary ();
		playerSelect.gameObject.SetActive (true);
		shareStatus = Status.GIVE;
    }

    protected void btn_Click(object sender, EventArgs e)
    {
        check();
        playerSelect.displayPlayerNecessary();
        
    }

    public void addCardToShare(GameObject child)
    {
        cardToShare = child;
    }
    public void takeButtonClicked()
    {
        //agreePanel.SetActive(false);
        takeButton.GetComponent<Button>().interactable = false;
		/*
        playerCardPanel.SetActive(false);
        basicOperationPanel.SetActive(false);
        otherPlayers.SetActive(false);
        check();
        playerSelect.displayPlayerWithCard();*/
		otherPlayers.SetActive (false);
		basicOperationPanel.SetActive(false);
		playerSelect.selectStatus = playerSelectionPanel.Status.SHARE;


		playerSelect.displayPlayerWithCardOrResearcher();
		playerSelect.gameObject.SetActive (true);
		shareStatus = Status.TAKE;
    }
    //player confirm to take the card
    public void check()
	{

    }
    //player cancel the operation
	public void roleSelected(string name){
		currentPlayer = game.getCurrentPlayer ();
		currentCity = currentPlayer.getPlayerPawn ().getCity ();
		if (shareStatus == Status.GIVE) {
			if (currentPlayer.getRoleKind () == RoleKind.Researcher) {
				int num = playerCardPanel.transform.GetChild(1).childCount;
				for (int i = 0; i < num; i++) {
					Debug.Log (i);
					GameObject child = playerCardPanel.transform.GetChild (1).GetChild (i).gameObject;
					string tname = playerCardPanel.transform.GetChild (1).GetChild (i).GetChild (0).gameObject.GetComponent<Text> ().text;
					child.GetComponent<Button> ().interactable = true;
					child.GetComponent<Button> ().onClick.AddListener (() => share(name, tname));
				}

			} 
			else
			{
                basicOperationPanel.SetActive(true);
                basicOperation.resetAll();
                game.share (name, currentCity.getCityName ().ToString ());
                waitingPanel.SetActive (true);
				cancel ();
			}
		} else if (shareStatus ==Status.TAKE) {
			if (name.Equals ("Researcher")) {
				otherPlayerSelction.selectStatus=otherPlayerCardSelection.Status.RESEARCHER;
				otherPlayerSelction.loadOtherPlayerCard ("Researcher");
				otherPlayerSelction.gameObject.SetActive (true);

			} else {
                basicOperation.resetAll();
                waitingPanel.SetActive(true);
                game.share (name, currentCity.getCityName ().ToString ());
                basicOperationPanel.SetActive(true);
                cancel ();
			}
		}
		/*
		roleSlected=name;
		if (name.Equals ("Researcher")) {
			if (isTake) {
				takeFromResearcher;
			}
		} else {
			game.share (name, cardToShare.transform.GetChild (0).gameObject.GetComponent<Text> ().text);
		}
        waitingPanel.SetActive(true);
		cancel();*/

	}

    public void share(string roleKind, string cardName)
    {
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string tname = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            child.GetComponent<Button>().interactable = false;
            child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        basicOperationPanel.SetActive(true);
        basicOperation.resetAll();
        game.share(roleKind, cardName);
    }

	public void takeFromResearcher(string cityName){
        basicOperationPanel.SetActive(true);
        basicOperation.resetAll();
        game.share("Researcher",cityName);
		waitingPanel.SetActive(true);
		cancel();		
	}

    public void cancel()
    {
		playerSelect.gameObject.SetActive (false);
        playerCardPanel.SetActive(true);
		otherPlayers.SetActive (true);
        giveButton.GetComponent<Button>().interactable = false;
        takeButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = false;

    }
	//pop permission panel TODO
	public void askGivePermission(string name){
        
		agreePanel.transform.GetChild (0).GetComponent<Text> ().text= currentPlayer.getRoleKind().ToString() + " want to give you " + name;
		agreePanel.SetActive (true);
	}

    public void askTakePermission(string name)
    {
        agreePanel.transform.GetChild(0).GetComponent<Text>().text = currentPlayer.getRoleKind().ToString() + " want you to give him " + name;
        agreePanel.SetActive(true);
    }
	public void acceptRequest(){
		game.sendResponse (true);
		agreePanel.SetActive (false);
	}
	public void declineRequest(){
		game.sendResponse (false);
		agreePanel.SetActive (false);
	}

	//pop agree or disagree message TODO
	public void showResponse(bool response){
		informResultPanel.SetActive (true);
		if (response) {
            informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "He accept, now you have the card";
        }
		else{
            informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "He reject, maybe you can try again";
            cardToShare.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        cancel();
        waitingPanel.SetActive(false);
        basicOperationPanel.SetActive(true);
        basicOperation.resetAll();
        shareOperationPanel.SetActive(false);
    }
	public string findCityCardPlayer(string cardname){
		Player target = game.findPlayerWithCard (cardname);
		return target.getRoleKind ().ToString ();
	}


	//for interactable or not
    public void shareButtonClicked()
    {
        if((currentPlayer.getRoleKind() == RoleKind.Researcher&&currentPlayer.containsCityCard()) || currentPlayer.containsSpecificCityCard(currentCity))
        {
            giveButton.GetComponent<Button>().interactable = true;
        }
        List<Player> players = game.getPlayers(currentCity);
        foreach(Player player in players)
        {
            if ((player.containsSpecificCityCard(currentCity)||(player.getRoleKind() == RoleKind.Researcher&&player.containsCityCard()))&&player!=currentPlayer)
            {
                takeButton.GetComponent<Button>().interactable = true;
            }
        }
        cancelButton.GetComponent<Button>().interactable = true;
    }
}
