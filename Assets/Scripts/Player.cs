using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
	private int maximumAction;
	private int remainingAction = 0;
	private bool oncePerTurnAction = false;
    private Role curRole = null;
	private List<PlayerCard> handCard = new List<PlayerCard>();
    private string username;

	public Player(User user)
	{
        username = user.getUsername();
		curRole = user.getRole();
		if(compareRole(RoleKind.Generalist)){
			maximumAction = 5;
		}
		else{
			maximumAction = 4;
		}

		refillAction();
		if(compareRole(RoleKind.FieldOperative) || compareRole(RoleKind.Archivist) || compareRole(RoleKind.Epidemiologist) || compareRole(RoleKind.OperationsExpert)){
			oncePerTurnAction = true;
		}
		else{
			oncePerTurnAction = false;
		}
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
        foreach(CityCard card in handCard)
        {
            if (card.getCity() == c)
            {
                return true;
            }
        }
        return false;
    }

    public CityCard getCard(City c)
    {
        foreach(CityCard card in handCard)
        {
            if (card.getCity() == c)
            {
                return card;
            }
        }
        return null;
    }

    public string getUsername()
    {
        return username;
    }
	public void decreaseRemainingAction(){
		remainingAction--;
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
    }
    public Role getRole()
    {
        return curRole;
    }
}
