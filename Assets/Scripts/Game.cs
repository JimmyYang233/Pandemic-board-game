using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    private Challenge challenge;
    private GamePhase currentPhase;
    private bool hasDLC;
    private int infectionRate;
    private int[] infectionArray;
    private int infectionCardDrawn;
    private bool isnewGame;
    private int outbreaksValue;
    private int researchStationRemain;
    private bool resolvingEpidemic;
    private int numOfEpidemicCard;
	private readonly int maxOutbreaksValue = 8;
    private Player currentPlayer;
    private List<Player> players;
    private List<InfectionCard> infectionDeck = new List<InfectionCard>();
    private List<InfectionCard> infectionDiscardPile = new List<InfectionCard>();
    private List<City> outbreakedCities = new List<City>();
    private List<City> cities = new List<City>();
    private List<PlayerCard> playerCardDeck = new List<PlayerCard>();
    private Dictionary<Color, Disease> diseases = new Dictionary<Color, Disease>();


    public Game(int numOfPlayer, int nEpidemicCard, List<User> users) {
        Maps mapInstance = Maps.getInstance();

        players = new List<Player>(numOfPlayer);
        numOfEpidemicCard = nEpidemicCard;
        foreach (User u in users)
        {
            players.Add(new Player(u));
        }

        List<CityName> cityNames = Maps.getInstance().getCityNames();

        foreach (CityName name in cityNames)
        {
            City c = new City(name);
            c.setCityColor(mapInstance.getCityColor(name));
            cities.Add(c);
            playerCardDeck.Add(new CityCard(c));
            infectionDeck.Add(new InfectionCard(c));
        }

        foreach (City c in cities)
        {
            List<CityName> neighborNames = mapInstance.getNeighbors(c.getCityName());
            foreach (CityName name in neighborNames)
            {
                c.addNeighbor(findCity(name));
            }
        }

        List<EventKind> eventKinds = mapInstance.getEventNames();
        foreach (EventKind k in eventKinds)
        {
            playerCardDeck.Add(new EventCard(k));
        }
        //TO-DO implement shuffle well
       // shuffleAndAddEpidemic(numOfEpidemicCard);

        foreach(Player p in players)
        {
            RoleKind rk = selectRole();
            Role r = new Role(rk);
            p.setRole(r);
            Pawn pawn = new Pawn(rk);
            r.setPawn(pawn);
        }
        List<Color> dc = mapInstance.getDiseaseColor();
        foreach (Color c in dc)
        {
            Disease d = new Disease(c);
            diseases.Add(c, d);
            
        }

        setUp();
    }
    //TO-DO here
    public RoleKind selectRole()
    {
        return RoleKind.Archivist;
    }

    private void setUp()
    {
        City Atlanta = findCity(CityName.Atlanta);
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
    private City findCity(CityName cityname)
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
    //since i am familiar with end turn method I am going to do this first, might be buggy --zhening

    /*
		endTurn
	*/

    public void endTurn()
    {
        if (currentPhase != GamePhase.PlayerTakeTurn)
            return;
        currentPhase = GamePhase.Completed;

        currentPlayer.refillAction();
        currentPlayer.setOncePerturnAction(false);

        int playerCardDeckSize = playerCardDeck.Count;

        //if there is no enough player cards in the deck, players lose the game
        if (playerCardDeckSize < 2)
        {
            notifyGameLost();
            //setGamePhase (GamePhase.Completed);
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
    private void infect(City city, Color color, int number)
    {
        Disease disease = diseases[color];
        bool hasMedic = city.contains(RoleKind.Medic);
        bool hasQS = city.contains(RoleKind.QuarantineSpecialist);
        bool isEradicated = disease.isEradicated();

        List<City> neighbors = city.getNeighbors();
        foreach (City neighbor in neighbors)
        {
            if (neighbor.contains(RoleKind.QuarantineSpecialist))
            {
                hasQS = true;
                break;
            }
        }

        if (hasQS || hasMedic || isEradicated) return;

        outbreakedCities.Add(city);
        int cubeNumber = city.getCubeNumber(color);
        int remainingCubes = disease.getNumOfDiseaseCubeLeft();
        //if not exceeding 3 cubes, put cubes to that city
        if (cubeNumber + remainingCubes <= 3)
        {
            //check if there is enough cubes left 
            if (remainingCubes - number < 0)
            {
                notifyGameLost();
                //setGamePhase (GamePhase.Completed);
                return;
            }
            city.addCubes(disease, number);
            disease.removeCubes (number);
        }
        //else there will be an outbreak
        else
        {
            outbreaksValue++;
            if (outbreaksValue == maxOutbreaksValue)
            {
                notifyGameLost();
                //setGamePhase (GamePhase.Completed);
                return;
            }

            if (remainingCubes - (3 - cubeNumber) < 0)
            {
                notifyGameLost();
                //setGamePhase (GamePhase.Completed);
                return;
            }

            city.addCubes(disease, 3 - cubeNumber);
			disease.removeCubes ( 3 - cubeNumber);

			foreach (City neighbor in neighbors) {
				if (outbreakedCities.Contains (neighbor))
					continue;
				infect (neighbor, color, 1);
			}

			return;
        }
    }
    /*
		draw two cards from the top of the player card deck
	*/
    private List<PlayerCard> draw()
    {
        if (playerCardDeck.Count < 2)
        {
            Debug.Log("there is no enough cards in the deck");
            return null;
        }

        List<PlayerCard> cards = new List<PlayerCard>();
        for (int i = 0; i < 1; i++)
        {
            cards.Add(playerCardDeck[0]);
            playerCardDeck.RemoveAt(0);
        }

        return cards;
    }

    private void setGamePhase(GamePhase gamePhase)
    {
        currentPhase = gamePhase;
    }

    //my part ends here

    // to do: inform the player that they lose the game
    private void notifyGameLost()
    {
        setGamePhase(GamePhase.Completed);
    }

    // to do: inform the player that they win the game
    private void notifyGameWin()
    {
    }

    //to do: inform the player that handcards exceed the limit
    private void notifyExceedLimit()
    {

    }

    public void moveTo(Player player, City finalCity)
    {
        Pawn pawn = player.getPlayerPawn();
        City initialCity = pawn.getCity();
        initialCity.removePawn(pawn);
        finalCity.addPawn(pawn);
        pawn.setCity(finalCity);

        //This part are only for the GUI, when Server side use this code, don't forget to remove this

        Vector3 position = finalCity.transform.position;
        position.y = position.y + 10;
        pawn.transform.position = position;
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
    //public City tempCity;
    public void cancelButtonClicked()
    {
        //This part is suppose to be in the game constructor, but it conflict with the current game onstructor
         GameObject[] tmp = GameObject.FindGameObjectsWithTag("City");
         foreach(GameObject aObject in tmp)
         {
            Button button = aObject.GetComponent<Button>();
            if (button.interactable)
            {
                button.interactable = false;
            }
         }
    }
    public City currentCity;
    public void driveButtonClicked()
    {
        foreach(City neighbor in currentCity.getNeighbors())
        {
            neighbor.displayButton();
        }
    }

    public Pawn testPawn; 
    public void testMovePawn(City destinationCity)
    {
        currentCity = destinationCity;
        Vector3 position = destinationCity.transform.position;
        position.y = position.y + 10; 
        testPawn.transform.position = position;
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
