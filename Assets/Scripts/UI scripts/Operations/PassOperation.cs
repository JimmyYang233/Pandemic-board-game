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
        Debug.Log("Pass button clicked");
        informResultPanel.SetActive(true);
        informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "Start draw Card";
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(startDrawCard);
    }
    
    public void startDrawCard()
    {
        informResultPanel.SetActive(false);
        game.EndTurn();
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveListener(startDrawCard);
    }

    public void startInfection()
    {
        Debug.Log("StartInfection got called");
        informResultPanel.SetActive(true);
        Debug.Log(informResultPanel.GetActive());
        informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "Start Infection";
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(startInfectNextCity);
    }

    private void startInfectNextCity()
    {
        Debug.Log("StartInfectNextCity got called");
        informResultPanel.SetActive(false);
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveListener(startInfectNextCity);
        game.InfectNextCity();
        
    }

    public void notifyResolveEpidemic()
    {
        informResultPanel.SetActive(true);
        informResultPanel.transform.GetChild(0).GetComponent<Text>().text = "Resolve Epidemic";
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(close);
    }

    private void close()
    {
        //game.ResolveEpidemic();
        informResultPanel.SetActive(false);
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveListener(close);
    }

    private void startResolveEpidemic()
    {
        //game.ResolveEpidemic();
        informResultPanel.SetActive(false);
        informResultPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveListener(startResolveEpidemic);
    }
}
