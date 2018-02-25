using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {



    private Enums.Challenge challenge;
	private Enums.GamePhase currentPhase;
	private bool hasDLC;
	private int infectionRate;
	private int[] infectionArray;
	private int infectionCardDrawn;
	private bool isnewGame;
	private int outbreaksValue;
	private int researchStationRemain;
	private bool resolvingEpidemic;
	private int numOfEpidemicCard;

	private Player currentPlayer;
	private List<Player> players;
	private List<InfectionCard> infectionDeck = new List<InfectionCard>();
	private List<InfectionCard> infectionDiscardPile = new List<InfectionCard>();
	private List<City> outbreakedCities = new List<City>();
	private List<City> cities = new List<City>();

	private Dictionary<Enums.DiseaseColor, Disease> disease = new Dictionary<Enums.DiseaseColor, Disease> ();

	public Game(int numOfPlayer, int nEpidemicCard, List<User> users){
		players = new List<Player>(numOfPlayer);
		numOfEpidemicCard = nEpidemicCard;
		foreach (User u in users)
		{
			players.Add(new Player(u));
		}

		
	}



    /// <summary>
    /// All below values and operations are only used in the client system. 
    /// </summary>
    public Button drive;
    public Button directFlight;
    public Button shuttleFlight;
    public Button charterFlight;
    public Button cancel;

    public void moveButtonClicked()
    {
        drive.GetComponent<Button>().interactable = true;
        directFlight.GetComponent<Button>().interactable = false;
        shuttleFlight.GetComponent<Button>().interactable = false;
        charterFlight.GetComponent<Button>().interactable = false;
        cancel.GetComponent<Button>().interactable = true;


    }
    public City currentCity;
    public void driveButtonClicked()
    {
        currentCity.setButtonActive();
    }

}
