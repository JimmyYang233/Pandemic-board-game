using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveOperation : MonoBehaviour {
    public Button driveButton;
    public Button directFlightButton;
    public Button shuttleFlightButton;
    public Button charterFlightButton;
    public Button cancelButton;
    
    public Game game;

    Player currentPlayer;


    public void moveButtonClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        driveButton.GetComponent<Button>().interactable = true;
        City currentCity = currentPlayer.getPlayerPawn().getCity();

        if (currentPlayer.containsCityCard())
        {
            directFlightButton.GetComponent<Button>().interactable = true;
        }
        if (currentPlayer.containsSpecificCityCard(currentCity))
        {
            charterFlightButton.GetComponent<Button>().interactable = true;
        }
        if (currentCity.getHasResearch())
        {
            shuttleFlightButton.GetComponent<Button>().interactable = true;
        }
        cancelButton.GetComponent<Button>().interactable = true;
    }

    public void cancelButtonClicked()
    {
        disableAllCities();
    }
   // public City tmpCity;
    public void driveButtonClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        Debug.Log(currentCity.getCityName());
        foreach (City neighbor in currentCity.getNeighbors())
        {
            Debug.Log(neighbor.getCityName());
            neighbor.displayButton();
        }
    }

    public void directFlightButtonClicked()
    {
        currentPlayer = game.getCurrentPlayer();
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        List<PlayerCard> cards = currentPlayer.getHand();
        foreach(CityCard card in cards)
        {
            City city = card.getCity();
            if (city != currentCity)
            {
                city.displayButton();
            }
        }
    }

    public static void disableAllCities()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("City");
        foreach (GameObject aObject in tmp)
        {
            Button button = aObject.GetComponent<Button>();
            if (button.interactable)
            {
                button.interactable = false;
            }
        }
    }
    /**
        public void testMovePawn(City destinationCity)
        {
            City currentCity = currentPlayer.getPlayerPawn().getCity();
            Vector3 position = destinationCity.transform.position;
            position.y = position.y + 10;
            testPawn.transform.position = position;
            disableAllCities();
        }

        
        **/
}
