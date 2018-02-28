using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCard : PlayerCard {
    private EventKind eventKind;

    public EventCard(EventKind kind): base(CardType.EventCard)
    {
        eventKind = kind;
    }

    public EventKind getEventKind()
    {
        return eventKind;
    }
}
