using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CureOperation : MonoBehaviour {
    public Game game;
    public Button cureBlue;
    public Button cureYellow;
    public Button cureBlack;
    public Button cureRed;
    public Button cancelButton;
    public GameObject playerCardPanel;
    public Button confirmCure;
    Player currentPlayer;
    Maps mapInstance;
    List<CityCard> cardsToCure;
    List<Button> children;

    // Use this for initialization
    void Start () {
        mapInstance = Maps.getInstance();
        cardsToCure = new List<CityCard>();
	}
	
	// Update is called once per frame
	void Update () {
        currentPlayer = game.getCurrentPlayer();
        
	}

    public void cureButtonClicked()
    {
        if (currentPlayer.hasEnoughCard(Color.blue))
        {
            cureBlue.GetComponent<Button>().interactable = true;
        }
        if (currentPlayer.hasEnoughCard(Color.yellow))
        {
            cureYellow.GetComponent<Button>().interactable = true;
        }
        if (currentPlayer.hasEnoughCard(Color.black))
        {
            cureBlack.GetComponent<Button>().interactable = true;
        }
        if (currentPlayer.hasEnoughCard(Color.red))
        {
            cureRed.GetComponent<Button>().interactable = true;
        }
        cancelButton.GetComponent<Button>().interactable = true;
    }

    public void cureBlueButtonClicked()
    {
        cureBlue.GetComponent<Button>().interactable = false;
        cureYellow.GetComponent<Button>().interactable = false;
        cureBlack.GetComponent<Button>().interactable = false;
        cureRed.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;

        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            if (child.GetComponent<Image>().color == Color.blue)
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(() => addCardToList(name));
                child.GetComponent<Button>().onClick.AddListener(() => child.GetComponent<playerCardUI>().mouseOn());
            }
            else
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
        confirmCure.gameObject.SetActive(true);
    }

    public void cureYellowButtonClicked()
    {
        cureBlue.GetComponent<Button>().interactable = false;
        cureYellow.GetComponent<Button>().interactable = false;
        cureBlack.GetComponent<Button>().interactable = false;
        cureRed.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;

        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            if (child.GetComponent<Image>().color == Color.yellow)
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(() => addCardToList(name));
                child.GetComponent<Button>().onClick.AddListener(() => child.GetComponent<playerCardUI>().mouseOn());
            }
            else
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
        confirmCure.gameObject.SetActive(true);
    }

    public void cureRedButtonClicked()
    {
        cureBlue.GetComponent<Button>().interactable = false;
        cureYellow.GetComponent<Button>().interactable = false;
        cureBlack.GetComponent<Button>().interactable = false;
        cureRed.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;

        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            if (child.GetComponent<Image>().color == Color.red)
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(() => addCardToList(name));
                child.GetComponent<Button>().onClick.AddListener(() => child.GetComponent<playerCardUI>().mouseOn());
            }
            else
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
        confirmCure.gameObject.SetActive(true);

    }

    public void cureBlackButtonClicked()
    {
        cureBlue.GetComponent<Button>().interactable = false;
        cureYellow.GetComponent<Button>().interactable = false;
        cureBlack.GetComponent<Button>().interactable = false;
        cureRed.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = true;

        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            if (child.GetComponent<Image>().color == Color.black)
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(() => addCardToList(name));
                child.GetComponent<Button>().onClick.AddListener(() => child.GetComponent<playerCardUI>().mouseOn());
            }
            else
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
        confirmCure.gameObject.SetActive(true);
    }

    public void cancelButtonClicked()
    {
        cureBlue.GetComponent<Button>().interactable = false;
        cureYellow.GetComponent<Button>().interactable = false;
        cureBlack.GetComponent<Button>().interactable = false;
        cureRed.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = false;
        confirmCure.gameObject.SetActive(false);

    }

    private void addCardToList(String cityName)
    {
        CityCard card = currentPlayer.getCard(game.findCity(cityName));
        cardsToCure.Add(card);
    }



}
