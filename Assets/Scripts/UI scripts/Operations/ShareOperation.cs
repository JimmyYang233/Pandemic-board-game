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

    public void give()
    {
		
        agreePanel.SetActive(false);
        playerCardPanel.SetActive(false);
        showCardPanel.SetActive(true);


        basicOperationPanel.SetActive(false);
		otherPlayers.SetActive (false);
        //load all players current in the city



        //load card for the current city
        showCardPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentCity.getCityName().ToString();
        isTake = false;
    }
    public void take()
    {
        agreePanel.SetActive(false);
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
			game.take();
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
		cancel ();
	}
    public void cancel()
    {
		playerSelect.gameObject.SetActive (false);
        playerCardPanel.SetActive(true);
        showCardPanel.SetActive(false);
        basicOperationPanel.SetActive(true);
		otherPlayers.SetActive (true);
    }
	//pop permission panel TODO
	public void askPermission(){
		
	}

}
