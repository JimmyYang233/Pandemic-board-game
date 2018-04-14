using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContingencyPlannerSkillOperation : MonoBehaviour {

    public GameObject contingencyPlannerOnlyPanel;
	public GameObject eventCardSelect;
    public Game game;

    string eventCardToPut = null;

	public void findEventCardButtonClicked()
    {
		int i = 0;
        foreach(EventCard eventCard in game.getEventCardsFromDiscardPile())
        {
			Debug.Log (eventCard.getName ());
			eventCardSelect.transform.GetChild(0).GetChild (i).gameObject.SetActive (true);
			eventCardSelect.transform.GetChild(0).GetChild (i).GetChild (0).GetComponent<Text> ().text = eventCard.getName ();
			eventCardSelect.transform.GetChild(0).GetChild (i).name = eventCard.getName ();
			i++;
        }        
    }

    public void cardButtonClicked()
    {
		eventCardToPut = EventSystem.current.currentSelectedGameObject.name;
    }

    public void takeButtonClicked()
    {
        if (eventCardToPut != null)
        {
            game.ContingencyPlannerPutCardOnTopOfRoleCard(eventCardToPut);
            eventCardToPut = null;
            //hide card in the panel,prepare for next one
            foreach (Transform t in contingencyPlannerOnlyPanel.transform.GetChild(0))
            {
                t.gameObject.SetActive(false);
            }
        }
        else
        {
            //TO-DO, say something
        }
       
    }

}
