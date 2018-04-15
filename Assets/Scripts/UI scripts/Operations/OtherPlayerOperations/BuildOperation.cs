using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildOperation : MonoBehaviour
{
    public Game game;
    public BasicOperation basicOperation;
    Player currentPlayer;
    City currentCity;
    City stationToRemove;
    List<City> citiesWithResearch;
    List<City> citiesWithMarker;
    public Button buildResearchButton;
    public Button buildQuarantineButton;
    public Button cancelButton;
    //Button build;
    // Use this for initialization
    void Start()
    {
        stationToRemove = null;
        citiesWithResearch = new List<City>();
        citiesWithMarker = new List<City>();
    }

    private void Update()
    {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();

    }

    public void buildButtonClicked()
    {
        if (currentCity.getMarker() == 0 && (game.getChallenge() != Challenge.BioTerroist) && (game.getChallenge() != Challenge.BioTerroistAndVirulentStrain))
        {
            buildQuarantineButton.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            buildQuarantineButton.gameObject.GetComponent<Button>().interactable = false;
        }
        if (!(currentCity.getHasResearch()))
        {
            buildResearchButton.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            buildResearchButton.gameObject.GetComponent<Button>().interactable = false;
        }
        cancelButton.gameObject.GetComponent<Button>().interactable = true;
    }

    public void buildResearchButtonClicked()
    {

        if (game.getRemainingResearch() == 0)
        {
            foreach (City city in game.getCities())
            {
                if (city.getHasResearch())
                {
                    citiesWithResearch.Add(city);
                }
            }
            foreach (City city in citiesWithResearch)
            {
                foreach (Transform child in city.transform)
                {
                    if (child.tag == "researchStation")
                    {
                        Debug.Log("addListener in button");
                        Button button = child.gameObject.GetComponent<Button>();
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(delegate { buildResearch(city); });
                        button.onClick.AddListener(delegate { basicOperation.resetAll(); });
                    }
                }
            }
        }
        else
        {
            game.Build(String.Empty, currentCity.getCityName().ToString());
        }

    }

    public void buildQuarantineButtonClicked()
    {
        if (game.getMarkersLeft() == 0)
        {
            foreach (City city in game.getCities())
            {
                if (city.getMarker() > 0)
                {
                    citiesWithMarker.Add(city);
                }
            }
            foreach (City city in citiesWithMarker)
            {
                foreach (Transform child in city.transform)
                {
                    if (child.tag == "marker")
                    {
                        Debug.Log("addListener in button");
                        Button button = child.gameObject.GetComponent<Button>();
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(delegate { buildQuarantine(city); });
                        button.onClick.AddListener(delegate { basicOperation.resetAll(); });
                    }
                }
            }
        }
        else
        {
            game.PlaceMarker("");
        }
    }


    public void cancelButtonClicked()
    {
        buildQuarantineButton.gameObject.GetComponent<Button>().interactable = false;
        buildResearchButton.gameObject.GetComponent<Button>().interactable = false;
        foreach (City city in citiesWithResearch)
        {
            foreach (Transform child in city.transform)
            {
                if (child.tag == "researchStation")
                {
                    Debug.Log("Removing Listener in research button");
                    Button button = child.gameObject.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                }
            }
        }

        foreach (City city in citiesWithMarker)
        {
            foreach (Transform child in city.transform)
            {
                if (child.tag == "marker")
                {
                    Debug.Log("Removing Listener in marker Button");
                    Button button = child.gameObject.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                }
            }
        }
        citiesWithResearch.Clear();
        citiesWithMarker.Clear();
    }

    public void buildResearch(City removeCity)
    {
        game.Build(removeCity.getCityName().ToString(), currentCity.getCityName().ToString());
        citiesWithResearch.Clear();
    }

    public void buildQuarantine(City removeCity)
    {
        game.PlaceMarker(removeCity.getCityName().ToString());
        citiesWithMarker.Clear();
    }
}
