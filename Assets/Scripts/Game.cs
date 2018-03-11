using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	#region private variables
    private readonly int MAX = 24;
    private Challenge challenge;
    private GamePhase currentPhase;
    private bool hasDLC;
    private int infectionRate=2;
    private int[] infectionArray;
    private int infectionCardDrawn;
    private bool isnewGame;
    private int outbreaksValue = 0;
    private int researchStationRemain;
    private bool resolvingEpidemic;
    private int numOfEpidemicCard;
    private int index = 0;
    private int difficulty;
	private readonly int maxOutbreaksValue = 8;
    private Player currentPlayer;
    private List<Player> players;
    private List<InfectionCard> infectionDeck = new List<InfectionCard>();
    private List<InfectionCard> infectionDiscardPile = new List<InfectionCard>();
    private List<City> outbreakedCities = new List<City>();
    
    private List<PlayerCard> playerCardDeck = new List<PlayerCard>();
    private List<PlayerCard> playerDiscardPile = new List<PlayerCard>();
    private Dictionary<Color, Disease> diseases = new Dictionary<Color, Disease>();
    #endregion

    public List<City> cities;
    public int numOfPlayer;
    public int nEpidemicCard;
    public Pawn prefab;
    public GameInfoDisplay gameInfoController;

    private void Start()
    {

        Maps mapInstance = Maps.getInstance();

        players = new List<Player>(numOfPlayer);
        numOfEpidemicCard = nEpidemicCard;
        difficulty = nEpidemicCard;
        Player me = new Player(new User("Jack", "111"));
        players.Add(me);
        currentPlayer = me;
        for(int i = 0; i< numOfPlayer-1; i++)
        {
            players.Add(new Player(new User("others", "2222")));
        }

        foreach(City c in cities)
        {
            playerCardDeck.Add(new CityCard(c));
            infectionDeck.Add(new InfectionCard(c));
        }
        List<EventKind> eventKinds = mapInstance.getEventNames();
        foreach (EventKind k in eventKinds)
        {
            playerCardDeck.Add(EventCard.getEventCard(k));
        }
        //TO-DO implement shuffle well
        // shuffleAndAddEpidemic(numOfEpidemicCard);

        foreach (Player p in players)
        {
            RoleKind rk = selectRole();
            Role r = new Role(rk);
            Pawn pawn = Instantiate(prefab, new Vector3(0, 0, 100), gameObject.transform.rotation);
            pawn.transform.parent = GameObject.FindGameObjectWithTag("background").transform;
            
            r.setPawn(pawn);
            p.setRole(r);
        }
        List<Color> dc = mapInstance.getDiseaseColor();
        foreach (Color c in dc)
        {
            Disease d = new Disease(c);
            diseases.Add(c, d);
        }

        gameInfoController.displayOutbreak();
        gameInfoController.displayInfectionRate();
        setInitialHand();
        shuffleAndAddEpidemic();
        setUp();
        currentPhase = GamePhase.PlayerTakeTurn;
        Debug.Log("Everything Complete");
    }

    public RoleKind selectRole()
    {
        return (RoleKind)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(RoleKind)).Length));
    }

    private void setUp()
    {
        City Atlanta = findCity(CityName.Atlanta);
        Atlanta.setHasResearch(true);
        foreach(Player p in players)
        {
            Atlanta.addPawn(p.getRole().getPawn());
        }
        Collection.Shuffle<InfectionCard>(infectionDeck);
        for(int i = 3; i > 0; i--)
        {
            for (int j = 3; j > 0; j--)
            {
                InfectionCard ic = infectionDeck[0];
                infectionDeck.Remove(infectionDeck[0]);
                infectionDiscardPile.Add(ic);
                infectionCardDrawn++;
                City c2 = ic.getCity();
                infect(c2, c2.getColor(), i);
            }
        }
    }

    public void setInitialHand()
    {
        int numOfPlayers = players.Count;
        int cardNeeded = numOfPlayers;
        if (numOfPlayers != 3)
        {
            cardNeeded = (numOfPlayers == 2) ? 4 : 2;
        }
        foreach (Player p in players)
        {
            draw(p, cardNeeded);
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

    private void shuffleAndAddEpidemic()
    {
        Collection.Shuffle(playerCardDeck);
        int subDeckSize = playerCardDeck.Count / difficulty;
        int lastSubDeckSize = playerCardDeck.Count - (difficulty - 1) * subDeckSize;

        List<PlayerCard> tempList = new List<PlayerCard>();

        int start = 0;
        for (int i=0; i<difficulty; i++)
        {
            if(i == difficulty - 1)
            {
                subDeckSize = lastSubDeckSize;
            }
            List<PlayerCard> temp = new List<PlayerCard>();
            temp = playerCardDeck.GetRange(start, subDeckSize);
            temp.Add(EpidemicCard.getEpidemicCard());
            Collection.Shuffle<PlayerCard>(temp);
            tempList.AddRange(temp);
            start += subDeckSize;
        }

        playerCardDeck = tempList;
    }
		
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
		//Note that epidemic card is resolved in "draw" method
        //if there is no enough player cards in the deck, players lose the game
        if (!draw(currentPlayer, 2))
        {
            return;
        }

		setGamePhase (GamePhase.InfectCities);
        infectCity();

        //Question: what if the cards exceed the player's hand limit?

    }

    private bool resolveEpidemic()
    {
        resolvingEpidemic = true;
        if(infectionRate < 4)
        {
            infectionRate = infectionArray[++index];
        }

        InfectionCard infectionCard = drawBottomInfectionDeck();

        City city = infectionCard.getCity();

        Color color = infectionCard.getColor();

        Disease disease = diseases[color];

        infectionDiscardPile.Add(infectionCard);

        outbreakedCities.Clear();

        if (!infect(city, color, 3))
        {
            return false;
        }

        Collection.Shuffle<InfectionCard>(infectionDiscardPile);
        placeInfectionDiscardPileOnTop();

        resolvingEpidemic = false;

        return true;
    }

    private void placeInfectionDiscardPileOnTop()
    {
        foreach(InfectionCard card in infectionDiscardPile)
        {
            infectionDeck.Insert(0, card);
        }

        infectionDiscardPile.Clear();
    }

    private InfectionCard drawBottomInfectionDeck()
    {
        InfectionCard card = infectionDeck[infectionDeck.Count - 1];
        infectionDeck.Remove(card);
        return card;
    }

    public Disease getDisease(Color color)
    {
        return diseases[color];
    }

    /*
		infect specified city with specified disease
		@city the city to be infected
		@color the color of the specified diesase
		@number the number of cubes to be put in this city
	*/
    private bool infect(City city, Color color, int number)
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

        if (hasQS || hasMedic || isEradicated) return true;

        outbreakedCities.Add(city);
        int cubeNumber = city.getCubeNumber(color);
        int remainingCubes = disease.getNumOfDiseaseCubeLeft();
        //if not exceeding 3 cubes, put cubes to that city
        if (cubeNumber <= 3)
        {
            //check if there is enough cubes left 
            if (remainingCubes - number < 0)
            {
                notifyGameLost(GameLostKind.RunOutOfDiseaseCube);
                //setGamePhase (GamePhase.Completed);
                return false;
            }
            city.addCubes(disease, number);
            disease.removeCubes (number);
            return true;
        }
        //else there will be an outbreak
        else
        {
            outbreaksValue++;
            gameInfoController.displayOutbreak();
            if (outbreaksValue == maxOutbreaksValue)
            {
                notifyGameLost(GameLostKind.MaxOutbreakAmountReached);
                //setGamePhase (GamePhase.Completed);
                return false;
            }

            if (remainingCubes - (3 - cubeNumber) < 0)
            {
                notifyGameLost(GameLostKind.RunOutOfDiseaseCube);
                //setGamePhase (GamePhase.Completed);
                return false;
            }

            city.addCubes(disease, 3 - cubeNumber);
			disease.removeCubes ( 3 - cubeNumber);

			foreach (City neighbor in neighbors) {
				if (outbreakedCities.Contains (neighbor))
					continue;
                if (!infect(neighbor, color, 1)) {
                    return false;
                };
			}

			return true;
        }
    }
    /*
		draw two cards from the top of the player card deck
		if there is a epidemic card, it will be resolved
		else it will be added to player's hand
		@player the player who draws card
		@count the number of card the player is drawing
	*/
    private bool draw(Player player, int count)
    {
        if (playerCardDeck.Count < count)
        {
            notifyGameLost(GameLostKind.RunOutOfPlayerCard);
            setGamePhase(GamePhase.Completed);
            return false;
        }
        
        for (int i = 0; i < count; i++)
        {
            PlayerCard card = playerCardDeck[0];
            playerCardDeck.RemoveAt(0);
            if (card.getType() == CardType.EpidemicCard)
            {
                resolveEpidemic();
            }
            else
            {
                player.addCard(card);
            }
            
        }

        return true;
    }

    private void setGamePhase(GamePhase gamePhase)
    {
        currentPhase = gamePhase;
    }

    //my part ends here

    public void infectNextCity()
    {
        InfectionCard card = infectionDeck[0];
        infectionDeck.Remove(card);
        infectionDiscardPile.Add(card);
        City city = card.getCity();
        Color color = card.getColor();
        Disease disease = diseases[color];
        outbreakedCities.Clear();
        if (!infect(city, color, 1))
        {
            return;
        }
    }

    public void infectCity()
    {
        for(int i=0; i<infectionRate; i++)
        {
            infectNextCity();
        }
        nextPlayer();
    }

    public Player getCurrentPlayer()
    {
        return currentPlayer;
    }

    public List<Player> getPlayers()
    {
        List<Player> copy = new List<Player>(players);
        return copy;
    }

    public void nextPlayer()
    {
        currentPlayer = players[(players.IndexOf(currentPlayer) + 1) % (players.Count)];
    }

	#region notify methods
    // to do: inform the player that they lose the game
    private void notifyGameLost(GameLostKind lostKind)
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
    #endregion

    public int getOutbreakRate()
    {
        return outbreaksValue;
    }

    public int getInfectionRate()
    {
        return infectionRate;
    }

    public void drive(Player player, City destinationCity)
    {
        Pawn p = player.getPlayerPawn();
        City initialCity = p.getCity();
        p.setCity(destinationCity);
        initialCity.removePawn(p);
        destinationCity.addPawn(p);
        RoleKind rolekind = player.getRoleKind();

        if (rolekind == RoleKind.Medic)
        {
            foreach (Disease disease in diseases.Values)
            {
                if (disease.isCured())
                {
                    int cubeNumber = destinationCity.getCubeNumber(disease);
                    destinationCity.removeCubes(disease, cubeNumber);
                    disease.addCubes(cubeNumber);
                    int num = disease.getNumOfDiseaseCubeLeft();
                    if (num == MAX)
                    {
                        disease.eradicate();
                    }
                }

            }
        }

        else if (rolekind == RoleKind.ContainmentSpecialist)
        {
            foreach (Disease disease in diseases.Values)
            {
                int cubeNumber = destinationCity.getCubeNumber(disease);
                if (cubeNumber > 1)
                {
                    destinationCity.removeCubes(disease, 1);
                    disease.addCubes(1);
                }
            }
        }
        player.decreaseRemainingAction();

        //UI only

    }

    public void takeDirectFlight(Player player, CityCard card)
    {
        Pawn p = player.getPlayerPawn();
        City initialCity = p.getCity();
        City destinationCity = card.getCity();
        p.setCity(destinationCity);
        initialCity.removePawn(p);
        destinationCity.addPawn(p);
        RoleKind rolekind = player.getRoleKind();
        if(rolekind != RoleKind.Troubleshooter)
        {
            player.removeCard(card);
            playerDiscardPile.Add(card);
            
        }
        if(rolekind == RoleKind.Medic)
        {
            foreach (Disease disease in diseases.Values)
            {
                if (disease.isCured())
                {
                    int cubeNumber = destinationCity.getCubeNumber(disease);
                    destinationCity.removeCubes(disease, cubeNumber);
                    disease.addCubes(cubeNumber);
                    int num = disease.getNumOfDiseaseCubeLeft();
                    if(num == MAX)
                    {
                        disease.eradicate();
                    }
                }
                
            }
        }
        else if(rolekind == RoleKind.ContainmentSpecialist)
        {
            foreach(Disease disease in diseases.Values)
            {
                int cubeNumber = destinationCity.getCubeNumber(disease);
                if(cubeNumber > 1)
                {
                    destinationCity.removeCubes(disease, 1);
                    disease.addCubes(1);
                }
            }
        }
        player.decreaseRemainingAction();

        //UI only
    }

    public void treatDisease(Disease d, City currentCity)
    {
        RoleKind rolekind = currentPlayer.getRoleKind();
        bool isCured = d.isCured();
        int treatNumber = 1;
        if(rolekind == RoleKind.Medic||isCured == true)
        {
            int n = currentCity.getCubeNumber(d);
            treatNumber = n;
        }
        currentCity.removeCubes(d, treatNumber);
        d.addCubes(treatNumber);
        int num = d.getNumOfDiseaseCubeLeft();
        if(num == MAX && isCured == true)
        {
            d.isEradicated();
        }

        currentPlayer.decreaseRemainingAction();
    }


    public void build(CityCard card)
    {
        RoleKind rolekind = currentPlayer.getRoleKind();
        Pawn p = currentPlayer.getPlayerPawn();
        City currentCity = p.getCity();

        if(researchStationRemain == 0)
        {
            //Todo: ask player to remove one Research Station   
        }

        if(rolekind != RoleKind.OperationsExpert)
        {
            currentPlayer.removeCard(card);
            playerDiscardPile.Add(card);
        }
        
        currentCity.setHasResearch(true);
        researchStationRemain--;

        currentPlayer.decreaseRemainingAction();
    }


    public void share(Player targetPlayer, CityCard card, bool giveOrTake){
        
        bool permission = true; //TODO: ask targetPlayer for permission
        
        if(permission){
            if (giveOrTake){
                currentPlayer.removeCard(card);
                targetPlayer.addCard(card);
            }
            else{
                targetPlayer.removeCard(card);
                currentPlayer.addCard(card);
            }
        }

        currentPlayer.decreaseRemainingAction();
    }

    public void cure(Disease d)
    {
        List<CityCard> cardsToRemove = new List<CityCard>(); //TODO: ask player to choose 5 cards of same color

        foreach (CityCard card in cardsToRemove)
        {
            currentPlayer.removeCard(card);
            playerDiscardPile.Add(card);
        }

        d.cure();
        int num = d.getNumOfDiseaseCubeLeft();
        if(num == MAX)
        {
            d.isEradicated();
        }

        //UI TODO: set disease’s cure marker

        currentPlayer.decreaseRemainingAction();
    }
    
    




}
