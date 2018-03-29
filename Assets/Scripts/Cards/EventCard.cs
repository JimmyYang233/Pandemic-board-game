using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EventCard : PlayerCard {
    private EventKind eventKind;
    private static Dictionary<EventKind, EventCard> eventCards = new Dictionary<EventKind, EventCard>();
    

	private EventCard(EventKind kind): base(CardType.EventCard)
    {
        eventKind = kind;
    }

    public EventKind getEventKind()
    {
        return eventKind;
    }

    public static EventCard getEventCard(EventKind kind)
    {
        if (!eventCards.ContainsKey(kind))
        {
            eventCards.Add(kind, new EventCard(kind));
        }

        return eventCards[kind];
    }

}
