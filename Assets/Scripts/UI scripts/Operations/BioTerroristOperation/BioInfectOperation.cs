using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioInfectOperation : MonoBehaviour {

    public Game game;
    public Button infectLocallyButton;
    public Button infectRemotellyButton;
    public Button cancelButton;
    public GameObject playerCardPanel;
    public GameObject basicOperationPanel;
    public GameObject infectPanel;
    public BioTerroristOperation bioTerroristOperation;
    Player currentPlayer;
    City currentCity;

    public void infectButtonClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
        infectLocallyButton.gameObject.GetComponent<Button>().interactable = true;
        if (currentPlayer.containsInfectionCard())
        {
            infectRemotellyButton.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            infectRemotellyButton.gameObject.GetComponent<Button>().interactable = false;
        }
        cancelButton.gameObject.GetComponent<Button>().interactable = true;
    }

    public void infectLocallyButtonClicked()
    {
        game.BioTerroristInfectLocally();
    }

    public void infectRemotellyButtonClicked()
    {
        infectLocallyButton.gameObject.GetComponent<Button>().interactable = false;
        infectRemotellyButton.gameObject.GetComponent<Button>().interactable = false;

        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            child.GetComponent<Button>().interactable = true;
            child.GetComponent<Button>().onClick.AddListener(() => startInfect(name));
            child.GetComponent<Button>().onClick.AddListener(() => basicOperationPanel.gameObject.SetActive(true));
            child.GetComponent<Button>().onClick.AddListener(() => infectPanel.gameObject.SetActive(false));
            child.GetComponent<Button>().onClick.AddListener(() => bioTerroristOperation.resetAll());

        }
    }

    public void startInfect(string cityName)
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
        game.BioTerroristInfectRemotely(cityName);
    }

    public void cancelButtonClicked()
    {
        infectLocallyButton.gameObject.GetComponent<Button>().interactable = false;
        infectRemotellyButton.gameObject.GetComponent<Button>().interactable = false;
        cancelButton.gameObject.GetComponent<Button>().interactable = false;
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            child.GetComponent<Button>().interactable = false;
            child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
