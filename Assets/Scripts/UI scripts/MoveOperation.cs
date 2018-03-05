using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOperation : MonoBehaviour {
    public Button driveButton;
    public Button directFlightButton;
    public Button shuttleFlightButton;
    public Button charterFlightButton;
    public Button cancelButton;
    
    public Game game;

    public void moveButtonClicked()
    {
        testRole.setPawn(testPawn);
        testCity.addPawn(testPawn);
        //testPlayer.setRole(testRole);
        //testPlayer.addCard(new CityCard(testCity));
        // testPlayer.addCard(new CityCard(destinationCity));
        //currentPlayer = testPlayer;
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

        /// <summary>
        /// All below values and operations are only used in the client system. 
        /// </summary>
   


    //This part for test only
    void Start()
    {

        Debug.Log("Start");

    }


    //All below may stay here forever

}
//public City tempCity;
public void cancelButtonClicked()
{
    disableAllCities();
}

public void driveButtonClicked()
{
    City currentCity = currentPlayer.getPlayerPawn().getCity();
    foreach (City neighbor in currentCity.getNeighbors())
    {
        neighbor.displayButton();
    }
}


public void testMovePawn(City destinationCity)
{
    City currentCity = currentPlayer.getPlayerPawn().getCity();
    Vector3 position = destinationCity.transform.position;
    position.y = position.y + 10;
    testPawn.transform.position = position;
    disableAllCities();
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
    }
