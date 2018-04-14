using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player {
	public readonly PhotonPlayer PhotonPlayer;

	private int maximumAction = -1;
	private int remainingAction = 0;
	private bool oncePerTurnAction = false;
    private Role curRole = null;
	private List<PlayerCard> handCard = new List<PlayerCard>();
    private bool mobileHospitalActivated = false;
    private EventCard eventCardOnTopOfRoleCard = null;
    private bool hasCommercialTravelBanInfrontOfPlayer = false;//TODO new field
    private int CommercialTravelBanTurn = 0;//TODO new field
    private int[] cubes = new int[5]; //TODO new field //0 blue 1 red 2 yellow 3 black 4 magenta
    
    public int[] getAllCubes()
    {
        return cubes;
    }

    public int getblueCubesHolded()
    {
        return cubes[0];
    }

    public int getredCubesHolded()
    {
        return cubes[1];
    }
    public int getyellowCubesHolded()
    {
        return cubes[2];
    }
    public int getblackCubesHolded()
    {
        return cubes[3];
    }
    public int getmagentaCubesHolded()
    {
        return cubes[4];
    }

    public int getTotalNumberOfCubesHolded()
    {
        return cubes.Length;
    }

    public void putCube(int index, int amount)
    {
        cubes[index] += amount;
    }

    public void returnCubes(int index, int amount)
    {
        if (cubes[index]>=amount)
        {
            cubes[index] -= amount;
        }
        else
        {
            Debug.Log("not enough");
        }
    }

    public bool hasEventCardInFront()
    {
        return hasCommercialTravelBanInfrontOfPlayer;
    }

	public void setCommercialTravelBanTurnValue(int input){
		CommercialTravelBanTurn = input;
	}

    public void setCommercialTravelBanTurn()
    {
        hasCommercialTravelBanInfrontOfPlayer = true;
        CommercialTravelBanTurn = 1;
    }

    public int getCommercialTravelBanTurn()
    {
        return CommercialTravelBanTurn;
    }

    public void decrementCommercialTravelBanTurn()
    {
        CommercialTravelBanTurn--;
    }

    public void terminateCommercialTravelBanTurn()
    {
        hasCommercialTravelBanInfrontOfPlayer = false;
    }

    //connect this player with the PhotonPlayer
    //not: in the future, we need to add argument"role" to constructor
    public Player(PhotonPlayer photonPlayer)
	{
		this.PhotonPlayer = photonPlayer;

	}

    public void setEventCardOnTopOfRoleCard(EventCard card)
    {
        eventCardOnTopOfRoleCard = card;
    }

    public int getHandSize()
    {
        return handCard.Count;
    }

    public void removeEventCardOnTopOfRoleCard()
    {
        eventCardOnTopOfRoleCard = null;
    }

	public EventCard getEventCardOnTopOfRoleCard(){
		return eventCardOnTopOfRoleCard;
	}

    public bool hasEventCardOnTopOfRoleCard()
    {
        return eventCardOnTopOfRoleCard != null;
    }

    public void setMobileHospitalActivated(bool toggle)
    {
        mobileHospitalActivated = toggle;
    }

    public bool getMobileHospitalActivated()
    {
        return mobileHospitalActivated;
    }

    // public boolean consentRequest(){

    // }

    public RoleKind getRoleKind()
    {
        return curRole.getRoleKind();
    }

	private bool compareRole(RoleKind role)
    {
        if (curRole == null)
        {
            return false;
        }
        return curRole.getRoleKind() == role;
	}

	public int getHandLimit(){
		if(curRole.getRoleKind() == RoleKind.Archivist){
			return 8;
		}
		else{
			return 7;
		}
	}
	
    public bool hasEnoughCard(Color color)
    {
        int cardNeeded = getNumberOfCardNeededToCure();

        foreach (PlayerCard p in handCard)
        {
            if(p.getType() == CardType.CityCard && ((CityCard)p).getColor() == color)
            {
                cardNeeded--;
            }
        }
        return cardNeeded <= 0;
    }

    public int getNumberOfCardNeededToCure()
    {
        int cardNeeded = 5;
        if (compareRole(RoleKind.Scientist))
        {
            cardNeeded = 4;
        }

        return cardNeeded;
    }

    public int getNumberOfCardNeededToCure(Color color)
    {
        int cardNeeded = 5;
        if (compareRole(RoleKind.Scientist))
        {
            cardNeeded = 4;
        }
        Game game = Game.Instance;
        if(game.isComplexMolecularStructure()){
                cardNeeded++;
            }

        return cardNeeded;
    }

    public int getHandCardNumber(){
		return handCard.Count;
	}

	public Pawn getPlayerPawn(){
		return curRole.getPawn();
	}

	public void addCard(PlayerCard card){

		handCard.Add(card);
	}

    public void removeCard(PlayerCard card){
		handCard.Remove(card);
	}

    public void dropAllCards()
    {
        handCard.Clear();
    }

    public List<PlayerCard> getHand()
    {
        List<PlayerCard> tmp = new List<PlayerCard>(handCard);
        return tmp;
    }

    public bool containsCityCard()
    {
        foreach(PlayerCard card in handCard)
        {
            if(card.getType() == CardType.CityCard)
            {
                return true;
            }
        }
        return false;
    }

    public bool containsSpecificCityCard(City c)
    {
        foreach(PlayerCard card in handCard)
        {
            if (card.getType() == CardType.CityCard)
            {
                CityCard aCityCard = (CityCard)card;
                if (aCityCard.getCity() == c)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public CityCard getCard(City c)
    {
        foreach(PlayerCard card in handCard)
        {
            if (card.getType() == CardType.CityCard)
            {
                CityCard aCard = (CityCard)card;
                if (aCard.getCity() == c)
                {
                    return aCard;
                }
            }
        }
        return null;
    }

	public void decreaseRemainingAction(){
		remainingAction--;
	}

	public void increaseRemainingAction(int num){
		remainingAction+=num;
	}

	public void refillAction(){
		remainingAction = maximumAction;
	}

	public int getRemainingAction()
    {
        return remainingAction;
    }

	public void removeOneAction(){
		oncePerTurnAction = false;
	}

	public void setOncePerturnAction(bool toggle){
		oncePerTurnAction = toggle;
	}
    
    public bool getOncePerTurnAction(){
        return oncePerTurnAction;
    }

	public void setRole(Role r)
	{
		this.curRole = r;
		if(compareRole(RoleKind.Generalist)){
			this.maximumAction = 5;
		}
		else if(compareRole(RoleKind.BioTerrorist)){
            maximumAction = 2;
        }
        else{
			this.maximumAction = 4;
		}
		refillAction();

		if(compareRole(RoleKind.FieldOperative) || compareRole(RoleKind.Archivist) || compareRole(RoleKind.Epidemiologist) || compareRole(RoleKind.OperationsExpert)){
			oncePerTurnAction = false;
		}
	}
		
	public int getMaxnumAction(){
		return maximumAction;
	}

    public Role getRole()
    {
        return curRole;
    }
}
