using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCard : Card
{
    private CardType cardType;

    public PlayerCard(CardType c)
    {
        cardType = c;

    }

    public CardType getType()
    {
        return cardType;
    }
}
