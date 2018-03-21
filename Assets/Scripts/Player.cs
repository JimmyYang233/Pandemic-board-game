using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
	public readonly PhotonPlayer PhotonPlayer;

	private int maximumAction = -1;
	private int remainingAction = 0;
	private bool oncePerTurnAction = false;
    private Role curRole = null;
	private List<PlayerCard> handCard = new List<PlayerCard>();
    private bool mobileHospitalActivated = false;

	//connect this player with the PhotonPlayer
	//not: in the future, we need to add argument"role" to constructor
	public Player(PhotonPlayer photonPlayer)
	{
		this.PhotonPlayer = photonPlayer;

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
    
	public void setRole(Role r)
	{
		this.curRole = r;
		if(compareRole(RoleKind.Generalist)){
			this.maximumAction = 5;
		}
		else{
			this.maximumAction = 4;
		}
		refillAction();

		if(compareRole(RoleKind.FieldOperative) || compareRole(RoleKind.Archivist) || compareRole(RoleKind.Epidemiologist) || compareRole(RoleKind.OperationsExpert)){
			oncePerTurnAction = true;
		}
		else{
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
