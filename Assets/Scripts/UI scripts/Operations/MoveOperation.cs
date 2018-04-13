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
    public playerSelectionPanel playerSelect;

    Game game;

    Player currentPlayer;
    Player playerToMove;

    private enum Status {DRIVE, DIRECTFLIGHT, SHUTTLEFLIGHT, CHARTERFLIGHT, NULL};

    private Status moveStatus = Status.NULL; 

    void Start()
    {
        //game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
		game = Game.Instance;
    }

    public void moveButtonClicked()
    {
        if (currentPlayer.getRoleKind() == RoleKind.Dispatcher)
        {
            playerSelect.gameObject.SetActive(true);
            playerSelect.selectStatus = playerSelectionPanel.Status.DISPATCHER;
        }
        else
        {
            showMove();
        }
    }

    public void showMove()
    {
        currentPlayer = game.getCurrentPlayer();
        playerToMove = currentPlayer;
        driveButton.GetComponent<Button>().interactable = true;
        City currentCity = playerToMove.getPlayerPawn().getCity();

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
            if (game.getRemainingResearch() < 5)
            {
                shuttleFlightButton.GetComponent<Button>().interactable = true;
            }
            else if (currentPlayer.getRoleKind() == RoleKind.OperationsExpert && currentPlayer.containsCityCard())
            {
                shuttleFlightButton.GetComponent<Button>().interactable = true;
            }

        }
        cancelButton.GetComponent<Button>().interactable = true;
    }

    public void cancelButtonClicked()
    {
        disableAllCities();
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        cancelButton.GetComponent<Button>().interactable = false;
    }
   // public City tmpCity;
    public void driveButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = playerToMove.getPlayerPawn().getCity();
        //Debug.Log(currentCity.getCityName());
        foreach (City neighbor in currentCity.getNeighbors())
        {
            //Debug.Log(neighbor.getCityName());
            neighbor.displayButton();
        }
        moveStatus = Status.DRIVE;
    }

    public void directFlightButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = playerToMove.getPlayerPawn().getCity();
        List<PlayerCard> cards = currentPlayer.getHand();
        foreach(PlayerCard card in cards)
        {
            if(card.getType() == CardType.CityCard)
            {
                CityCard aCard = (CityCard)card;
                City city = aCard.getCity();
                if (city != currentCity)
                {
                    city.displayButton();
                }
            }
            
        }

        moveStatus = Status.DIRECTFLIGHT;
    }

    public void charterFlightButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = playerToMove.getPlayerPawn().getCity();
        foreach (City city in game.getCities())
        {
            if (city != currentCity)
            {
                city.displayButton();
            }
        }

        moveStatus = Status.CHARTERFLIGHT;
    }

    public void shuttleFlightButtonClicked()
    {
        driveButton.GetComponent<Button>().interactable = false;
        directFlightButton.GetComponent<Button>().interactable = false;
        charterFlightButton.GetComponent<Button>().interactable = false;
        shuttleFlightButton.GetComponent<Button>().interactable = false;
        currentPlayer = game.getCurrentPlayer();
        City currentCity = playerToMove.getPlayerPawn().getCity();
        foreach (City city in game.getCities())
        {
            if (((city != currentCity)&&(city.getHasResearch())||(currentPlayer.getRoleKind() == RoleKind.OperationsExpert && currentPlayer.containsSpecificCityCard(city))))
            {
                city.displayButton();
            }
        }

        moveStatus = Status.SHUTTLEFLIGHT;
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
    

    public void cityButtonClicked(City destinationCity)
    {
        currentPlayer = game.getCurrentPlayer();
        if(moveStatus == Status.DRIVE)
        {
			game.Drive(playerToMove.getRoleKind().ToString(), destinationCity.cityName.ToString());
        }
        else if(moveStatus == Status.DIRECTFLIGHT)
        {
			game.TakeDirectFlight(playerToMove.getRoleKind().ToString(), currentPlayer.getCard(destinationCity).getCity().getCityName().ToString());
        }
        else if(moveStatus == Status.CHARTERFLIGHT)
        {
            game.TakeCharterFlight(playerToMove.getRoleKind().ToString(), destinationCity.cityName.ToString());
        }
        else if(moveStatus == Status.SHUTTLEFLIGHT)
        {
            game.TakeShuttleFlight(playerToMove.getRoleKind().ToString(), destinationCity.cityName.ToString());
        }
        moveStatus = Status.NULL;
        disableAllCities();
    }

    public void changePlayerToMove(string rolekind)
    {
        playerToMove = game.findPlayer(rolekind);
        showMove();
        playerSelect.selectStatus = playerSelectionPanel.Status.SHARE;
    }

    public void dispatcherMove()
    {
        //TO-DO a function call to player selection
    }

    public void selectCityWithPawn()
    {
        moveStatus = Status.DRIVE;
        foreach(Player player in game.getPlayers())
        {
            City city = player.getPlayerPawn().getCity();
            city.displayButton();
        }
    }

}
