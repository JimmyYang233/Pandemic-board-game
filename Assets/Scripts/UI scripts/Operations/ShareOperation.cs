using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareOperation : MonoBehaviour {
    public GameObject agreePanel;
    public GameObject playerCardPanel;
    public GameObject showCardPanel;
    public GameObject basicOperationPanel;
	public playerSelectionPanel playerSelect;
	public GameObject otherPlayers;
    public Button giveButton;
    public Button takeButton;
    public Button cancelButton;

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
        playerCardPanel.SetActive(false);
        showCardPanel.SetActive(true);


        basicOperationPanel.SetActive(false);
		otherPlayers.SetActive (false);
        //load all players current in the city



        //load card for the current city
        showCardPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentCity.getCityName().ToString();
        isTake = false;
    }
    public void takeButtonClicked()
    {
        //agreePanel.SetActive(false);
        playerCardPanel.SetActive(false);
        showCardPanel.SetActive(true);
        basicOperationPanel.SetActive(false);
        //load card for the current city.just now
        showCardPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentCity.getCityName().ToString();
        isTake = true;
    }
    //player confirm to take the card
    public void check()
	{
		playerSelect.setShareStatus ();
        if (isTake)
        {
			
            //pc.addCityCard(currentCity.cityName);
			//game.take(currentCity.getCityName().ToString());
			cancel();
        }
        else
        {
			showCardPanel.SetActive (false);
			playerSelect.gameObject.SetActive (true);
            

        }
       
    }
    //player cancel the operation
	public void selectRole(string name){
		roleSlected=name;
		//pc.deleteCityCard(currentCity.cityName);
		cancel();
	}
    public void cancel()
    {
		playerSelect.gameObject.SetActive (false);
        playerCardPanel.SetActive(true);
        showCardPanel.SetActive(false);
        basicOperationPanel.SetActive(true);
		otherPlayers.SetActive (true);
        giveButton.GetComponent<Button>().interactable = false;
        takeButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = false;

    }
	//pop permission panel TODO
	public void askPermission(string name){
		agreePanel.SetActive (true);
	}
	public void acceptRequest(){
		
	}
	public void declineRequest(){
	}
	//receive response TODO
	public void sentResponse(){
		
	}
	//pop agree or disagree message TODO
	public void showResponse(bool response){
		
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
