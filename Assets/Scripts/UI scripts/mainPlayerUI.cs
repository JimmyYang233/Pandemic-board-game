using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainPlayerUI : MonoBehaviour {
    public Game game;
    public Button card;

    Player me;

	// Use this for initialization
	void Start () {
		me = game.FindPlayer(PhotonNetwork.player);
    }
	
	// Update is called once per frame
	void Update () {
		if(me.getRoleKind() == RoleKind.ContingencyPlanner&&me.hasEventCardOnTopOfRoleCard())
        {
            EventCard theCard = me.getEventCardOnTopOfRoleCard();
            card.gameObject.SetActive(true);
            card.transform.GetChild(0).gameObject.GetComponent<Text>().text = theCard.getName().ToString();
        }
        else
        {
            card.gameObject.SetActive(false);
        }
        if (me.getRoleKind() == RoleKind.FieldOperative)
        {
            //TO-DO
        }
	}
}
