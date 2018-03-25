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
    List<String> cardsToCure;
    List<GameObject> children;
    private Color colorToCure = Color.blue;

    // Use this for initialization
    void Start () {
        mapInstance = Maps.getInstance();
        cardsToCure = new List<String>();
        children = new List<GameObject>();
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
        goCure(Color.blue);
    }

    public void cureYellowButtonClicked()
    {
        goCure(Color.yellow);
    }

    public void cureRedButtonClicked()
    {
        goCure(Color.red);
    }

    public void cureBlackButtonClicked()
    {
        goCure(Color.black);
    }

    public void cancelButtonClicked()
    {
        cureBlue.GetComponent<Button>().interactable = false;
        cureYellow.GetComponent<Button>().interactable = false;
        cureBlack.GetComponent<Button>().interactable = false;
        cureRed.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = false;
        confirmCure.gameObject.SetActive(false);
        foreach (GameObject child in children)
        {
            child.GetComponent<playerCardUI>().mouseClick();
            child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            child.GetComponent<Button>().interactable = false;
        }
        children.Clear();
        cardsToCure.Clear();

    }

    public void startCure()
    {
        if (cardsToCure.Count == currentPlayer.getNumberOfCardNeededToCure())
        {
            game.Cure(currentPlayer.getRoleKind().ToString(), cardsToCure, "Color." + colorToCure.ToString());
            cancelButtonClicked();
        }
    }

    private void addCardToList(String cityName)
    {
        cardsToCure.Add(cityName);
    }

    private void goCure(Color color)
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
            if (child.GetComponent<Image>().color == color)
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(() => addCardToList(name));
                child.GetComponent<Button>().onClick.AddListener(() => child.GetComponent<playerCardUI>().mouseClick());
                child.GetComponent<Button>().onClick.AddListener(() => child.GetComponent<Button>().interactable = false);
                child.GetComponent<Button>().onClick.AddListener(() => children.Add(child));
            }
            else
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
        colorToCure = color;
        confirmCure.gameObject.SetActive(true);
    }



}
