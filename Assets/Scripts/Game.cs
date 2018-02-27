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
    private List<PlayerCard> playerCardDeck = new List<PlayerCard>();
    private Dictionary<Enums.DiseaseColor, Disease> diseases = new Dictionary<Enums.DiseaseColor, Disease>();


    public Game(int numOfPlayer, int nEpidemicCard, List<User> users) {
        Maps mapInstance = Maps.getInstance();

        players = new List<Player>(numOfPlayer);
        numOfEpidemicCard = nEpidemicCard;
        foreach (User u in users)
        {
            players.Add(new Player(u));
        }

        List<Enums.CityName> cityNames = Maps.getInstance().getCityNames();

        foreach (Enums.CityName name in cityNames)
        {
            City c = new City(name);
            c.setCityColor(mapInstance.getCityColor(name));
            cities.Add(c);
            playerCardDeck.Add(new CityCard(c));
            infectionDeck.Add(new InfectionCard(c));
        }

        foreach (City c in cities)
        {
            List<Enums.CityName> neighborNames = mapInstance.getNeighbors(c.getCityName());
            foreach (Enums.CityName name in neighborNames)
            {
                c.addNeighbor(findCity(name));
            }
        }

        List<Enums.EventKind> eventKinds = mapInstance.getEventNames();
        foreach (Enums.EventKind k in eventKinds)
        {
            playerCardDeck.Add(new EventCard(k));
        }
        //TO-DO implement shuffle well
       // shuffleAndAddEpidemic(numOfEpidemicCard);

        foreach(Player p in players)
        {
            Enums.RoleKind rk = selectRole();
            Role r = new Role(rk);
            p.setRole(r);
            Pawn pawn = new Pawn(rk);
            r.setPawn(pawn);
        }
        List<Enums.DiseaseColor> dc = mapInstance.getDiseaseColor();
        foreach (Enums.DiseaseColor c in dc)
        {
            Disease d = new Disease(c);
            diseases.Add(c, d);
            
        }

        setUp();
    }
    //TO-DO here
    public Enums.RoleKind selectRole()
    {
        return Enums.RoleKind.Archivist;
    }

    private void setUp()
    {
        City Atlanta = findCity(Enums.CityName.Atlanta);
        Atlanta.setHasResearch(true);
        foreach(Player p in players)
        {
            Atlanta.addPawn(p.getRole().getPawn());
        }
        
        for(int i = 3; i > 0; i--)
        {
            InfectionCard ic=infectionDeck[0];
            infectionDeck.Remove(infectionDeck[0]);
            infectionDiscardPile.Add(ic);
            infectionCardDrawn++;
            City c2=ic.getCity();
            //TO-DO HERE has some difference compared to original design


        }
    }
    private City findCity(Enums.CityName cityname)
    {
        foreach (City c in cities)
        {
            if (c.getCityName() == cityname)
            {
                return c;
            }
        }

        return null;
    }

    //need correction, may be errors, so sorry I forgot to write on sequence diagram, need shuffle into here.
    /*private void shuffleAndAddEpidemic(int numOfEpidemicCard)
    {
        Shuffle(playerCardDeck);
        

        for (int i = 0; i < numOfEpidemicCard; i++)
        {
            playerCardDeck.Add(new EpidemicCard());
        }

    }*/
    //need correction, may be errors
  /*  public void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
    */
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

	//since i am familiar with end turn method I am going to do this first, might be buggy --zhening

	/*
		endTurn
	*/
	public void endTurn(){	
		if (currentPhase != Enums.GamePhase.PlayerTakeTurn)
			return;
		currentPhase = Enums.GamePhase.Completed;

		currentPlayer.refillAction ();
		currentPlayer.setOncePerturnAction (false);

		int playerCardDeckSize = playerCardDeck.Count;

		//if there is no enough player cards in the deck, players lose the game
		if (playerCardDeckSize < 2) {
			currentPhase = Enums.GamePhase.Completed;
			notifyGameLost();
			return;
		}

		//Question: what if the cards exceed the player's hand limit?

	}

	/*
		infect specified city with specified disease
		@city the city to be infected
		@color the color of the specified diesase
		@number the number of cubes to be put in this city
	*/
	private void infect(City city, Enums.DiseaseColor color,int number ){
		Disease disease = diseases [color];
		bool hasMedic = city.contains (Enums.RoleKind.Medic);
		bool hasQS = city.contains(Enums.RoleKind.QuarantineSpecialist);
		bool isEradicated = disease.isEradicated ();

		List<City> neighbors = city.getNeighbors ();
		foreach (City neighbor in neighbors) {
			if ( neighbor.contains(Enums.RoleKind.QuarantineSpecialist) ){
				hasQS = true;
				break;
			}
		}

		if (!hasQS && !hasMedic && !isEradicated) {
			
		}
	}
	/*
		draw two cards from the top of the player card deck
	*/
	private List<PlayerCard> draw(){
		if (playerCardDeck.Count < 2) {
			Debug.Log ("there is no enough cards in the deck");
			return null;
		}

		List<PlayerCard> cards = new List<PlayerCard>();
		for (int i = 0; i < 1; i++) {
			cards.Add(playerCardDeck [0]);
			playerCardDeck.RemoveAt [0];
		}

		return cards;	
	}
	//my part ends here

	// to do: inform the player that they lose the game
	private void notifyGameLost(){
	}

	// to do: inform the player that they win the game
	private void notifyGameWin(){
	}

	//to do: inform the player that 
	private void notifyExceedLimit(){
		
	}
}
