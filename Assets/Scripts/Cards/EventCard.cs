using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCard : PlayerCard {
    private Enums.EventKind eventKind;

    public EventCard(Enums.EventKind kind): base(Enums.CardType.EventCard)
    {
        eventKind = kind;
    }

    public Enums.EventKind getEventKind()
    {
        return eventKind;
    }
}
