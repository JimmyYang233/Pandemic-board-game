using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private int maximumAction;
	private int remainingAction;
	private boolean oncePerTurnAction;
	private Role curRole;
	private List<PlayerCard> handCard = new List<PlayerCard>();

	public Player(User: user)
	{
		curRole = user.getRole();
		if(compareRole(Enums.RoleKind.Generalist)){
			maximumAction = 5;
		}
		else{
			maximumAction = 4;
		}

		refillAction();
		if(compareRole(Enums.RoleKind.FieldOperative) || compareRole(Enums.RoleKind.Archivist) || compareRole(Enums.RoleKind.Epidemiologist) || compareRole(Enums.RoleKind.OperationsExpert)){
			oncePerTurnAction = true;
		}
		else{
			oncePerTurnAction = false;
		}
	}

	// public boolean consentRequest(){
		
	// }

	private boolean compareRole(Enums.RoleKind role){
		return curRole == role;
	}

	public int getHandLimit(){
		if(curRole == Enums.RoleKind.Archivist){
			return 8;
		}
		else{
			return 7;
		}
	}
	
	public int getHandCardNumber(){
		return handCard.size();
	}

	public Pawn getPlayerPawn(){
		return curRole.getPawn();
	}

	public void addCard(PlayerCard card){
		handCard.add(card);
	}

	public void decreaseRemainingAction(){
		remainingAction--;
	}

	public void refillAction(){
		remainingAction = maximumAction;
	}

	public void removeCard(PlayerCard card){
		handCard.remove(card);
	}

	public void removeOneAction(){
		oncePerTurnAction = false;
	}

	public void setOncePerturnAction(boolean toggle){
		oncePerTurnAction = toggle;
	}

}
