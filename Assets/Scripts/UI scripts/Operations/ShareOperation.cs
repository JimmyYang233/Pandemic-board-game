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

    public GameObject basicOperationPanel;
	public playerSelectionPanel playerSelect;
	public GameObject otherPlayers;
    public Button giveButton;
    public Button takeButton;
    public Button cancelButton;
    public BasicOperation basicOperation;

    Game game;
    Player currentPlayer;
    City currentCity;
    bool isTake;
    PCPanelController pc;
	string roleSlected;


    void Start()
    {
        pc= GameObject.FindGameObjectWithTag("PlayerCardController").GetComponent<PCPanelController>();

        isTake = true;
        //game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
		game = Game.Instance;
    }

	void Update (){
		currentPlayer = game.getCurrentPlayer();
		currentCity = currentPlayer.getPlayerPawn().getCity();
	}

    public void giveButtonClicked()
    {

        //agreePanel.SetActive(false);
        giveButton.GetComponent<Button>().interactable = false;
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for(int i = 0; i< num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            if (name == currentCity.getCityName().ToString())
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(check);
            }
            else
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
        basicOperationPanel.SetActive(false);
		otherPlayers.SetActive (false);
        //load all players current in the city



        //load card for the current city
        isTake = false;
    }

    protected void btn_Click(object sender, EventArgs e)
    {
        check();
    }
    public void takeButtonClicked()
    {
        //agreePanel.SetActive(false);
        takeButton.GetComponent<Button>().interactable = false;
        playerCardPanel.SetActive(false);
        basicOperationPanel.SetActive(false);
        otherPlayers.SetActive(false);
        check();
        isTake = true;
    }
    //player confirm to take the card
    public void check()
	{
		playerSelect.setShareStatus ();
		playerSelect.gameObject.SetActive (true);
       
    }
    //player cancel the operation
	public void selectRole(string name){
		roleSlected=name;
		//pc.deleteCityCard(currentCity.cityName);
		game.share(name,currentCity.getCityName().ToString());
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
	public void askPermission(string name){
		agreePanel.transform.GetChild (0).GetComponent<Text> ().text="Accept changes to card"+name;
		agreePanel.SetActive (true);
	}
	public void acceptRequest(){
		game.sendResponse (true);
		agreePanel.SetActive (false);
	}
	public void declineRequest(){
		game.sendResponse (false);
		agreePanel.SetActive (false);
	}

	//receive response TODO
	public void sentResponse(){
		
	}
	//pop agree or disagree message TODO
	public void showResponse(bool response){
		informResultPanel.SetActive (true);
		if (response) {
            informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "Accept";
        }
		else{
            informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "Reject";
        }
        cancel();
        basicOperationPanel.SetActive(true);
        basicOperation.resetAll();
        shareOperationPanel.SetActive(false);
    }
	public string findCityCardPlayer(string cardname){
		Player target = game.findPlayerWithCard (cardname);
		return target.getRoleKind ().ToString ();
	}
    public void shareButtonClicked()
    {
        if(currentPlayer.getRoleKind() == RoleKind.Researcher || currentPlayer.containsSpecificCityCard(currentCity))
        {
            giveButton.GetComponent<Button>().interactable = true;
        }
        List<Player> players = game.getPlayers(currentCity);
        foreach(Player player in players)
        {
            if (player.containsSpecificCityCard(currentCity)&&player!=currentPlayer)
            {
                takeButton.GetComponent<Button>().interactable = true;
            }
        }
        cancelButton.GetComponent<Button>().interactable = true;
    }
}
