using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MutationEventCard : PlayerCard {


    // private static Dictionary<EventKind, EventCard> eventCards = new Dictionary<EventKind, EventCard>();
    private MutationEvent MutationEvent;


    public MutationEventCard(MutationEvent me): base(CardType.MutationEventCard)
    {
        MutationEvent = me;
    }
    
    public MutationEvent getMutationEvent(){
        return MutationEvent;
    }

	public override string getName(){
		return MutationEvent.ToString();
	}
    
}
