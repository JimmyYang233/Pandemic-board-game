using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareOperation : MonoBehaviour {
    public GameObject agreePanel;
    public GameObject playerCardPanel;
    public GameObject showCardPanel;
    public GameObject basicOperationPanel;
    Game game;
    Player currentPlayer;
    City currentCity;
    bool isTake;
    PCPanelController pc;
    void Start()
    {
        pc= GameObject.FindGameObjectWithTag("PlayerCardController").GetComponent<PCPanelController>();

        isTake = true;
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
    }
    public void give()
    {
        agreePanel.SetActive(false);
        playerCardPanel.SetActive(false);
        showCardPanel.SetActive(true);
        basicOperationPanel.SetActive(false);
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
        if (isTake)
        {
            pc.addCityCard(currentCity.cityName);
        }
        else
        {
            pc.deleteCityCard(currentCity.cityName);
        }
        cancel();
    }
    //player cancel the operation
    public void cancel()
    {
        playerCardPanel.SetActive(true);
        showCardPanel.SetActive(false);
        basicOperationPanel.SetActive(true);
    }

}
