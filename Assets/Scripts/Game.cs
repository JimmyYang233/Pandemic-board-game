using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public static Game Instance;
	public PhotonView PhotonView;
	#region private variables
    private readonly int MAX = 24;
    private Challenge challenge;
    private GamePhase currentPhase;
    private bool hasDLC;
    private int infectionRate=2;
    private int[] infectionArray;
    private int infectionCardDrawn= 0 ;
    private bool isnewGame = true;
    private int outbreaksValue = 0;
    private int researchStationRemain = 6;
    private bool resolvingEpidemic = false;
    private int numOfEpidemicCard;
    private int index = 0;
    private int difficulty;
	private readonly int maxOutbreaksValue = 8;
    private Player currentPlayer;
	private List<Player> players = new List<Player>();
    private List<RoleKind> roleKindTaken = new List<RoleKind>();
    private List<InfectionCard> infectionDeck = new List<InfectionCard>();
    private List<InfectionCard> infectionDiscardPile = new List<InfectionCard>();
    private List<City> outbreakedCities = new List<City>();
    
    private List<PlayerCard> playerCardDeck = new List<PlayerCard>();
    private List<PlayerCard> AllHandCards = new List<PlayerCard>();
    private List<PlayerCard> playerDiscardPile = new List<PlayerCard>();
    private Dictionary<Color, Disease> diseases = new Dictionary<Color, Disease>();
    #endregion
    //FOR GUI
    public PlayerPanelController playerPanel;
    public PCPanelController mainPlayerPanel;
	public playerSelectionPanel playerSelect;
	public GameInfoDisplay gameInfo;
	public ShareOperation shareOperation;


    GameObject backGround;

    private List<City> cities;
	private int numOfPlayer;
    Player me;
    public int nEpidemicCard;
    public Pawn prefab;
    public GameInfoDisplay gameInfoController;

	#region RPC method		
	[PunRPC]
	private void RPC_InitializePlayer(){
		Instance.InitializePlayer ();
	}

	[PunRPC]
	private void RPC_InitializeGame(){
		Instance.InitializeGame ();
	}

	[PunRPC]
	public void RPC_drive(string roleKind,string name){
		drive(getPlayer(roleKind),findCity(name));
	}

	[PunRPC]
	public void RPC_takeDirectFlight(string roleKind,string name){
		takeDirectFlight(getPlayer(roleKind),(CityCard)getPlayerCard(name));
	}

	[PunRPC]
	public void RPC_treatDisease(string color,string cityName){
		treatDisease(diseases[findColor(color)], findCity(cityName));
	}

	[PunRPC] 
	public void RPC_askForPermission(string cardName){
		askForPermisson (name);
	}

	[PunRPC]
	public void RPC_sendConsentResult(bool consentResult){
		informResponse (consentResult);
	}

	[PunRPC]
	public void RPC_endTurn(){
		endTurn ();
	}
	#endregion

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
		Debug.Log ("move succeed");
	}



	private void Awake(){
		
		if (Instance == null) {
			Instance = this;
			PhotonView = GetComponent<PhotonView> ();
		}

	}

    private void Start()
    {	
		if (PhotonNetwork.isMasterClient) {
			PhotonView.RPC ("RPC_InitializePlayer",PhotonTargets.All);
			PhotonView.RPC ("RPC_InitializeGame",PhotonTargets.All);
		}
			
    }

	public void Drive(string roleKind,string name){
		PhotonView.RPC ("RPC_drive",PhotonTargets.All,roleKind,name);
	}

	public void TakeDirectFlight(string roleKind,string name){
		PhotonView.RPC ("RPC_takeDirectFlight",PhotonTargets.All,roleKind,name);
	}

	//initialize player in the network, set the first player as current player
	private void InitializePlayer(){
		PhotonPlayer[] photonplayers = PhotonNetwork.playerList;
		Array.Sort (photonplayers, delegate(PhotonPlayer x, PhotonPlayer y) {
			return x.ID.CompareTo(y.ID);
		});
		foreach (PhotonPlayer player in photonplayers){
			players.Add (new Player(player));
		}
	}
		
	private void InitializeGame(){
		//load city
		cities = new List<City>();
		backGround = GameObject.FindGameObjectWithTag("background");
        backGround.transform.position += new Vector3(0.0001f, 0, 0);
        foreach (Transform t in backGround.transform)
		{
			if (t.GetComponent<City>() != null)
			{
				cities.Add(t.GetComponent<City>());
			}

		}

		Maps mapInstance = Maps.getInstance();
		//initialize infectionArray
		infectionArray = new int[]{2,2,2,3,3,4,4};


		numOfEpidemicCard = nEpidemicCard;
		difficulty = nEpidemicCard;
		me = FindPlayer(PhotonNetwork.player);

		//players.Add(me);
		currentPlayer = players[0];
		Debug.Log ("current player is player" + currentPlayer.PhotonPlayer.NickName);
		//for(int i = 0; i< numOfPlayer-1; i++)
		//{
		//    players.Add(new Player(new User("others", "2222")));
		//}

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

		foreach (PlayerCard p in playerCardDeck){
			AllHandCards.Add(p);
		}
		AllHandCards.Add(EpidemicCard.getEpidemicCard());

		foreach (Player p in players)
		{
			RoleKind rk = selectRole();
			Role r = new Role(rk);
			Pawn pawn = Instantiate(prefab, new Vector3(0, 0, 100), gameObject.transform.rotation);
			r.setPawn(pawn);
			p.setRole(r);
			pawn.transform.parent = GameObject.FindGameObjectWithTag("background").transform;


		}
		List<Color> dc = mapInstance.getDiseaseColor();
		foreach (Color c in dc)
		{
			Disease d = new Disease(c);
			diseases.Add(c, d);
		}

		gameInfoController.displayOutbreak();
		gameInfoController.displayInfectionRate();

		//FOR GUI
		foreach (Player p in players)
		{
			if (!p.Equals(me))
			{
				playerPanel.addOtherPlayer(p.getRoleKind());
			}
		}
		playerSelect.gameObject.SetActive (false);

		setInitialHand();
		shuffleAndAddEpidemic();
		setUp();
		currentPhase = GamePhase.PlayerTakeTurn;
		//Debug.Log("Everything Complete");
		Debug.Log("the role is" + me.getRoleKind().ToString());
	}

	public Player FindPlayer(PhotonPlayer photonPlayer){
		int index = players.FindIndex (x => x.PhotonPlayer == photonPlayer);
		//Debug.Log (photonPlayer.ID);
		return players [index];
	}

    public RoleKind selectRole()
    {	
		UnityEngine.Random.seed = 34;
        RoleKind rkRandom = (RoleKind)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(RoleKind)).Length));

        while(roleKindTaken.Contains(rkRandom))
        {
            rkRandom =  (RoleKind)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(RoleKind)).Length));
        }

        roleKindTaken.Add(rkRandom);
        return rkRandom;
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
                InfectionCard ic = getInfectionCard();
                ic.getCity().addCubes(i);
            }
        }
    }

    public void setInitialHand()
    {
        Collection.Shuffle<PlayerCard>(playerCardDeck);
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
		Debug.Log ("current player is player" + currentPlayer.PhotonPlayer.NickName);
    }

	public void EndTurn(){
		PhotonView.RPC ("RPC_endTurn",PhotonTargets.All);
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
		draw card from the top of the player card deck
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
                if (currentPlayer.getHandCardNumber() > currentPlayer.getHandLimit())
                {
                    player.removeCard(AckCardToDrop(player.getHand()));
                }
                //FOR GUI
                Debug.Log(me.getRoleKind());
                if (!player.Equals(me))
                {

                    playerPanel.addPlayerCardToOtherPlayer(player.getRoleKind(), card);
                }
                else
                {
                    Debug.Log("add card to main player" + card.ToString());
                    mainPlayerPanel.addPlayerCard(card);
                }
            }
			gameInfo.changeCardNumber (-1);
            
        }

        return true;
    }

    public PlayerCard AckCardToDrop(List<PlayerCard> cards)
    {
        //TODO Handle hand limit here
        //Input the list of PlayerCards he has output the card he want to drop
        return null; 
    }

    private void setGamePhase(GamePhase gamePhase)
    {
        currentPhase = gamePhase;
    }

    //my part ends here

    public void infectNextCity()
    {
        InfectionCard card = getInfectionCard();
        City city = card.getCity();
        Color color = card.getColor();
        Disease disease = diseases[color];
        outbreakedCities.Clear();
        if (!infect(city, color, 1))
        {
            return;
        }
    }

    public InfectionCard getInfectionCard()
    {
        InfectionCard card = infectionDeck[0];
        infectionDeck.Remove(card);
        infectionDiscardPile.Add(card);

        return card;
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
    public List<Player> getPlayers(City city)
    {
        List<Player> pInCity = new List<Player>();
        foreach (Pawn p in city.getPawns())
        {
            pInCity.Add(getPlayer(p.getRoleKind()));
        }
        return pInCity;
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
    public PlayerCard getPlayerCard(String cardName)
    {

        foreach (PlayerCard card in AllHandCards)
        {
            CardType type = card.getType();
            String universalName;
            if (type == CardType.EventCard)
            {
                universalName = ((EventCard)card).getEventKind().ToString();
            }
            else if (type == CardType.CityCard)
            {
                universalName = ((CityCard)card).getCity().getCityName().ToString();
      
            }
            else if (type == CardType.EpidemicCard)
            {
                universalName = ((EpidemicCard)card).getType().ToString();
            }
            else
            {
                Debug.Log("Invalid card type exists in AllHandCards. Class: Game.cs : getPlayerCard");
                return null;
            }

            if (universalName.Equals(cardName))
            {
                return card;
            }
            
        }
        Debug.Log("Corresponding PlayerCard Not found. Class: Game.cs : getPlayerCard");
        return null; 
    }

    public void exchangeCard(RoleKind roleKind, CityCard cityCard)
    {
        Player cardHolder = null;
        Player target = getPlayer(roleKind);
        foreach(Player player in players)
        {
            foreach(PlayerCard p in player.getHand())
            {
                if(p == cityCard)
                {
                    cardHolder = player;
                    break;
                }
            }
        }

        if(cardHolder != null)
        {
            if(cardHolder == currentPlayer)
            {
                giveCard(currentPlayer, target, cityCard);
            }
            else if(cardHolder == target)
            {
                giveCard(target, currentPlayer, cityCard);
            }
            else {
                Debug.Log("A uninterested player is holding the card. Class: Game.cs : exchangeCard(RoleKind,CityCard)");
            }
        }
        else
        {
            Debug.Log("CardHolder not found. Class: Game.cs : exchangeCard(RoleKind,CityCard)");
        }

    }

    public void giveCard(Player p1, Player p2, CityCard card)
    {
        p1.removeCard(card);
        p2.addCard(card);
    }

    public Player getPlayer(RoleKind roleKind)
    {
        foreach (Player p in players)
        {
            if (p.getRoleKind()== roleKind)
            {
                return p;
            }
        }
        Debug.Log("Corresponding Player not found of the given role kind. Class: Game.cs : getPlayer(RoleKind)");
        return null;
    }

    public Player getPlayer(String roleKind)
    {
        foreach (Player p in players)
        {
            if (p.getRoleKind().ToString().Equals(roleKind))
            {
                return p;
            }
        }
        Debug.Log("Corresponding Player not found of the given role kind. Class: Game.cs : getPlayer(String)");
        return null;
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

    public int getInfectionIndex()
    {
        return index;
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
            if (!player.Equals(me))
            {

                playerPanel.deletePlayerCardFromOtherPlayer(player.getRoleKind(), card);
            }
            else
            {
                mainPlayerPanel.deletePlayerCard(card);
            }
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
		Debug.Log ("Flight succeed");
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

	public void TreatDisease(string color, string name){
		PhotonView.RPC ("RPC_treatDisease",PhotonTargets.All,color,name );
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

	//we need to split this method to two methods: take and give, pls do this asap!
	public void share(string targetPlayerRoleKind, string cardName){   
		PhotonPlayer target = getPlayer (targetPlayerRoleKind).PhotonPlayer;
		PhotonView.RPC ("RPC_askForPermission", target, cardName);
     }

    public void takeCard(Player targetPlayer, CityCard card){
        targetPlayer.removeCard(card);
        currentPlayer.addCard(card);

        currentPlayer.decreaseRemainingAction();
    }

    public void giveCard(Player targetPlayer, CityCard card){
        currentPlayer.removeCard(card);
        targetPlayer.addCard(card);

        currentPlayer.decreaseRemainingAction();
    }

	// zhening's work! written at 5.04am!
	public void take(string name){
		PhotonPlayer target = findPlayerWithCard (name).PhotonPlayer;
		PhotonView.RPC ("RPC_askForPermission",target,name);
		//AskforPermisson ();
	}

	private void askForPermisson(string name){
		shareOperation.askPermission (name);
	}

	public void sendResponse(bool consentResult){
		PhotonView.RPC ("RPC_sendConsentResult",currentPlayer.PhotonPlayer, consentResult);
	}

	private void informResponse(bool consentResult){
		shareOperation.showResponse(consentResult);
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

	public City findCity(string name){

		foreach (City c in cities) {
			if (c.cityName.ToString ().Equals (name))
				return c;
		}

		return null;
	}
    
    //convert a string color to Color
	public Color findColor(string color){
		switch (color) {
		case "red":
			return Color.red;
		case "blue":
			return Color.blue;
		case "black":
			return Color.black;
		case "yellow":
			return Color.yellow;
		}
		return Color.yellow;
	}

	/* this method is used to find a player with specific card
	 * @cardNmae the name of the card
	 */

	private Player findPlayerWithCard(string cardName){
		foreach (Player player in players) {
			foreach (PlayerCard card in player.getHand()) {
				if (card.getType() == CardType.CityCard 
					&& ((CityCard)card).getCity().getCityName().ToString().Equals(cardName)) {
					return player;
				}
			}
		}
		return null;
	}

}
