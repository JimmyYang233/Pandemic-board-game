﻿using System;
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
	[SerializeField]
    private int infectionRate=2;
    private int[] infectionArray;
    private int infectionCardDrawn;
    private bool isnewGame = true;
	[SerializeField]
    private int outbreaksValue = 0;
	private bool oneQueitNightUsed = false;
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
	private List<City> cities;
	private Player me;
	private CityCard cardToShare;
	private Player playerToShare;
	private bool switchPlayer = false;
	private int numOfInfection = 0;
	private Player BioTerroristVolunteer = null;
    #endregion
    //FOR GUI
    public PlayerPanelController playerPanel;
    public PCPanelController mainPlayerPanel;
	public playerSelectionPanel playerSelect;
	public ShareOperation shareOperation;
    public PassOperation passOperation;
    public ChatBox chatBox;
    public Record record;


    GameObject backGround;

    
    public int nEpidemicCard;
    public Pawn prefab;
    public GameInfoDisplay gameInfoController;


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
		drive(findPlayer(roleKind),findCity(name));
	}

	[PunRPC]
	public void RPC_takeDirectFlight(string roleKind,string name){
		takeDirectFlight(findPlayer(roleKind),(CityCard)findPlayerCard(name));
	}

	[PunRPC]
	public void RPC_treatDisease(string color,string cityName){
		treatDisease(diseases[findColor(color)], findCity(cityName));
	}

	[PunRPC] 
	public void RPC_askForPermission(string cardName){
		askForPermisson (cardName);
	}

	[PunRPC]
	public void RPC_sendConsentResult(bool consentResult){
		informResponse (consentResult);
	}

	[PunRPC]
	public void RPC_endTurn(){
		endTurn ();
	}

	[PunRPC]
	public void RPC_exchangeCard (string targetPlayerRoleKindName, string cardName){
		RoleKind targetPlayerRoleKind = findRoleKind (targetPlayerRoleKindName);
		CityCard card = (CityCard)findPlayerCard (cardName);
		exchangeCard (targetPlayerRoleKind, card);	
	}

	[PunRPC]
	public void RPC_takeCharterFlight(string playerRoleKindName, string cityName){
		takeCharterFlight (findPlayer(playerRoleKindName), findCity(cityName));
	}

	[PunRPC]
	public void RPC_takeShuttleFlight(string playerRoleKindName, string cityName){
		takeCharterFlight (findPlayer(playerRoleKindName), findCity(cityName));
	}

	[PunRPC]
	public void RPC_build(string initialCityName, string cityCardName){
        if (initialCityName.Equals(String.Empty))
        {
            build(null, (CityCard)findPlayerCard(cityCardName));
        }
        else
        {
            build(findCity(initialCityName), (CityCard)findPlayerCard(cityCardName));
        }
		
	}

	[PunRPC]
	public void RPC_displayMessage(string myRoleKindName,string text){
		chatBox.displayText (findRoleKind(myRoleKindName),text);
	}

	[PunRPC]
	public void RPC_infectNextCity(){
        //Debug.Log("RPC_infectNextCity got called");
		infectNextCity ();
	}

	[PunRPC]
	public void RPC_infectCity(){
		infectCity ();
	}

	[PunRPC]
	public void RPC_nextPlayer(){
		nextPlayer ();
		switchPlayer = false;
	}

	[PunRPC]
	public void RPC_notifyResolveEpidemic(){
		notifyResolveEpidemic ();
	}

	[PunRPC]
	public void RPC_cure(string playerRoleKind, string diseaseColor, string card1, string card2, string card3, string card4, string card5){
		Player player = findPlayer (playerRoleKind);
		List<CityCard> cityCardsToRemove = new List<CityCard>();
		cityCardsToRemove.Add((CityCard)findPlayerCard(card1));
        cityCardsToRemove.Add((CityCard)findPlayerCard(card2));
        cityCardsToRemove.Add((CityCard)findPlayerCard(card3));
        cityCardsToRemove.Add((CityCard)findPlayerCard(card4));
        if (!card5.Equals(""))
        {
            cityCardsToRemove.Add((CityCard)findPlayerCard(card1));
        }
        Disease disease = findDisease(diseaseColor);
		cure (player, cityCardsToRemove, disease);
	}
	#endregion

	//called by chatbox to send chat message
	public void sendChatMessage(string message){
		PhotonView.RPC ("RPC_displayMessage", PhotonTargets.All,me.getRoleKind().ToString(), message );
	}

	#region basicOperationInteraction
	public void Drive(string roleKind,string name){
		PhotonView.RPC ("RPC_drive",PhotonTargets.All,roleKind,name);
	}

	public void TakeDirectFlight(string roleKind,string name){
		PhotonView.RPC ("RPC_takeDirectFlight",PhotonTargets.All,roleKind,name);
	}

	public void TakeCharterFlight(string playerRoleKindName, string cityName){
		PhotonView.RPC ("RPC_takeCharterFlight", PhotonTargets.All,playerRoleKindName, cityName);
	}

	public void TakeShuttleFlight(string playerRoleKindName, string cityName){
		PhotonView.RPC ("RPC_takeShuttleFlight", PhotonTargets.All,playerRoleKindName, cityName);
	}

	//this method will be called by shareOperation to ask the target for permission
	public void share(string targetPlayerRoleKind, string cardName){   
		PhotonPlayer target = findPlayer (targetPlayerRoleKind).PhotonPlayer;
		PhotonView.RPC ("RPC_askForPermission", target, cardName);
		cardToShare = (CityCard)findPlayerCard (cardName);
		playerToShare = findPlayer (targetPlayerRoleKind);
	}

	public void TreatDisease(string color, string name){
		PhotonView.RPC ("RPC_treatDisease",PhotonTargets.All,color,name );
	}

	public void Cure(string playerRoleKind, List<string> cardsToRemove, string diseaseColor){
        string card1 = cardsToRemove[0];
        string card2 = cardsToRemove[1];
        string card3 = cardsToRemove[2];
        string card4 = cardsToRemove[3];
        string card5 = "";
        if (cardsToRemove.Count > 4)
        {
            card5 = cardsToRemove[4];
        }
        
        PhotonView.RPC ("RPC_cure", PhotonTargets.All, playerRoleKind, diseaseColor, card1, card2, card3, card4, card5);
	}

	public void EndTurn(){
		PhotonView.RPC ("RPC_endTurn",PhotonTargets.All);
		PhotonView.RPC ("RPC_infectCity",currentPlayer.PhotonPlayer);
	}

	//called by front end to make all clients infect next city
	public void InfectNextCity(){
        //Debug.Log("InfectNextCity got called");
		PhotonView.RPC ("RPC_infectNextCity",PhotonTargets.All);
	}
	#endregion

	public void NextPlayer(){
		PhotonView.RPC ("RPC_nextPlayer",PhotonTargets.All);
	}


	#region initialization
	//initialize player in the network
	private void InitializePlayer(){
		PhotonPlayer[] photonplayers = PhotonNetwork.playerList;
		Array.Sort (photonplayers, delegate(PhotonPlayer x, PhotonPlayer y) {
			return x.ID.CompareTo(y.ID);
		});
		foreach (PhotonPlayer player in photonplayers){
			players.Add (new Player(player));
		}
	}

	//initialzie game, set the first player as current player
	private void InitializeGame(){
		//load city
		researchStationRemain = 6;
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
		currentPlayer = players[0];
		//Debug.Log ("current player is player" + currentPlayer.PhotonPlayer.NickName);

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

        Player bioTerrorist = null;

        if(challenge == Challenge.BioTerroist)
        {
			bioTerrorist = (BioTerroristVolunteer==null) ? players[UnityEngine.Random.Range(0, players.Count+1)] : BioTerroristVolunteer;
        }

        foreach (Player p in players) 
		{
			Role r = (p != bioTerrorist) ? new Role(selectRole()) : new BioTerrorist();
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
		playerPanel.addMainPlayer(me.getRoleKind());
		playerSelect.gameObject.SetActive (false);

		setInitialHand();
		shuffleAndAddEpidemic();
		setUp();
		currentPhase = GamePhase.PlayerTakeTurn;
		//Debug.Log("Everything Complete");
		//Debug.Log("the role is" + me.getRoleKind().ToString());
	}
	#endregion


	#region basicOperation
	//drive
	private void drive(Player player, City destinationCity)
	{
		Pawn p = player.getPlayerPawn();
		City initialCity = p.getCity();
		p.setCity(destinationCity);
		initialCity.removePawn(p);
		destinationCity.addPawn(p);
		RoleKind rolekind = player.getRoleKind();

		if (rolekind == RoleKind.Medic)
		{
            resolveMedic(destinationCity);
		}
		else if (rolekind == RoleKind.ContainmentSpecialist)
		{
            resolveContainmentSpecialist(destinationCity);
		}
        if (player.getMobileHospitalActivated())
        {
            //TODO call treat, to remove 1 cube from the city
        }
        if (player.getRoleKind() == RoleKind.BioTerrorist && !((BioTerrorist)player.getRole()).getBioTerroristExtraDriveUsed())
        {
            ((BioTerrorist)player.getRole()).setbioTerroristExtraDriveUsed();
        }
        else
        {
            player.decreaseRemainingAction();
        }
		
		record.drive(currentPlayer, destinationCity);
		//Debug.Log ("move succeed");
	}

	//take direct flight
	private void takeDirectFlight(Player player, CityCard card)
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
			resolveMedic (destinationCity);
		}
		else if(rolekind == RoleKind.ContainmentSpecialist)
		{
			resolveContainmentSpecialist (destinationCity);
		}
		player.decreaseRemainingAction();
		record.directFlight(currentPlayer, destinationCity);
		//Debug.Log ("Flight succeed");
		//UI only
	}

	//take charter flight
	private void takeCharterFlight(Player pl1, City destination){
		CityCard card = null;
		City curCity = pl1.getPlayerPawn ().getCity ();
		foreach(PlayerCard p in pl1.getHand()){
			if(p.getType() == CardType.CityCard && ((CityCard)p).getCity()== curCity){
				card = (CityCard) p;
				pl1.removeCard(card);
                if (!pl1.Equals(me))
                {

                    playerPanel.deletePlayerCardFromOtherPlayer(pl1.getRoleKind(), card);
                }
                else
                {
                    mainPlayerPanel.deletePlayerCard(card);
                }
                playerDiscardPile.Add(card);
				move(pl1,destination);
				pl1.decreaseRemainingAction ();
				record.charterFlight(currentPlayer, destination);
				break;
			}
		}

		if(card == null){
			//Debug.Log("Player does not have corresponding card.");
		}
	}

	//take shuttle flight
	private void takeShuttleFlight(Player pl1, City destination){
		if (pl1.getPlayerPawn ().getCity ().getHasResearch () && destination.getHasResearch()) {
			move (pl1, destination);
			pl1.decreaseRemainingAction ();
			record.shuttleFlight(currentPlayer, destination);
		} 
		else 
		{
			//Debug.Log ("One of the cities does not have a reseach lab. Game.cs: takeShuttleFlight");
		}
	}

	//share
	private void exchangeCard(RoleKind roleKind, CityCard cityCard)
	{
		Player cardHolder = null;
		Player target = findPlayer(roleKind);
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
				record.giveCard(currentPlayer, target, cityCard);
			}
			else if(cardHolder == target)
			{
				giveCard(target, currentPlayer, cityCard);
				record.takeCard(currentPlayer, target, cityCard);
			}
			else {
				Debug.Log("A uninterested player is holding the card. Class: Game.cs : exchangeCard(RoleKind,CityCard)");
			}
		}
		else
		{
			Debug.Log("CardHolder not found. Class: Game.cs : exchangeCard(RoleKind,CityCard)");
		}
		currentPlayer.decreaseRemainingAction ();
	}

	//treat
	private void treatDisease(Disease d, City currentCity)
	{
		RoleKind rolekind = currentPlayer.getRoleKind();
		bool isCured = d.isCured();
		int treatNumber = 1;
		if(rolekind == RoleKind.Medic||isCured == true)
		{
			treatNumber = currentCity.getCubeNumber(d);
		}
		currentCity.removeCubes(d, treatNumber);
		d.addCubes(treatNumber);
        gameInfoController.changeDiseaseNumber(d.getColor(), d.getNumOfDiseaseCubeLeft());
       // Debug.Log(d.getNumOfDiseaseCubeLeft());
        int num = d.getNumOfDiseaseCubeLeft();
		if(num == MAX && isCured == true)
		{
			d.isEradicated();
		}

		currentPlayer.decreaseRemainingAction();
		record.treat(currentPlayer, treatNumber, d, currentCity);
	}

	//cure
	private void cure(Player player, List<CityCard> cardsToRemove, Disease d)
	{
		foreach (CityCard card in cardsToRemove)
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

		d.cure();
		gameInfoController.cure (d.getColor());
		int num = d.getNumOfDiseaseCubeLeft();
		if(num == MAX)
		{
			d.isEradicated();
		}

		//UI TODO: set disease’s cure marker

		currentPlayer.decreaseRemainingAction();
	}

	//pass
	private void endTurn()
	{
		if (currentPhase != GamePhase.PlayerTakeTurn)
			return;
		currentPhase = GamePhase.PlayerDrawCard;

		currentPlayer.refillAction();
		currentPlayer.setOncePerturnAction(false);

        if (currentPlayer.getMobileHospitalActivated())
        {
            currentPlayer.setMobileHospitalActivated(false);
        }

		int playerCardDeckSize = playerCardDeck.Count;
		record.pass(currentPlayer);
		//Note that epidemic card is resolved in "draw" method
		//if there is no enough player cards in the deck, players lose the game
		
		if (!draw(currentPlayer, 2))
		{
			return;
		}
		currentPhase = GamePhase.InfectCities;
		numOfInfection = 0;
	}
	#endregion

	#region endTurnOperation
	//infectNextCity
	private void infectNextCity()
	{
		numOfInfection++;
		//Debug.Log ("Danning kan zhe li");
		InfectionCard card = getInfectionCard();
		City city = card.getCity();
		Color color = card.getColor();
		Disease disease = diseases[color];
		
		outbreakedCities.Clear();
		if (!infect(city, color, 1))
		{
			return;
		}
		if (currentPlayer == me && numOfInfection < infectionRate) {
			//Debug.Log ("num of infection is + " + numOfInfection.ToString());
			passOperation.startInfection ();
		}

		if (numOfInfection == infectionRate && PhotonNetwork.isMasterClient)
			PhotonView.RPC ("RPC_nextPlayer", PhotonTargets.All);
	}

	private void infectCity()
	{
		if(oneQueitNightUsed){
			oneQueitNightUsed = false;
			return;
		}
		//Debug.Log ("start infect city");
		passOperation.startInfection ();
		//nextPlayer();
	}

	private void notifyResolveEpidemic(){
		passOperation.notifyResolveEpidemic ();
	}
	#endregion


    private void move(Player pl1, City destinationCity){
        RoleKind rolekind = pl1.getRoleKind();
        Pawn p = pl1.getPlayerPawn();
		City initialCity = p.getCity();
		p.setCity(destinationCity);
		initialCity.removePawn(p);
		destinationCity.addPawn(p);

		if (rolekind == RoleKind.Medic) {
			resolveMedic (initialCity);
		} else if (rolekind == RoleKind.ContainmentSpecialist) {
			resolveContainmentSpecialist (destinationCity);
		} else if (rolekind == RoleKind.ContainmentSpecialist) {
			Dictionary<Color,int> cubes = destinationCity.getNumOfCubes ();
			foreach(Color c in cubes.Keys){
				if (cubes [c] > 1) {
					destinationCity.removeCubes (diseases[c],1);
				}
			}
		} 
    }

	private void resolveMedic(City destinationCity){
        foreach (Disease disease in diseases.Values)
			{
				if (disease.isCured())
				{
					int cubeNumber = destinationCity.getCubeNumber(disease);
					destinationCity.removeCubes(disease, cubeNumber);
					disease.addCubes(cubeNumber);
                    gameInfoController.changeDiseaseNumber(disease.getColor(), disease.getNumOfDiseaseCubeLeft());
                    Debug.Log(disease.getNumOfDiseaseCubeLeft());
                    int num = disease.getNumOfDiseaseCubeLeft();
					if (num == MAX)
					{
						disease.eradicate();
					}
				}

			}
    }

	private void resolveContainmentSpecialist(City destinationCity){
        foreach (Disease disease in diseases.Values)
			{
				int cubeNumber = destinationCity.getCubeNumber(disease);
				if (cubeNumber > 1)
				{
					destinationCity.removeCubes(disease, 1);
					disease.addCubes(1);
                    gameInfoController.changeDiseaseNumber(disease.getColor(), disease.getNumOfDiseaseCubeLeft());
                    Debug.Log(disease.getNumOfDiseaseCubeLeft());
                }
			}
    }

	private void putIntoInfectionDeck(List<InfectionCard> list){
		list.AddRange (infectionDeck);
		infectionDeck = list;
	}

	public void foreCast(){
		List<InfectionCard> list = borrowInfectionCard (6);

		// TODO

		putIntoInfectionDeck (list);
		dropEventCard (EventKind.Forecast);
	}

	public void governmentGrant(City c){
		if (getRemainingResearch () == 0) {
			//TODO
		}
		c.setHasResearch (true);
		researchStationRemain--;
		dropEventCard (EventKind.GovernmentGrant);
	}


	public void oneQuietNight(){
		oneQueitNightUsed = true;
		dropEventCard (EventKind.OneQuietNight);
	}



	public void resilientPopulation(InfectionCard card){
		infectionDiscardPile.Remove (card);
		dropEventCard (EventKind.ResilientPopulation);
	}

	public void airlift(Player pl1, City destination){
		move (pl1, destination);
		dropEventCard (EventKind.Airlift);
	}

	public void borrowedTime(){
		if (findEventCardHolder(EventKind.BorrowedTime) == currentPlayer) {
			currentPlayer.increaseRemainingAction (2);
			dropEventCard (EventKind.BorrowedTime);
		} 
		else {
			//TODO notify player that card cannot be used
		}
	}

    public void mobileHospital(Player pl1)
    {
        pl1.setMobileHospitalActivated(true);
        dropEventCard(EventKind.MobileHospital);
    }

    public Player findEventCardHolder(EventKind eCard){
		Player holder = null;

		foreach(Player pl in players){
			foreach(PlayerCard p in pl.getHand()){
				if(p.getType() == CardType.EventCard){
					if (((EventCard)p).getEventKind() == eCard) {
						holder = pl;
						break;
					}
				}
			}
		}

		return holder;
	}

	private bool checkForRoleExistence(RoleKind roleKind){
		foreach(Player pl in players){
			if(pl.getRoleKind() == roleKind)
			{
				return true;
			}
		}
		return false;
	}

	private bool swapRole(Player pl1, RoleKind roleKind){
		if(checkForRoleExistence(roleKind)){
			Debug.Log ("Role already exist in the game. Game.cs: swapRole");
			return false;
		}
		Pawn pawn = pl1.getPlayerPawn ();
		City city = pawn.getCity ();
		city.removePawn (pawn);
		Role r2 = new Role (roleKind);

		if(r2.getRoleKind() == RoleKind.Generalist && pl1 == currentPlayer && pl1.getMaxnumAction() == 4){
			pl1.increaseRemainingAction (1);
		}

		pl1.setRole (r2);
		city.addPawn (pl1.getPlayerPawn());
        return true;
	}

	public void newAssignment(Player pl1, RoleKind roleKind){
		if (!swapRole (pl1, roleKind)) {
			return;
		}
		dropEventCard (EventKind.NewAssignment);
	}

	public void dropEventCard(EventKind eKind){
		Player pl = findEventCardHolder (eKind);
		EventCard eCard = EventCard.getEventCard (eKind);
		if (pl == null) {
			pl = findPlayer(RoleKind.ContingencyPlanner);
            if(pl == null)
            {
                Debug.Log("No player is holding this card. Game.cs: dropEventCard(EventKind)");
            }
            else
            {
                pl.removeEventCardOnTopOfRoleCard();
            }
		}
        else
        {
            pl.removeCard (eCard);
		    playerDiscardPile.Add (eCard);
        }
		
	}

    

    public void contingencyPlannerPutCardOnTopOfRoleCard(Player pl1, EventCard card)
    {
        pl1.setEventCardOnTopOfRoleCard(card);
        pl1.decreaseRemainingAction();
    }

    public RoleKind selectRole()
    {	
		UnityEngine.Random.seed = 34;

        
        int num = 8;

        if (hasDLC)
        {
            num = 14;
        }

        RoleKind rkRandom = (RoleKind)(UnityEngine.Random.Range(0, num));

        while (roleKindTaken.Contains(rkRandom))
        {
            rkRandom = (RoleKind)(UnityEngine.Random.Range(0, num));
        }

        roleKindTaken.Add(rkRandom);
        return rkRandom;
    }

    private void setUp()
    {
        City Atlanta = findCity(CityName.Atlanta);
        Atlanta.setHasResearch(true);
        researchStationRemain--;
		gameInfoController.changeResearchNumber (researchStationRemain);
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
                diseases[ic.getColor()].removeCubes(i);
                gameInfoController.changeDiseaseNumber(ic.getColor(), diseases[ic.getColor()].getNumOfDiseaseCubeLeft());
				record.infect(ic.getCity(),i);
			}
        }
    }

    private void setInitialHand()
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
		
    



    private bool resolveEpidemic()
    {
		PhotonView.RPC ("RPC_notifyResolveEpidemic",PhotonTargets.All);
        resolvingEpidemic = true;
        if(infectionRate < 4)
        {
            infectionRate = infectionArray[++index];
            gameInfoController.displayInfectionRate();
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
		bool isCured = disease.isCured();
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

		record.infect(city,number,hasMedic,hasQS,isCured,isEradicated);

        if ((hasQS&&isCured) || hasMedic || isEradicated) return true;

        outbreakedCities.Add(city);
        int cubeNumber = city.getCubeNumber(color);
        int remainingCubes = disease.getNumOfDiseaseCubeLeft();
        //if not exceeding 3 cubes, put cubes to that city
        if (cubeNumber < 3)
        {
            //check if there is enough cubes left 
            if (remainingCubes - number < 0)
            {
                notifyGameLost(GameLostKind.RunOutOfDiseaseCube);
                //setGamePhase (GamePhase.Completed);
                return false;
            }
            city.addCubes(disease, number);
            disease.removeCubes(number);
            gameInfoController.changeDiseaseNumber(disease.getColor(), disease.getNumOfDiseaseCubeLeft());
            //Debug.Log(disease.getNumOfDiseaseCubeLeft());
            return true;
        }
        //else there will be an outbreak
        else
        {
			Debug.Log ("An outbreak happens in " + city.ToString());
			record.outbreak(city);
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
            disease.removeCubes(3 - cubeNumber);
            gameInfoController.changeDiseaseNumber(disease.getColor(), disease.getNumOfDiseaseCubeLeft());
            Debug.Log(disease.getNumOfDiseaseCubeLeft());

            foreach (City neighbor in neighbors) {
                if (outbreakedCities.Contains(neighbor))
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
			record.draw(player, card);
            if (card.getType() == CardType.EpidemicCard)
            {
                resolveEpidemic();
            }
            else if (card.getType() == CardType.MutationEventCard && !diseases[Color.magenta].isEradicated())
            {
                InfectionCard mCard = drawBottomInfectionDeck();
                City mCity = mCard.getCity();
                infect(mCity, Color.magenta, 3);
                infectionDiscardPile.Add(mCard);
            }
            else
            {
                player.addCard(card);
                if (currentPlayer.getHandCardNumber() > currentPlayer.getHandLimit())
                {
                    player.removeCard(AckCardToDrop(player.getHand()));
                }
                //FOR GUI
                //Debug.Log(me.getRoleKind());
                if (!player.Equals(me))
                {

                    playerPanel.addPlayerCardToOtherPlayer(player.getRoleKind(), card);
                }
                else
                {
                    //Debug.Log("add card to main player" + card.ToString());
                    mainPlayerPanel.addPlayerCard(card);
                }

				
            }
			gameInfoController.changeCardNumber (playerCardDeck.Count);
			//Debug.Log ("player deck count"+playerCardDeck.Count);

			
			// For debugging: After first turn, number of player card will increase
			// if (playerCardDeck[0].getType() == CardType.CityCard){
			// 	CityCard tmp = (CityCard)playerCardDeck[0];
			// 	//Debug.Log (tmp.getCity().cityName);
			// }
			// else if(playerCardDeck[0].getType() == CardType.EventCard){
			// 	//Debug.Log ("Event");
			// }
			// else if(playerCardDeck[0].getType() == CardType.EpidemicCard){
			// 	//Debug.Log ("Epidemic");
			// }
        }

        return true;
    }


    public void BioTerroristDraw(Player pl)
    {
        InfectionCard card = infectionDeck[0];
        infectionDeck.Remove(card);
        record.draw(pl, card);
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

	public GamePhase getCurrentPhase(){
		return currentPhase;
	}

    //my part ends here

    

    private InfectionCard getInfectionCard()
    {
        InfectionCard card = infectionDeck[0];
        infectionDeck.Remove(card);
        infectionDiscardPile.Add(card);

        return card;
    }

	private List<InfectionCard> borrowInfectionCard(int num){
		List<InfectionCard> list = new List<InfectionCard> ();
		for(int i=0; i<num;i++){
			InfectionCard card = infectionDeck[0];
			infectionDeck.Remove(card);
			list.Add (card);
		}
		return list;
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
			pInCity.Add(findPlayer(p.getRoleKind()));
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
		currentPlayer =(challenge == Challenge.BioTerroist) ? findPlayer (RoleKind.BioTerrorist):players[(players.IndexOf(currentPlayer) + 1) % (players.Count)];
		currentPhase = GamePhase.PlayerTakeTurn;
		//Debug.Log (players.IndexOf(currentPlayer));
    }

	public void setBioTerroristVolunteer(Player pl){
		BioTerroristVolunteer = pl;
	}

    

    public void giveCard(Player p1, Player p2, CityCard card)
    {
        p1.removeCard(card);
		if (!p1.Equals(me))
		{

			playerPanel.deletePlayerCardFromOtherPlayer(p1.getRoleKind(), card);
		}
		else
		{
			mainPlayerPanel.deletePlayerCard(card);
		}
        p2.addCard(card);
		if (!p2.Equals(me))
		{

			playerPanel.addPlayerCardToOtherPlayer(p2.getRoleKind(), card);
		}
		else
		{
			//Debug.Log("add card to main player" + card.ToString());
			mainPlayerPanel.addPlayerCard(card);
		}
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

    public int getRemainingResearch()
    {
        return researchStationRemain;
    }

    public List<City> getCities()
    {
        return cities;
    }
    
    public List<PlayerCard> getPlayerDiscardPile()
    {
        return playerDiscardPile;
    }


    public void build(City initialCity, CityCard card)
    {
        RoleKind rolekind = currentPlayer.getRoleKind();
        Pawn p = currentPlayer.getPlayerPawn();
        City currentCity = p.getCity();
        if (rolekind != RoleKind.OperationsExpert)
        {
            currentPlayer.removeCard(card);
            if (!currentPlayer.Equals(me))
            {
                playerPanel.deletePlayerCardFromOtherPlayer(currentPlayer.getRoleKind(), card);
            }
            else
            {
                mainPlayerPanel.deletePlayerCard(card);
            }
            playerDiscardPile.Add(card);
        }
		record.build(initialCity);
        currentCity.setHasResearch(true);

        if (researchStationRemain == 0)
        {
            if(initialCity != null)
            {
                initialCity.setHasResearch(false);
            }   
        }
        else
        {
            researchStationRemain--;
            gameInfoController.changeResearchNumber(researchStationRemain);
        }

        currentPlayer.decreaseRemainingAction();
    }

	public void Build(string initialCityName, string cityCardName){
		PhotonView.RPC ("RPC_build",PhotonTargets.All, initialCityName, cityCardName);
	}


	/*
    public void takeCard(Player targetPlayer, CityCard card){
        targetPlayer.removeCard(card);
        currentPlayer.addCard(card);

        currentPlayer.decreaseRemainingAction();
    }

    public void giveCard(Player targetPlayer, CityCard card){
        currentPlayer.removeCard(card);
        targetPlayer.addCard(card);

        currentPlayer.decreaseRemainingAction();
    }*/

	// zhening's work! written at 5.04am!  obsolete in this version
	public void take(string name){
		PhotonPlayer target = findPlayerWithCard (name).PhotonPlayer;
		PhotonView.RPC ("RPC_askForPermission",target,name);
		//AskforPermisson ();
	}

	private void askForPermisson(string name){
        Player p = findPlayerWithCard(name);
        if(currentPlayer == p)
        {
            shareOperation.askGivePermission(name);
        }
        else
        {
            shareOperation.askTakePermission(name);
        }
		
	}

	//this method will be called by shareOperation to send response to current player
	public void sendResponse(bool consentResult){
        //Debug.Log("SendRespons Called");
		PhotonView.RPC ("RPC_sendConsentResult",currentPlayer.PhotonPlayer, consentResult);
	}

	private void informResponse(bool consentResult){
		shareOperation.showResponse(consentResult);
		//if true, exchange this card
		if (consentResult) {
			PhotonView.RPC ("RPC_exchangeCard",PhotonTargets.All, playerToShare.getRoleKind().ToString(), cardToShare.getCity().getCityName().ToString());
		}
		//TO DO: maybe we need to inform the player the current player the response result;
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

	#region findMethod
	/* this method is used to find a player with specific card
	 * @cardNmae the name of the card
	 */
	public Player findPlayerWithCard(string cardName){
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

	public Player FindPlayer(PhotonPlayer photonPlayer){
		int index = players.FindIndex (x => x.PhotonPlayer == photonPlayer);
		//Debug.Log (photonPlayer.ID);
		return players [index];
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

	//used to find playerCard with name @cardName
	public PlayerCard findPlayerCard(String cardName)
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
				Debug.Log("Invalid card type exists in AllHandCards. Class: Game.cs : findPlayerCard");
				return null;
			}

			if (universalName.Equals(cardName))
			{
				return card;
			}

		}
		Debug.Log("Corresponding PlayerCard Not found. Class: Game.cs : findPlayerCard");
		return null; 
	}

	public Player findPlayer(RoleKind roleKind)
	{
		foreach (Player p in players)
		{
			if (p.getRoleKind()== roleKind)
			{
				return p;
			}
		}
		Debug.Log("Corresponding Player not found of the given role kind. Class: Game.cs : findPlayer(RoleKind)");
		return null;
	}

	public Player findPlayer(string roleKind)
	{
		foreach (Player p in players)
		{
			if (p.getRoleKind().ToString().Equals(roleKind))
			{
				return p;
			}
		}
		Debug.Log("Corresponding Player not found of the given role kind. Class: Game.cs : findPlayer(String)");
		return null;
	}

	public RoleKind findRoleKind(string roleKind){
		return (RoleKind)Enum.Parse (typeof(RoleKind), roleKind);
	}

	public Disease findDisease(string diseaseColorName){
		return diseases [findColor (diseaseColorName)];
	}
	#endregion




}
