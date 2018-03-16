using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassOperation : MonoBehaviour {

    public GameObject informResultPanel;
    public Game game;

    public void passButtonClicked()
    {
        informResultPanel.SetActive(true);
        informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "Start draw Card";
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(startDrawCard);
    }
    
    public void startDrawCard()
    {
        game.EndTurn();
        informResultPanel.SetActive(false);
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveListener(startDrawCard);
    }

    public void startInfection()
    {
        informResultPanel.SetActive(true);
        informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "Start Infection";
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(startInfectNextCity);
    }

    private void startInfectNextCity()
    {
        game.InfectNextCity();
        informResultPanel.SetActive(false);
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveListener(startInfectNextCity);
    }

    public void startEpidemic()
    {
        informResultPanel.SetActive(true);
        informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "Resolve Epidemic";
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(startResolveEpidemic);
    }

    private void startResolveEpidemic()
    {
        //game.ResolveEpidemic();
        informResultPanel.SetActive(false);
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveListener(startResolveEpidemic);
    }
}
