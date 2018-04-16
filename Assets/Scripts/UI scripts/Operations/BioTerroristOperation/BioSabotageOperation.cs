using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioSabotageOperation : MonoBehaviour {

    public Game game;
    public GameObject playerCardPanel;
    Player currentPlayer;
    City currentCity;


	
	// Update is called once per frame
	void Update () {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
	}

    public void sabotageButtonClicked()
    {
        Color color = currentCity.getColor();
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            if (child.GetComponent<Image>().color == color)
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(() => sabotage(name));
            }
            else
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void sabotage(string cardName)
    {
  
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            child.GetComponent<Button>().interactable = false;
            child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        game.BioTerroristSabotage(cardName);
    }
}
