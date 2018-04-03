using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContingencyPlannerSkillOperation : MonoBehaviour {

    public GameObject contingencyPlannerOnlyPanel;
    public Game game;

    string eventCardToPut = null;

	public void findEventCardButtonClicked()
    {
        foreach(EventCard eventCard in game.getEventCardsFromDiscardPile())
        {
            //TO-DO What I want to do is to display all the eventCard that I get, but how?
        }        
    }

    public void cardButtonClicked(string eventCardName)
    {
        eventCardToPut = eventCardName;
    }

    public void takeButtonClicked()
    {
        game.ContingencyPlannerPutCardOnTopOfRoleCard("ContingencyPlanner", eventCardToPut);
        eventCardToPut = null;
    }
}
