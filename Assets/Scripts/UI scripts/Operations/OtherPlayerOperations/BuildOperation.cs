using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildOperation : MonoBehaviour {
	public Game game;
    public BasicOperation basicOperation;
	Player currentPlayer;
	City currentCity;
    City stationToRemove; 
    List<City> citiesWithResearch;
	//Button build;
	// Use this for initialization
	void Start () {
        stationToRemove = null;
        citiesWithResearch = new List<City>();
    }

    private void Update()
    {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
        
    }

    public void buildButtonClicked()
    {

        if(game.getRemainingResearch() == 0)
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
                foreach(Transform child in city.transform)
                {
                    if (child.tag == "researchStation")
                    {
                        Debug.Log("addListener in button");
                        Button button = child.gameObject.GetComponent<Button>();
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(delegate { build(city); });
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

    public void build(City removeCity)
    {
        game.Build(removeCity.getCityName().ToString(), currentCity.getCityName().ToString());
    }
}
