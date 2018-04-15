using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class Game : MonoBehaviour {
	public static Game Instance;
	public PhotonView PhotonView;

	#region private variables
	private GameData savedGame;
    private readonly int MAX = 24;
    private readonly int purpleMax = 12;
    private int markersAvailable = 6; // TODO new field
    private Challenge challenge;
    private GamePhase currentPhase;
    private bool hasDLC;
	// Virulent Strain Challenge Part
	private Disease VirulentStrainDisease = null;
	private VirulentStrainEpidemicEffects currentVirulentStrainEpidemicEffects;
	private bool chronicEffect = false;
	private bool complexMolecularStructure = false;
	private bool governmentInterference = false;
	private bool usedTreated = false;
	// Virulent Strain end
	[SerializeField]
    private int infectionRate=2;
    private int[] infectionArray;
    private int infectionCardDrawn;
    private bool isnewGame = true;
	[SerializeField]
    private int outbreaksValue = 0;
	private bool oneQuietNightUsed = false;
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
	private int BioTerroristVolunteer = -1;
    private BioTerrorist bioTerroristRole = new BioTerrorist(); //TODO new field
    #endregion
    //FOR GUI
    public PlayerPanelController playerPanel;
    public PCPanelController mainPlayerPanel;
	public playerSelectionPanel playerSelect;
	public ShareOperation shareOperation;
    public PassOperation passOperation;
	public MoveOperation moveOperation;
    public ChatBox chatBox;
    public Record record;
	public infectionDiscardPileUI infectionDiscardUI;
	public playerDiscardPileUI playerDiscardUI;
	public eventCardController eventController;

    GameObject backGround;

    
	public int nEpidemicCard;
    public Pawn playerPawn;
    public Pawn bioterroristPawn;
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
			if (PlayerNetwork.Instance.isNewGame) {
				PhotonView.RPC ("RPC_InitializePlayer", PhotonTargets.All);
				PhotonView.RPC ("RPC_InitializeGame", PhotonTargets.All);
			} else {
				PhotonView.RPC ("RPC_LoadPlayer", PhotonTargets.All, PlayerNetwork.Instance.savedGameJson);
				PhotonView.RPC ("RPC_LoadGame", PhotonTargets.All);
			}
		}

	} 

	#region RPC method		
	[PunRPC]
	private void RPC_InitializePlayer(){
		Instance.InitializePlayer ();
	}

	[PunRPC]
	private void RPC_LoadPlayer(string gameDataJson){
		savedGame = JsonUtility.FromJson<GameData>(gameDataJson) ;
		Instance.LoadPlayer (savedGame);
	}

	[PunRPC]
	private void RPC_InitializeGame(){
		Instance.InitializeGame ();
	}

	[PunRPC]
	private void RPC_LoadGame(){
		Instance.LoadGame ();
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
		exchangeCard (targetPlayerRoleKind, card,false);	
	}

	[PunRPC]
	public void RPC_takeCharterFlight(string playerRoleKindName, string cityName){
		takeCharterFlight (findPlayer(playerRoleKindName), findCity(cityName));
	}

	[PunRPC]
	public void RPC_takeShuttleFlight(string playerRoleKindName, string cityName){
		takeShuttleFlight (findPlayer(playerRoleKindName), findCity(cityName));
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
	public void RPC_cure(string playerRoleKind, string diseaseColor, string[] cityCardName){
		Player player = findPlayer (playerRoleKind);
        List<CityCard> cityCardsToRemove = new List<CityCard>();
        for(int i = 0; i<cityCardName.Length; i++)
        {
            cityCardsToRemove.Add((CityCard)findPlayerCard(cityCardName[i]));
        }
        Disease disease = findDisease(diseaseColor);
		cure (player, cityCardsToRemove, disease);
	}
	[PunRPC]
	public void RPC_borrowedTime(){
		borrowedTime();
	}

	[PunRPC]
	public void RPC_oneQuietNight(){
		oneQuietNight();
	}

	[PunRPC]
	public void RPC_resilientPopulation(String cardName){
		resilientPopulation(findInfectionCard(cardName));
	}

	[PunRPC]
	public void RPC_ContingencyPlannerPutCardOnTopOfRoleCard(string cardName){
		EventCard card = (EventCard)findPlayerCard (cardName);
		contingencyPlannerPutCardOnTopOfRoleCard (card);
	}

	[PunRPC]
	public void RPC_forecast(string card1, string card2, string card3, string card4, string card5, string card6){
		List<InfectionCard> orderedCards = new List<InfectionCard>();
		
		if(card1!=null){orderedCards.Add(findInfectionCard(card1));}
		if(card2!=null){orderedCards.Add(findInfectionCard(card2));}
		if(card3!=null){orderedCards.Add(findInfectionCard(card3));}
		if(card4!=null){orderedCards.Add(findInfectionCard(card4));}
		if(card5!=null){orderedCards.Add(findInfectionCard(card5));}
		if(card6!=null){orderedCards.Add(findInfectionCard(card6));}
		
		forecast(orderedCards);
	}

	[PunRPC]
    public void RPC_mobileHospital(string roleKind)
    {
        mobileHospital(findPlayer(roleKind));
    }

    [PunRPC]
    public void RPC_governmentGrant(string initialCity, string endCity)
    {
        if (initialCity.Equals(String.Empty))
        {
            governmentGrant(null, findCity(endCity));
        }
        else
        {
            governmentGrant(findCity(initialCity), findCity(endCity));
        }
    }

	[PunRPC]
	public void RPC_archivistDraw(){
		archivistDraw ();
	}

	[PunRPC]
	public void RPC_fieldOperativeSample(string color){
		fieldOperativeSample (diseases[stringToColor(color)]);
	}

	[PunRPC]
	public void RPC_askForEventCardPermission(string info, string sourcePlayerRoleKind){
		askForEventCardPermission (info,sourcePlayerRoleKind);
	}

	[PunRPC]
	public void RPC_informEventCardPermissionResult(bool result){
		informEventCardPermissionResult (result);
	}

	[PunRPC]
	public void RPC_reExaminedResearch(string playerRoleKind, string cardName){
		Player player = findPlayer (playerRoleKind);
		CityCard cityCard = (CityCard)findPlayerCard (cardName);
		reExaminedResearch (player, cityCard);
	}

	[PunRPC]
	public void RPC_newAssignment(string targetRoleKind, string newRoleKind){
		Player player = findPlayer (targetRoleKind);
		newAssignment (player, findRoleKind(newRoleKind));
	}

	[PunRPC]
	public void RPC_remoteTreatment(string city1Name, string city2Name, string color1Name, string color2Name){
		City city1 = findCity (city1Name);
		City city2 = findCity (city2Name);
		Color color1 = stringToColor(color1Name);
		Color color2 = stringToColor (color2Name);
		remoteTreatment (city1,city2,color1,color2);
	}

	[PunRPC]
	public void RPC_commercialTravelBan(string playerRoleKind){
		Player player = findPlayer (playerRoleKind);
		commercialTravelBan (player);
	}

	[PunRPC]
	public void RPC_displayReExaminedResearch(){
		displayReExaminedResearch ();
	}

	[PunRPC]
	public void RPC_displayNewAssignment(){
		displayNewAssignment ();
	}

	[PunRPC]
	public void RPC_fieldOperativePutBack(string playerRoleKind, string colorString){
		Player targetPlayer = findPlayer (playerRoleKind);
		Color color = stringToColor (colorString);
		fieldOperativePutBack (targetPlayer,color);
	}

	[PunRPC]
	public void RPC_rapidVaccineDeployment(string colorString, string[] cityNamesArray){
		Debug.Log ("RPC sends: " + cityNamesArray.Length);
		Color color = stringToColor (colorString);
		List<City> cities = new List<City> ();
		for (int i = 0; i < cityNamesArray.Length; i++) {
			cities.Add (findCity(cityNamesArray[i]));
		}
		rapidVaccineDeployment (color, cities);
	}

	[PunRPC]
	public void RPC_askPermissionDispatcher(string request){
		askPermissionDispatcher (request);
	}

	[PunRPC]
	public void RPC_informDispatcher(bool result){
		informDispatcher (result);
	}

    [PunRPC]
    public void RPC_bioterroristDraw()
    {
        bioTerroristDraw(players[BioTerroristVolunteer], 1);
    }

	[PunRPC]
	public void RPC_airlift(string playerRoleKind, string cityName){
		Player player = findPlayer (playerRoleKind);
		City city = findCity (cityName);
		airlift (player, city);
	}

    [PunRPC]
    public void RPC_operationsExpertMove(string cityName, string cardName)
    {
        City theCity = findCity(cityName);
        CityCard theCard = (CityCard)findPlayerCard(cardName);
        operationsExpertMove(theCard, theCity);
    }

	[PunRPC]
	public void RPC_placeMarker(string cityName){
		City targetCity = findCity (cityName);
		placeMarker (targetCity);
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

    public void Build(string initialCityName, string cityCardName)
    {
        PhotonView.RPC("RPC_build", PhotonTargets.All, initialCityName, cityCardName);
    }

    public void Cure(string playerRoleKind, List<string> cardsToRemove, Color color){
        string[] cards = new string[cardsToRemove.Count];
        for(int i = 0; i<cardsToRemove.Count; i++)
        {
            cards[i] = cardsToRemove[i];
        }
        string diseaseColor = colorToString(color);
        PhotonView.RPC ("RPC_cure", PhotonTargets.All, playerRoleKind, diseaseColor, cards);
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
	

	public void NextPlayer(){
		PhotonView.RPC ("RPC_nextPlayer",PhotonTargets.All);
	}

	// Event Card
	public void Airlift()
	{

	}	

	public void BorrowedTime(){
		PhotonView.RPC ("RPC_borrowedTime",PhotonTargets.All);
	}

	public void OneQuietNight(){
		PhotonView.RPC ("RPC_oneQuietNight",PhotonTargets.All);
	}

	public void ResilientPopulation(string cardName){
		PhotonView.RPC ("RPC_resilientPopulation",PhotonTargets.All,cardName);
	}

    public void MobileHospital(string roleKind)
    {
        PhotonView.RPC("RPC_mobileHospital", PhotonTargets.All, roleKind);
    }

	public void Forecast(List<string> orderedCards){
		string[] cards = new string[6];
		for (int i=0; i<orderedCards.Count; i++){
			cards[i] = orderedCards[i];
		}
		PhotonView.RPC ("RPC_forecast",PhotonTargets.All,cards[0],cards[1],cards[2],cards[3],cards[4],cards[5]);
	}

    public void GovernmentGrant(string initialCity, string endCity)
    {
        PhotonView.RPC("RPC_governmentGrant", PhotonTargets.All, initialCity, endCity);
    }

    // Special Role Skills
    public void ContingencyPlannerPutCardOnTopOfRoleCard(string eventCardName)
    {
		PhotonView.RPC ("RPC_ContingencyPlannerPutCardOnTopOfRoleCard",PhotonTargets.All, eventCardName);
    }

    public void EpidemiologistShare()
    {
        //TO-DO
    }

	//ATTENTION: THIS HAS BEEND CHANGED
	public void ArchivistDraw()
	{
		PhotonView.RPC ("RPC_archivistDraw", PhotonTargets.All);
	}

    public void FieldOperativeSample(Color color)
    {
		PhotonView.RPC ("RPC_fieldOperativeSample",PhotonTargets.All, colorToString(color));
    }

	public void AskForEventCardPermission (string info, string targetRoleKind, string sourceRoleKind){
		PhotonPlayer targetPlayer = findPlayer (targetRoleKind).PhotonPlayer;
		PhotonView.RPC ("RPC_askForEventCardPermission",targetPlayer,info, sourceRoleKind);
	}

	public void InformEventCardPermissionResult(bool result, string sourceRoleKind){
		PhotonPlayer targetPlayer = findPlayer (sourceRoleKind).PhotonPlayer;
		PhotonView.RPC ("RPC_informEventCardPermissionResult", targetPlayer, result);
	}

	public void ReExaminedResearch(string playerRoleKind, string cardName){
		PhotonView.RPC ("RPC_reExaminedResearch", PhotonTargets.All, playerRoleKind, cardName);
	}

	public void NewAssignment(string targetPlayer, string newRoleKind){
		PhotonView.RPC ("RPC_newAssignment",PhotonTargets.All, targetPlayer, newRoleKind);
	}

	public void RemoteTreatment(string city1Name, string city2Name, Color color1, Color color2){
        string color1Name = colorToString(color1);
        string color2Name = colorToString(color2);
		PhotonView.RPC ("RPC_remoteTreatment", PhotonTargets.All, city1Name, city2Name, color1Name, color2Name);
	}

	public void CommercialTravelBan(string roleKindString){
		PhotonView.RPC ("RPC_commercialTravelBan", PhotonTargets.All, roleKindString);
	}

	public void DisplayReExaminedResearch(string roleKindString){
		PhotonPlayer targetPlayer = findPlayer (roleKindString).PhotonPlayer;
		PhotonView.RPC ("RPC_displayReExaminedResearch",targetPlayer);
	}

	public void DisplayNewAssignment(string roleKindString){
		PhotonPlayer targetPlayer = findPlayer (roleKindString).PhotonPlayer;
		PhotonView.RPC ("RPC_displayNewAssignment",targetPlayer);
		
	}

	public void FieldOperativePutBack(string playerRoleKind, Color color){
        string colorString = colorToString(color);
		PhotonView.RPC ("RPC_fieldOperativePutBack", PhotonTargets.All, playerRoleKind, colorString);
	}

	public void RapidVaccineDeployment(Color color, List<City> cities)
    {
		string colorString = colorToString (color);
		string[] cityNamesArray = new string[cities.Count];
		int i = 0;
		foreach (City city in cities) {
			cityNamesArray [i] = city.getCityName().ToString();
			i++;
		}

		PhotonView.RPC ("RPC_rapidVaccineDeployment", PhotonTargets.All, colorString, cityNamesArray);
    }

	public void AskPermissionDispatcher (string targetRoleKind, string request){
		PhotonPlayer targetPlayer = findPlayer (targetRoleKind).PhotonPlayer;
		PhotonView.RPC ("RPC_askPermissionDispatcher", targetPlayer, request );
	}

	public void InformDispatcher(bool request){
		PhotonPlayer targetPlayer = currentPlayer.PhotonPlayer;
		PhotonView.RPC ("RPC_informDispatcher", targetPlayer, request);
	}

	public void Airlift(string playerRoleKind, string cityName){
		PhotonView.RPC ("RPC_airlift",PhotonTargets.All, playerRoleKind, cityName);
	}

    public void OperationsExpertMove(string cityName, string cardName)
    {
        PhotonView.RPC("RPC_operationsExpertMove", PhotonTargets.All, cityName, cardName);
    }

	public void PlaceMarker(string cityName){
		PhotonView.RPC("RPC_placeMarker", PhotonTargets.All, cityName);
	}
    #endregion

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

	private void LoadPlayer(GameData savedGame){
		this.savedGame = savedGame;

		Debug.Log ("Start to load players....");

		//reconstruct role
		PhotonPlayer[] photonplayers = PhotonNetwork.playerList;
		Array.Sort (photonplayers, delegate(PhotonPlayer x, PhotonPlayer y) {
			return x.ID.CompareTo(y.ID);
		});
		foreach (PhotonPlayer player in photonplayers){
			Player newPlayer = new Player (player);
			players.Add (new Player(player));
		}


	}

	//initialzie game, set the first player as current player
	private void InitializeGame(){
		//load city
		challenge = (Challenge)Enum.Parse (typeof(Challenge), (string)PhotonNetwork.room.CustomProperties["Challenge"]);
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

		if(challenge == Challenge.Mutation || challenge == Challenge.MutationAndVirulentStrain){
			for(int i = 0; i<2; i++){
				infectionDiscardPile.Add(MutationCard.getMutationCard());
			}
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
        
        UnityEngine.Random.seed = (int)PhotonNetwork.room.CustomProperties["seed"];
        if (BioTerroristVolunteer == -1)
        {
            BioTerroristVolunteer =  UnityEngine.Random.Range(0, players.Count);
        }
        
        
        foreach (Player p in players) 
        {
            Role r;
            if (challenge == Challenge.BioTerroist)
            {
                r = (p != players[BioTerroristVolunteer]) ? new Role(selectRole()) : bioTerroristRole;
            }
            else
            {
                r = new Role(selectRole());
            }
            
            Pawn pawn;
            if (r.getRoleKind() == RoleKind.BioTerrorist)
            {
                pawn = Instantiate(bioterroristPawn, new Vector3(0, 0, 100), gameObject.transform.rotation);
            }
            else
            {
                pawn = Instantiate(playerPawn, new Vector3(0, 0, 100), gameObject.transform.rotation);
            }
			r.setPawn(pawn);
			p.setRole(r);
			pawn.transform.parent = GameObject.FindGameObjectWithTag("background").transform;
		}

		List<Color> dc = mapInstance.getDiseaseColor(challenge);
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

		
		if(challenge == Challenge.Mutation || challenge == Challenge.MutationAndVirulentStrain){
			shuffleMutatonEventCards();
		}
        setInitialHand();
        shuffleAndAddEpidemic();
		setUp();
        if (challenge == Challenge.BioTerroist)
        {
            bioTerroristDraw(players[BioTerroristVolunteer],2);
        }
		currentPhase = GamePhase.PlayerTakeTurn;
		//Debug.Log("Everything Complete");
		//Debug.Log("the role is" + me.getRoleKind().ToString());
	}

	/*
	LoadGame will:
	load all game info like outbreakrate, infection rate
	load all player hand card :done
	load all deck and discard pile
	load all city info: done
	load vs card: done
	load mutation event card: TODO
	load mutation card: TODO
	RoleKind for each player has been loaded in LoadPlayer, TODO: player position and pawn
	*/
	private void LoadGame(){
		challenge = savedGame.challenge;
		researchStationRemain = savedGame.remainingResearch;
		index = savedGame.infectionRateIndex;
		outbreaksValue = savedGame.outBreakRate;
		cities = new List<City>();
		EpidemicCard.getEpidemicCard ().setIntList (savedGame.EpidemicCardIntList);
		backGround = GameObject.FindGameObjectWithTag("background");
		backGround.transform.position += new Vector3(0.0001f, 0, 0);
		foreach (Transform t in backGround.transform)
		{
			if (t.GetComponent<City>() != null)
			{
				cities.Add(t.GetComponent<City>());
			}

		}

		using(var e1 = players.GetEnumerator())
		using(var e2 = savedGame.roleKindList.GetEnumerator())
		using(var e3 = savedGame.playerCardList.GetEnumerator())
		using(var e4 = savedGame.mobileHospitalActivated.GetEnumerator())
		using(var e6 = savedGame.hasCommercialTravelBanInfrontOfPlayer.GetEnumerator())
			
		{
			while(e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e6.MoveNext())
			{
				var curPlayer = e1.Current;
				var curRolekind = e2.Current;
				PlayerCardList playerHand = e3.Current;

				//EventCard eventCardOnTopOfRoleCard = EventCard.getEventCard((EventKind)Enum.Parse (typeof(EventKind), e5.Current));


				Role curRole = new Role(curRolekind);

				bool mobileHospitalActivated = e4.Current;
				curPlayer.setMobileHospitalActivated (mobileHospitalActivated);
				if (curRolekind == RoleKind.ContingencyPlanner && !savedGame.eventCardOnTopOfRoleCard.Equals("Null")) {
					curPlayer.setEventCardOnTopOfRoleCard (EventCard.getEventCard((EventKind)Enum.Parse (typeof(EventKind), savedGame.eventCardOnTopOfRoleCard)));
				}
				if(curRolekind == RoleKind.FieldOperative){
					curPlayer.setAllCubes(savedGame.FOcubes);
				}
				bool hasCommercialTravelBanInfrontOfPlayer = e6.Current;

				if (hasCommercialTravelBanInfrontOfPlayer) {
					curPlayer.setCommercialTravelBanTurn ();
				}
				//int commercialTravelBanTurn = e7.Current;
				//curPlayer.setCommercialTravelBanTurnValue (commercialTravelBanTurn);

				curPlayer.setRole (curRole);
				foreach (string s in playerHand.playerHand) {
					if (curRolekind != RoleKind.BioTerrorist){
						if (Enum.IsDefined (typeof(EventKind), s)) {
							curPlayer.addCard (EventCard.getEventCard((EventKind)Enum.Parse (typeof(EventKind), s)));
							Debug.Log ("Event card " + s);
						}
						if (Enum.IsDefined (typeof(CityName), s)) {
							curPlayer.addCard (new CityCard(findCity((CityName)Enum.Parse(typeof(CityName),s))));
							Debug.Log ("City card " + s);
						}
					}
					else{
						if (Enum.IsDefined (typeof(InfectionCard), s)) {
							curPlayer.addCard (new InfectionCard(findCity((CityName)Enum.Parse(typeof(CityName),s))));
							Debug.Log ("Bio-t has infection card: " + s);
						}
					}


				}
				// use item1 and item2
			}
		}

		Maps mapInstance = Maps.getInstance();
		//initialize infectionArray
		infectionArray = new int[]{2,2,2,3,3,4,4};

	
		numOfEpidemicCard = savedGame.difficulity;
		difficulty = savedGame.difficulity;
		me = FindPlayer(PhotonNetwork.player);

		currentPlayer = findPlayer(savedGame.currentPlayerRoleKind);
		Debug.Log ("current player is player" + currentPlayer.getRoleKind());


		playerCardDeck = convertToPlayerCardList(savedGame.playerCardDeck);
		playerDiscardPile = convertToPlayerCardList (savedGame.playerDiscardPile);
		infectionDeck = convertToInfecionCardList (savedGame.infectionCardDeck);
		infectionDiscardPile = convertToInfecionCardList (savedGame.infectionDiscardPile);

		foreach (PlayerCard p in playerCardDeck){
			AllHandCards.Add(p);
		}

		foreach (PlayerCard p in playerDiscardPile){
			AllHandCards.Add(p);
		}
			
		AllHandCards.Add(EpidemicCard.getEpidemicCard());

		foreach(Player p in players){
			foreach (PlayerCard pc in p.getHand()) {
				AllHandCards.Add (pc);
			}
		}

		//TODO : IMPORTANT! add playr hand to allhandcard
		Player bioTerrorist = null;

		if(challenge == Challenge.BioTerroist)
		{
			bioTerrorist = findPlayer (RoleKind.BioTerrorist);
		}

		foreach (Player p in players) 
		{
			Role r = p.getRole ();
			Pawn pawn = Instantiate(playerPawn, new Vector3(0, 0, 100), gameObject.transform.rotation);
			r.setPawn(pawn);
			p.setRole(r);
			pawn.transform.parent = GameObject.FindGameObjectWithTag("background").transform;
		}

		List<Color> dc = mapInstance.getDiseaseColor(challenge);
		foreach (Color c in dc)
		{
			Disease d = new Disease(c);
			diseases.Add(c, d);
		}

		using (var e = savedGame.diseaseInfoList.GetEnumerator ()) {
			e.MoveNext ();
			foreach (KeyValuePair<Color, Disease> entry in diseases) {
				Disease curDisease = entry.Value;
				curDisease.setCured(e.Current.cured);
				curDisease.setEradicated(e.Current.eradicated);
				curDisease.setNumOfDiseaseCubeLeft(e.Current.numberOfDiseaseCubesLeft);
				//for gui
				gameInfoController.changeDiseaseNumber (curDisease.getColor (), curDisease.getNumOfDiseaseCubeLeft ());

				Debug.Log (colorToString(entry.Key) + "has " + curDisease.getNumOfDiseaseCubeLeft ());
				e.MoveNext ();
			}
		}

		using (var e = savedGame.CityInfoList.GetEnumerator ()) {
			e.MoveNext ();
			foreach(City city in cities){
				city.restoreCityInfo (e.Current);
				e.MoveNext ();
			}
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

		currentPhase = savedGame.currentGamePhase;




		foreach (RoleKind rk in savedGame.roleKindList) {
			Debug.Log ("RoleKind is " + rk.ToString());
		}

		foreach(Player player in players){
			foreach (PlayerCard pc in player.getHand()) {
				Debug.Log (pc.GetType());
				//for gui
				if (!player.Equals(me))
				{

					playerPanel.addPlayerCardToOtherPlayer(player.getRoleKind(), pc);
				}
				else
				{
					//Debug.Log("add card to main player" + card.ToString());
					mainPlayerPanel.addPlayerCard(pc);
				}
			}
		}
	}
	#endregion


	#region basicOperation
	//drive
	private void drive(Player player, City destinationCity)
	{
        move(player, destinationCity);

		RoleKind rolekind = player.getRoleKind();

		
        if (player.getMobileHospitalActivated())
        {
            resolveContainmentSpecialist(destinationCity); //I realize I can use exactly the same function
        }
        if (player.getRoleKind() == RoleKind.BioTerrorist && !getBioTerrorist().getBioTerroristExtraDriveUsed())
        {
            getBioTerrorist().setbioTerroristExtraDriveUsed();
        }
        else
        {
            currentPlayer.decreaseRemainingAction();
        }
		
		record.drive(currentPlayer, destinationCity);
		//Debug.Log ("move succeed");
	}

	//take direct flight
	private void takeDirectFlight(Player player, CityCard card)
	{
        City destinationCity = card.getCity();
        move(player, destinationCity);
        
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
		currentPlayer.decreaseRemainingAction();
		record.directFlight(player, destinationCity);
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
	private void exchangeCard(RoleKind roleKind, CityCard cityCard, bool actionFree)
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

        if (!actionFree)
        {
            currentPlayer.decreaseRemainingAction ();
        }
		
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

		usedTreated = true;
		currentPlayer.decreaseRemainingAction();
		record.treat(currentPlayer, treatNumber, d, currentCity);
	}

	//cure
	private void cure(Player player, List<CityCard> cardsToRemove, Disease d)
	{
		if(cardsToRemove.Count==3 && currentPlayer.getRoleKind() == RoleKind.FieldOperative){// field operative
			currentPlayer.returnCubes(d, 3);
			gameInfoController.changeDiseaseNumber(d.getColor(), d.getNumOfDiseaseCubeLeft());
		}
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
		gameInfoController.cure(d.getColor());
		int num = d.getNumOfDiseaseCubeLeft();
        if (getNumStandardDiseasesCured() == 4 && (numOfPurpleCubesOnTheBoard() == 0 || diseases[Color.magenta].isCured()))
        {
            notifyGameWin();
        }
        
		if(num == MAX && d.getColor()!= Color.magenta)
		{
			d.isEradicated();
			gameInfoController.eradicate(d.getColor());
		}
        if (num == purpleMax && d.getColor() == Color.magenta)
        {
            d.isEradicated();
            notifyBioterroristLost(GameLostKind.PurpleDiseaseEradicated);
            gameInfoController.eradicate(d.getColor());
        }

        //UI TODO: set disease’s cure marker

        currentPlayer.decreaseRemainingAction();
        askForRapidVaccineDeployment(d.getColor());
    }

    private int getNumStandardDiseasesCured()
    {
        int total = 0;
        foreach(Disease d in diseases.Values)
        {
            if(d.getColor()!=Color.magenta && d.isCured())
            {
                total++;
            }
        }
        return total;
    }

	//pass
	private void endTurn()
	{
		if (currentPhase != GamePhase.PlayerTakeTurn)
			return;
		currentPhase = GamePhase.PlayerDrawCard;
        if(currentPlayer == players[BioTerroristVolunteer])
        {
            getBioTerrorist().refillDriveAction();
        }
		currentPlayer.refillAction();
		currentPlayer.setOncePerturnAction(false);
        currentPlayer.setMobileHospitalActivated(false);

		usedTreated = false;

        if (currentPlayer.getMobileHospitalActivated())
        {
            currentPlayer.setMobileHospitalActivated(false);
        }

		record.pass(currentPlayer);
		//Note that epidemic card is resolved in "draw" method
		//if there is no enough player cards in the deck, players lose the game
		
		if ((currentPlayer!= players[BioTerroristVolunteer] || Challenge.BioTerroist != challenge) && !draw(currentPlayer, 2))
		{
			return;
		}
		currentPhase = GamePhase.InfectCities;
		numOfInfection = 0;
	}

	private void askPermissionDispatcher(string request){
		moveOperation.askPermission (request);
	}

	private void informDispatcher(bool result){
		moveOperation.informResult (result);
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
		if(isChronicEffect()){
				infect(city, color, 2);
			}
		else if (!infect(city, color, 1))
		{
			return;
		}
		// for mutation challenge
		if((challenge == Challenge.Mutation || challenge == Challenge.MutationAndVirulentStrain)
		&& !diseases[Color.magenta].isEradicated()
		&& city.getCubeNumber(diseases[Color.magenta])>0){
			infect(city, Color.magenta, 1);
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
		if(oneQuietNightUsed){
			oneQuietNightUsed = false;
            PhotonView.RPC("RPC_nextPlayer", PhotonTargets.All);
            return;
        }
		//Debug.Log ("start infect city");
		passOperation.startInfection ();
		//nextPlayer();
	}

	private void notifyResolveEpidemic(){
		passOperation.notifyResolveEpidemic ();
	}

	private void placeMarker(City city)
	{
		if ( markersAvailable == 0) {
			city.removeMarker();
			markersAvailable++;
		}
		currentPlayer.getPlayerPawn().getCity().putMarker();
		markersAvailable--;
		currentPlayer.decreaseRemainingAction();

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
		} 

        if (Challenge.BioTerroist == challenge && destinationCity.getNumPlayers() > 1)
        {
            getBioTerrorist().spot();
        }

        if(pl1.getRoleKind() == RoleKind.Colonel)
        {
            colonelFlip();
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
                    int num = disease.getNumOfDiseaseCubeLeft();
                    if (num == MAX)
                    {
                        disease.eradicate();
                    }
                    Debug.Log(disease.getNumOfDiseaseCubeLeft());
                }
			}
    }


	#region eventCard
	private void forecast(List<InfectionCard> orderedCards){
		foreach(InfectionCard c in orderedCards){
			infectionDeck.Remove(c);
		}
		orderedCards.AddRange (infectionDeck);
		infectionDeck = orderedCards;
		dropEventCard (EventKind.Forecast);
		record.eventCard(currentPlayer,EventKind.Forecast);
	}

    private void governmentGrant(City initialCity, City c)
    {
        if (researchStationRemain == 0)
        {
            if (initialCity != null)
            {
                initialCity.setHasResearch(false);
            }
        }
        else
        {
            researchStationRemain--;
            gameInfoController.changeResearchNumber(researchStationRemain);
        }
        c.setHasResearch(true);
        dropEventCard(EventKind.GovernmentGrant);
		record.eventCard(currentPlayer,EventKind.GovernmentGrant);
    }


	private void askForEventCardPermission(string info, string sourcePlayerRoleKind){
		eventController.showAgreePanel (info, sourcePlayerRoleKind);
	}

	private void informEventCardPermissionResult(bool result){
		eventController.informResult (result);
	}

	public void askForRapidVaccineDeployment(Color color){
		Player cardHolder = findEventCardHolder (EventKind.RapidVaccineDeployment);
		if (cardHolder == me) {
            eventController.askForRapidVaccineDeployment(color);
		}
	}

    public void oneQuietNight(){
		oneQuietNightUsed = true;
		dropEventCard (EventKind.OneQuietNight);
		record.eventCard(currentPlayer,EventKind.OneQuietNight);
	}



	public void resilientPopulation(InfectionCard card){
		infectionDiscardPile.Remove (card);
		infectionDiscardUI.deleteCityCard (card.getName ().ToString ());
		dropEventCard (EventKind.ResilientPopulation);
		record.eventCard(currentPlayer,EventKind.ResilientPopulation);
	}

	public void airlift(Player pl1, City destination){
		move (pl1, destination);
		dropEventCard (EventKind.Airlift);
		record.eventCard(currentPlayer,EventKind.Airlift);
	}

	public void borrowedTime(){
		// if (findEventCardHolder(EventKind.BorrowedTime) == currentPlayer) {
		currentPlayer.increaseRemainingAction (2);
		dropEventCard (EventKind.BorrowedTime);
		record.eventCard(currentPlayer,EventKind.BorrowedTime);
		
	}

	public void specialOrders(){
		dropEventCard (EventKind.SpecialOrders);
		record.eventCard(currentPlayer,EventKind.SpecialOrders);
	}

	/* 
	Remove cubes of color c in cities.
	Return 1 if 5 cubes are removed, return 0 if less than 5.
	*/
	public int rapidVaccineDeployment(Color c, List<City> cities){
		int ctr = 0;
		dropEventCard (EventKind.RapidVaccineDeployment);
		record.eventCard(currentPlayer,EventKind.RapidVaccineDeployment);
		foreach (City city in cities){
			city.removeCubes(diseases[c], 1);
        	diseases[c].incrementNumOfDiseaseCubeLeft();
			gameInfoController.changeDiseaseNumber(diseases[c].getColor(), diseases[c].getNumOfDiseaseCubeLeft());
			ctr++;
			if(ctr>5){
				return 1;
			}
		}
		return 0;
	}

    public void mobileHospital(Player pl1)
    {
        pl1.setMobileHospitalActivated(true);
        dropEventCard(EventKind.MobileHospital);
		record.eventCard(currentPlayer,EventKind.MobileHospital);
    }

    public void remoteTreatment(City city1, City city2, Color color1, Color color2)
    {
        city1.removeCubes(diseases[color1], 1);
        diseases[color1].incrementNumOfDiseaseCubeLeft();
        city2.removeCubes(diseases[color2], 1);
        diseases[color2].incrementNumOfDiseaseCubeLeft();
        dropEventCard(EventKind.RemoteTreatment);
		record.eventCard(currentPlayer,EventKind.RemoteTreatment);
    }

    public void reExaminedResearch(Player pl, CityCard card)
    {
        if (!playerDiscardPile.Contains(card))
        {
            Debug.Log("Does not contain that card: Games.cs ReExaminedResearch()");
            return;
        }
        dropEventCard(EventKind.ReExaminedResearch);
		record.eventCard(currentPlayer,EventKind.ReExaminedResearch);
        if(pl.getHandLimit() > pl.getHandSize())
        {
            playerDiscardPile.Remove(card);
			playerDiscardUI.deleteCityCard (card.getName ().ToString ());
            pl.addCard(card);
			//for gui
			if (!pl.Equals(me))
			{

				playerPanel.addPlayerCardToOtherPlayer(pl.getRoleKind(), card);
			}
			else
			{
				//Debug.Log("add card to main player" + card.ToString());
				mainPlayerPanel.addPlayerCard(card);
			}

        }
    }

    public void commercialTravelBan(Player pl)
    {
        dropEventCard(EventKind.CommercialTravelBan);
		record.eventCard(currentPlayer,EventKind.CommercialTravelBan);
        pl.setCommercialTravelBanTurn();
        infectionRate = 1;
        gameInfoController.displayInfectionRate();
    }

	private void displayReExaminedResearch(){
		eventController.doReExamineResearch ();
	}

	private void displayNewAssignment(){
		eventController.doNewAssignment ();
	}

	#endregion
   

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
		RoleKind old = pl1.getRoleKind ();
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
        r2.setPawn(pawn);
		city.addPawn (pawn);

		if (pl1 == me) {
			playerPanel.swapRoleSelf (roleKind);
		} else {
			playerPanel.swapRoleOther (old,roleKind);
			playerSelect.swapRole (old, roleKind);
		}

        return true;
	}

	public void newAssignment(Player pl1, RoleKind roleKind){
		if (!swapRole (pl1, roleKind)) {
			return;
		}
		dropEventCard (EventKind.NewAssignment);
		record.eventCard(currentPlayer,EventKind.NewAssignment);
	}

	public void dropEventCard(EventKind eKind){
		//TODO:call front end
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
                if(pl.hasEventCardOnTopOfRoleCard()){
                    if (pl.getEventCardOnTopOfRoleCard().getEventKind() == eKind)
                    {
                        pl.removeEventCardOnTopOfRoleCard();
                    }
                    else
                    {
                        Debug.Log("No player is holding this card. Game.cs: dropEventCard(EventKind)");
                    }
                }
            }
		}
        else
        {
            pl.removeCard (eCard);
			if (!pl.Equals(me))
			{

				playerPanel.deletePlayerCardFromOtherPlayer(pl.getRoleKind(), eCard);
			}
			else
			{
				mainPlayerPanel.deleteEventCard (eCard.getEventKind());
			}
            if (EventKind.CommercialTravelBan != eKind)
            {
                playerDiscardPile.Add(eCard);
            }
		    
        }
		
	}

    public void contingencyPlannerPutCardOnTopOfRoleCard(EventCard card)
    {
        if (!playerDiscardPile.Contains(card))
        {
            Debug.Log("Discard pile does not contain this card.");
        }
        playerDiscardPile.Remove(card);
        currentPlayer.setEventCardOnTopOfRoleCard(card);
        
		currentPlayer.decreaseRemainingAction();
    }

    public RoleKind selectRole()
    {	
		//set random seed
		UnityEngine.Random.seed = (int)PhotonNetwork.room.CustomProperties["seed"];

        
        int num = 8;

        if (hasDLC)
        {
            num = 14;
        }
        //Testing only
   
		RoleKind testRole = RoleKind.Epidemiologist;
        if (!roleKindTaken.Contains(testRole)){
            roleKindTaken.Add(testRole);
            return testRole;
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
            if (p != players[BioTerroristVolunteer] || Challenge.BioTerroist != challenge)
            {
                draw(p, cardNeeded);
            }
        }
    }

    private void shuffleMutatonEventCards(){
		playerCardDeck.Add(new MutationEventCard(MutationEvent.Threatens));
		playerCardDeck.Add(new MutationEventCard(MutationEvent.Spreads));
		playerCardDeck.Add(new MutationEventCard(MutationEvent.Intensifies));
		Collection.Shuffle(playerCardDeck);
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
		
    public bool containsEventCardInDiscardPile()
    {
        foreach(PlayerCard p in playerDiscardPile)
        {
            if(p.getType() == CardType.EventCard)
            {
                return true;
            }
        }
        return false; 
    }

    public List<EventCard> getEventCardsFromDiscardPile()
    {
        List<EventCard> result = new List<EventCard>();

        foreach (PlayerCard p in playerDiscardPile)
        {
            if (p.getType() == CardType.EventCard)
            {
                result.Add((EventCard)p);
            }
        }

        return result; 
    }

    public bool containsSpecificCityCardInDiscardPile(City city)
    {
        foreach (PlayerCard p in playerDiscardPile)
        {
            if (p.getType() == CardType.CityCard && ((CityCard)p).getCity() == city)
            {
                return true;
            }
        }
        return false; 
    }

    private bool resolveEpidemic()
    {
		PhotonView.RPC ("RPC_notifyResolveEpidemic",PhotonTargets.All);
        resolvingEpidemic = true;
        if(infectionRate < 4)
        {
            bool infectionRateDummied = false;
            foreach(Player pl in players)
            {
                if (pl.hasEventCardInFront())
                {
                    infectionRateDummied = true;
                }
            }
            index++;
            if (!infectionRateDummied)
            {
                infectionRate = infectionArray[index];
            }
            
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

		// Virulent Strain part: need to be before shuffle discard pile
		if(challenge == Challenge.VirulentStrain || challenge == Challenge.BioTerroistAndVirulentStrain){
			//set Virulent Strain
			if (VirulentStrainDisease == null){
				int min = 25;
				foreach (Disease d in diseases.Values){
					if(d.getNumOfDiseaseCubeLeft()<min){
						min = d.getNumOfDiseaseCubeLeft();
						VirulentStrainDisease = d;
					}
				}
			}

			// apply Virulent Strain Epidemic Effects
			switch(currentVirulentStrainEpidemicEffects){
			case VirulentStrainEpidemicEffects.ChronicEffect:
				chronicEffect = true;
				break;
			case VirulentStrainEpidemicEffects.ComplexMolecularStructure:
				complexMolecularStructure = true;
				break;
			case VirulentStrainEpidemicEffects.GovernmentInterference:
				governmentInterference = true;
				break;
			case VirulentStrainEpidemicEffects.HiddenPocket:
				if (VirulentStrainDisease.isEradicated()){
					// TODO: flip cure maker!
					foreach(InfectionCard ic in infectionDeck){
						if(ic.getColor() == VirulentStrainDisease.getColor()){
							ic.getCity().addCubes(VirulentStrainDisease,1);
						}
					}
				}
				break;
			default:
				Debug.Log("no Virulent Strain Epidemic Effects");
				break;
			}
		}

        Collection.Shuffle<InfectionCard>(infectionDiscardPile);
        placeInfectionDiscardPileOnTop();

        resolvingEpidemic = false;
        return true;
    }
	
	public bool isChronicEffect(){
		return (challenge == Challenge.VirulentStrain || challenge == Challenge.BioTerroistAndVirulentStrain)
			&& VirulentStrainDisease.getColor() == getCurrentColor()
			&& chronicEffect == true;
	}
	
	public bool isComplexMolecularStructure(){
		return (challenge == Challenge.VirulentStrain || challenge == Challenge.BioTerroistAndVirulentStrain)
			&& VirulentStrainDisease.getColor() == getCurrentColor()
			&& complexMolecularStructure == true
			&& VirulentStrainDisease.isCured() == false;
		}

	public bool isGovernmentInterference(){
		return (challenge == Challenge.VirulentStrain || challenge == Challenge.BioTerroistAndVirulentStrain)
			&& VirulentStrainDisease.getColor() == getCurrentColor()
			&& governmentInterference == true
			&& usedTreated == false;
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

        if ((hasQS /*&&isCured*/) || hasMedic || isEradicated) return true;

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
				if(challenge == Challenge.VirulentStrain || challenge == Challenge.BioTerroistAndVirulentStrain){
					currentVirulentStrainEpidemicEffects = ((EpidemicCard)card).getVirulentStrainEpidemicEffects();
				}
                resolveEpidemic();
            }
            else if (card.getType() == CardType.MutationEventCard)
            {
				switch (((MutationEventCard)card).getMutationEvent()) {
				case MutationEvent.Threatens:
					if(!diseases[Color.magenta].isEradicated()){
						InfectionCard mCard = drawBottomInfectionDeck();
						City mCity = mCard.getCity();
						infect(mCity, Color.magenta, 3);
						infectionDiscardPile.Add(mCard);
					}
					break;
				case MutationEvent.Spreads:
					if(!diseases[Color.magenta].isEradicated()){
						for(int j=0; j<3; j++){
							InfectionCard mCard = drawBottomInfectionDeck();
							City mCity = mCard.getCity();
							infect(mCity, Color.magenta, 1);
							infectionDiscardPile.Add(mCard);
						}
					}
					break;
				case MutationEvent.Intensifies:
					foreach (City c in cities){
						if(c.getCubeNumber(diseases[Color.magenta]) == 2){
							infect(c, Color.magenta, 1);
						}
					}
					break;
				default:
					Debug.Log("Error: no mutation event kind");
					break; 
				}
                
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

    public int numOfPurpleCubesOnTheBoard()
    {
        return 12 - diseases[Color.magenta].getNumOfDiseaseCubeLeft();
    }

    public bool bioTerroristCanDraw()
    {
        return false;
    }

    private void bioTerroristDraw(Player pl, int amount)
    {
        for (int i=0; i<amount; i++)
        {
            if (infectionDeck.Count > 0)
            {
                InfectionCard card = infectionDeck[0];
                infectionDeck.Remove(card);
                record.draw(pl, card);
            }
            else
            {
                Debug.Log("no card to draw for bio");
                break;
            }
            
        }
        
    }

    private void operationsExpertMove(CityCard card, City c)
    {
        currentPlayer.removeCard(card);
        playerDiscardPile.Add(card);
        move(currentPlayer, c);
        currentPlayer.decreaseRemainingAction();
    }

    private void operativeExpertBuild(City initialCity)
    {
        if (researchStationRemain == 0)
        {
            if (initialCity != null)
            {
                initialCity.setHasResearch(false);
            }
        }
        else
        {
            researchStationRemain--;
            gameInfoController.changeResearchNumber(researchStationRemain);

        }
        City currentCity = currentPlayer.getPlayerPawn().getCity();
        currentCity.setHasResearch(true);
        record.build(currentCity);
        currentPlayer.decreaseRemainingAction();
    }

    public List<InfectionCard> troubleShooter()
    {
        List<InfectionCard> cardsToView = new List<InfectionCard>();
        for (int i=0;i<infectionRate; i++)
        {
            if (infectionDeck.Count > i)
            {
                cardsToView.Add(infectionDeck[i]);
            }
            
        }

        return cardsToView;
    }

    private void colonelFlip()
    {
        City city = currentPlayer.getPlayerPawn().getCity();
        if (city.getMarker() == 1) {
            city.flipMarkerTo(2);
        }
    }

    private void localInitiative(City city)
    {
        if (markersAvailable > 0)
        {
            markersAvailable--;
            city.putMarker();
            dropEventCard(EventKind.LocalInitiative);
        }
        else
        {
            Debug.Log("No more marker");
        }
    }

    public int getMarkersLeft()
    {
        return markersAvailable;
    }

    private void colonelPlaceMarker(CityCard card, City city)
    {
        if (markersAvailable > 0)
        {
            markersAvailable--;
            city.putMarker();
			currentPlayer.removeCard(card);
			playerDiscardPile.Add(card);	
            currentPlayer.decreaseRemainingAction();
        }
        else
        {
            Debug.Log("No more marker");
        }
    }

    public void bioTerroristCapture()
    {
        if (currentPlayer.getPlayerPawn().getCity() != players[BioTerroristVolunteer].getPlayerPawn().getCity())
        {
            Debug.Log("Not in the same city, cannot capture: Game.cs capture()");
        }
        getBioTerrorist().setCaptured();
        foreach(PlayerCard card in players[BioTerroristVolunteer].getHand())
        {
            if(card.getType() != CardType.InfectionCard)
            {
                Debug.Log("Has invalid card: Game.cs capture()");
            }

            infectionDiscardPile.Add((InfectionCard)card);
        }

        players[BioTerroristVolunteer].dropAllCards();
        currentPlayer.decreaseRemainingAction();
    }

    public BioTerrorist getBioTerrorist()
    {
        return bioTerroristRole;
    }

    public void bioTerroristSabotage(InfectionCard card)
    {
        City city = players[BioTerroristVolunteer].getPlayerPawn().getCity();
        if (!city.getHasResearch())
        {
            Debug.Log("No reseach in that city: Game.cs BioTerrorist Sabotage");
        }
        if(card.getColor() != city.getColor())
        {
            Debug.Log("Color does not match: Game.cs BioTerrorist Sabotage");
        }
        players[BioTerroristVolunteer].removeCard(card);
        infectionDiscardPile.Add(card);
        players[BioTerroristVolunteer].getPlayerPawn().getCity().setHasResearch(false);
        researchStationRemain++;

    }
    
    public void bioTerroristInfectLocally()
    {
        BioTerrorist bioTerrorist = getBioTerrorist();
        if (bioTerrorist.getinfectLocallyUsed())
        {
            return;
        }
        if (bioTerrorist.getIsCaptured() || bioTerrorist.getIsSpotted())
        {
            bioTerrorist.useInfectLocally();
        }

        players[BioTerroristVolunteer].getPlayerPawn().getCity().addCubes(diseases[Color.magenta],1);

        players[BioTerroristVolunteer].decreaseRemainingAction();

    }

    public void bioTerroristInfectRemotely(InfectionCard card)
    {
        BioTerrorist bioTerrorist = getBioTerrorist();
        if (bioTerrorist.getInfectRemotelyUsed())
        {
            return;
        }
        if (bioTerrorist.getIsCaptured() || bioTerrorist.getIsSpotted())
        {
            bioTerrorist.useInfectRemotely();
        }

        card.getCity().addCubes(diseases[Color.magenta], 1);
        players[BioTerroristVolunteer].removeCard(card);
        infectionDiscardPile.Add(card);
        players[BioTerroristVolunteer].decreaseRemainingAction();

    }

    private void bioTerroristMove(Player pl, City destinationCity)
    {
        Pawn p = pl.getPlayerPawn();
        p.setCity(destinationCity);
        p.getCity().removePawn(p);
        destinationCity.addPawn(p);
        if (Challenge.BioTerroist == challenge && destinationCity.getNumPlayers() > 1)
        {
            getBioTerrorist().spot();
        }
    }

    private void bioTerroristDirectFlight(InfectionCard card)
    {
        players[BioTerroristVolunteer].removeCard(card);
        infectionDiscardPile.Add(card);
        bioTerroristMove(players[BioTerroristVolunteer], card.getCity());
        announceAirportSighting();
        players[BioTerroristVolunteer].decreaseRemainingAction();
    }

    private void bioTerroristCharterFlight(InfectionCard card, City city)
    {
        players[BioTerroristVolunteer].removeCard(card);
        infectionDiscardPile.Add(card);
        bioTerroristMove(players[BioTerroristVolunteer], city);
        announceAirportSighting();
        players[BioTerroristVolunteer].decreaseRemainingAction();
    }

    private void bioTerroristEscape(InfectionCard card)
    {
        bioTerroristDirectFlight(card);
    }

    private void announceAirportSighting()
    {
        //TBW TODO
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

	public Color getCurrentColor(){
		return currentPlayer.getPlayerPawn().getCity().getColor();
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
        if (currentPlayer.hasEventCardInFront())
        {
            if(currentPlayer.hasEventCardInFront())
            {
                infectionRate = infectionArray[index]; 
                currentPlayer.terminateCommercialTravelBanTurn();
            }
        }
        currentPhase = GamePhase.PlayerTakeTurn;
		//Debug.Log (players.IndexOf(currentPlayer));
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

	/*
	For Archivist draw citycard from discard pile only!
	 */
	private void archivistDraw(){
		Player player = currentPlayer;
		RoleKind rk = player.getRoleKind();
		if (rk!=RoleKind.Archivist){
			return;
		}
        Pawn p = player.getPlayerPawn();
        City currentCity = p.getCity();

		PlayerCard card = playerDiscardPile.Find(x => ((CityCard)x).getCity() == currentCity);
		if (card == null){
			return;
		}

		playerDiscardPile.Remove(card);
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
        currentPlayer.decreaseRemainingAction();
		player.setOncePerturnAction(true);
	}

	/*
	TODO: for EPIDEMIOLOGIST, once per turn
	 */
	private void epidemiologistShare(CityCard card){
        exchangeCard(RoleKind.Epidemiologist, card,true);
        currentPlayer.removeOneAction();
    }


	/*
	for Field Operative, once per turn
	 */
	private void fieldOperativeSample(Disease d){
		Player player = currentPlayer;
		RoleKind rk = player.getRoleKind();
		if (rk!=RoleKind.FieldOperative){
			return;
		}
        City currentCity = player.getPlayerPawn().getCity();

		
		currentCity.removeCubes(d, 1);
		player.putCube(d,1);
        
		if(d.getNumOfDiseaseCubeLeft() == MAX && d.isCured())
		{
			d.isEradicated();
		}

		player.setOncePerturnAction(true);
		player.decreaseRemainingAction();
	}
	
    private void fieldOperativePutBack(Player pl, Color c)
    {
        Disease d = diseases[c];
        pl.returnCubes(d,1);
		gameInfoController.changeDiseaseNumber(d.getColor(), d.getNumOfDiseaseCubeLeft());
    }

    #region notify methods
    // to do: inform the player that they lose the game
    private void notifyGameLost(GameLostKind lostKind)
    {
        if(Challenge.BioTerroist == challenge && numOfPurpleCubesOnTheBoard()>0 && !diseases[Color.magenta].isEradicated())
        {
            notifyBioterroristWin();
        }
        setGamePhase(GamePhase.Completed);
    }

    private void notifyBioterroristWin()
    {
        //BOWEN TODO
    }

    private void notifyBioterroristLost(GameLostKind lostKind)
    {
        //BOWEN TODO
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
		
        currentCity.setHasResearch(true);
		record.build(currentCity);
		
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

    
	#region findMethod
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


	public Pawn FindPlayerPawnWithRoleKind(RoleKind roleKind){
		foreach (Player player in players) {
			if (player.getRoleKind ().Equals (roleKind)) {
				return player.getPlayerPawn ();
			}
		}
		return null;
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

	public InfectionCard findInfectionCard(String cardName){
		foreach (InfectionCard ic in infectionDeck){
			if(ic.getName() == cardName){
				return ic;
			}
		}
		foreach (InfectionCard ic in infectionDiscardPile){
			if(ic.getName() == cardName){
				return ic;
			}
		}
		Debug.Log("InfectionCard not found. Class: Game.cs : findInfectionCard");
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

	#region getMethods
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

	public Challenge getChallenge(){
		return challenge;
	}

	public Disease getDisease(Color color)
	{
		return diseases[color];
	}

	public Dictionary<Color, Disease> getDiseases(){
		return diseases;
	}

	public List<PlayerCard> getPlayerDiscardPile(){
		return playerDiscardPile;
	}

	public List<string> getPlayerCardDeckString(){
		return cardListToStringList(playerCardDeck);
	}

	public List<string> getPlayerDiscardPileString(){
		return cardListToStringList(playerDiscardPile);
	}

	public List<string> getInfectionDeckString(){
		return cardListToStringList(infectionDeck.Cast<PlayerCard>().ToList());
	}

	public List<string> getInfectionDiscardPileString(){
		return cardListToStringList(infectionDiscardPile.Cast<PlayerCard>().ToList());
	}

	public List<string> getAllHandCards(){
		return cardListToStringList (AllHandCards);
	}

	public List<string> getUnusedRole(){
		List<string> unusedRole = new List<string>();
		foreach(RoleKind rk in Enum.GetValues(typeof(RoleKind))){
			if (!roleKindTaken.Contains(rk)){
				unusedRole.Add(rk.ToString());
			}
		}
		return unusedRole;
	}


	private static List<string> cardListToStringList(List<PlayerCard> inputCards){
		List<string> output = new List<string> ();
		foreach (PlayerCard pc in inputCards) {
			output.Add (pc.getName());
		}
		return output;
	}
	#endregion

	#region convertMethod
	private List<PlayerCard> convertToPlayerCardList(List<string> input){
		List<PlayerCard> output = new List<PlayerCard> ();
		foreach(string s in input){
			if (s.Equals ("Epidemic")) {
				//Debug.Log ("Found an epidemic card");
				output.Add (EpidemicCard.getEpidemicCard ());
			}
			else if (Enum.IsDefined (typeof(EventKind), s)){
				//Debug.Log ("Found an event card");
				output.Add (EventCard.getEventCard((EventKind)Enum.Parse (typeof(EventKind), s)));
			}
			else if (Enum.IsDefined (typeof(CityName), s)){
				//Debug.Log ("Found a city card");
				output.Add (new CityCard(findCity((CityName)Enum.Parse(typeof(CityName),s))));
			}
			else if (Enum.IsDefined(typeof(MutationEvent),s)){
				output.Add (new MutationEventCard((MutationEvent)Enum.Parse (typeof(MutationEvent), s)));
			}
			else{
				Debug.Log("Unknown Card in PlayerCard pile");
			}
		}
		return output;
	}

	private List<InfectionCard> convertToInfecionCardList(List<string> input){
		List<InfectionCard> output = new List<InfectionCard> ();
		foreach (string s in input) {
			if (Enum.IsDefined (typeof(CityName), s)) {
				//Debug.Log ("Found an infection card");
				output.Add (new InfectionCard (findCity((CityName)Enum.Parse (typeof(CityName), s))));
			} 
			else if (s.Equals("MuatationCard")){
				output.Add (MutationCard.getMutationCard());
			}else {
				Debug.Log ("Unknown card in InfectionCard pile");
			}
		}
		return output;
	}

    public static string colorToString(Color color)
    {
        if (color == Color.red)
        {
            return "red";
        }
        else if (color == Color.blue)
        {
            return "blue";
        }
        else if (color == Color.black)
        {
            return "black";
        }

        else if (color == Color.yellow)
        {
            return "yellow";
        }

        else if(color == Color.magenta)
        {
            return "purple";
        }

        else
        {
            Debug.Log("Wrong color type");
            return "";
        }
    }

    public static Color stringToColor(string color)
    {
        if (color.Equals("red"))
        {
            return Color.red;
        }
        else if (color.Equals("blue"))
        {
            return Color.blue;
        }
        else if (color.Equals("yellow"))
        {
            return Color.yellow;
        }
        else if (color.Equals("black"))
        {
            return Color.black;
        }
        else if (color.Equals("purple"))
        {
            return Color.magenta;
        }
        else
        {
            Debug.Log("Wrong string value");
            return Color.white;
        }
    }
	#endregion

	#region saveAndQuit
	public void save(string name){
		SaveAndLoadManager.SaveGameData (Instance, name);
	}

	[PunRPC]
	public void RPC_quitAll(){
		quit ();
	}
	public void quit(){
		Debug.Log ("Some one left the room, game over");
		PhotonNetwork.LeaveRoom ();
		SceneManager.LoadScene ("Lobby");
	}

	public void quitAll(){
		PhotonView.RPC ("RPC_quitAll",PhotonTargets.All);
	}

	#endregion





}
