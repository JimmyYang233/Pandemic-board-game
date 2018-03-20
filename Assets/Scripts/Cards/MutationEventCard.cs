using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationEventCard : PlayerCard {

    private static MutationEventCard INSTANCE = new MutationEventCard();
    private static Dictionary<EventKind, EventCard> eventCards = new Dictionary<EventKind, EventCard>();



    private MutationEventCard(): base(CardType.MutationEventCard)
    {}

    public MutationEventCard getMutationEventCard()
    {
        return INSTANCE;
    }

    
}
