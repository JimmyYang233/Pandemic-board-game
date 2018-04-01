using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MutationEventCard : PlayerCard {

    private static MutationEventCard INSTANCE = new MutationEventCard();
    private static Dictionary<EventKind, EventCard> eventCards = new Dictionary<EventKind, EventCard>();
	private string name = "MuatationEventCard";


    private MutationEventCard(): base(CardType.MutationEventCard)
    {}

    public MutationEventCard getMutationEventCard()
    {
        return INSTANCE;
    }

	public string getName(){
		return name;
	}
    
}
