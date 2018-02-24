using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCard : Card
{
    private Enums.CardType cardType;

    public PlayerCard(Enums.CardType c)
    {
        cardType = c;
    }

    public Enums.CardType getType()
    {
        return cardType;
    }
}
