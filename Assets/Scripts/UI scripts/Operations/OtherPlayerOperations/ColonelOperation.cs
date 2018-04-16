using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColonelOperation : MonoBehaviour {

    public Game game;
    public GameObject playerCardPanel;
    Player currentPlayer;
    City currentCity;
    List<UnityEngine.Events.UnityAction> calls = new List<UnityEngine.Events.UnityAction>();

    // Update is called once per frame
    void Update () {
        currentPlayer = game.getCurrentPlayer();
        currentCity = currentPlayer.getPlayerPawn().getCity();
	}

    public void colonelSkillButtonClicked()
    {
        List<City> cities = game.getCities();
        for(int i = 0; i<cities.Count; i++)
        {
            City thisCity = cities[i];
            UnityEngine.Events.UnityAction thisCall = () => colonelSelectCard(thisCity);
            thisCity.gameObject.GetComponent<Button>().onClick.AddListener(thisCall);
            calls.Add(thisCall);
            if (thisCity.getMarker() == 0)
            {
                thisCity.displayButton();
            }
        }
    }

    public void colonelSelectCard(City colonelCity)
    {
        List<City> cities = game.getCities();
        for (int i = 0; i < cities.Count; i++)
        {
            City thisCity = cities[i];
            thisCity.gameObject.GetComponent<Button>().onClick.RemoveListener(calls[i]);
            thisCity.undisplayButton();
        }

    }
}
