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

    public void give()
    {
        agreePanel.SetActive(false);
        playerCardPanel.SetActive(false);
        showCardPanel.SetActive(true);
        basicOperationPanel.SetActive(false);
        showCardPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentCity.getCityName().ToString();
        isTake = false;
    }
    public void take()
    {
        agreePanel.SetActive(false);
        playerCardPanel.SetActive(false);
        showCardPanel.SetActive(true);
        basicOperationPanel.SetActive(false);
        showCardPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentCity.getCityName().ToString();
        isTake = true;
    }
    public void check()
    {
        if (isTake)
        {

        }
        else
        {
          
        }
        cancel();
    }
    public void cancel()
    {
        playerCardPanel.SetActive(true);
        showCardPanel.SetActive(false);
        basicOperationPanel.SetActive(true);
    }
	// Use this for initialization
	void Start () {
        isTake = true;
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
