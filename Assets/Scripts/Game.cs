using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	private Enums.Challenge challenge;
	private Enums.GamePhase currentPhase;
	private boolean hasDLC;
	private int infectionRate;
	private int[] infectionArray;
	private int infectionCardDrawn;
	private boolean isnewGame;
	private int outbreaksValue;
	private int researchStationRemain;
	private boolean resolvingEpidemic;
	private numOfEpidemicCard;

	private Player currentPlayer;
	private List<Player> players;
	private List<InfectionCard> infectionDeck = new List<InfectionCard>();
	private List<InfectionCard> infectionDiscardPile = new List<InfectionCard>();
	private List<City> outbreakedCities = new List<City>();
	private List<City> cities = new List<City>();

	private Dictionary<Enums.DeseaseColor, Disease> disease = new Dictionary<Enums.DeseaseColor, Disease> ();

	public Game(int numOfPlayer, int nEpidemicCard, List<User> users){
		players = new List<Player>(numOfPlayer);
		numOfEpidemicCard = nEpidemicCard;
		foreach (User u in users)
		{
			players.add(new Player(u));
		}

		
	}


}
