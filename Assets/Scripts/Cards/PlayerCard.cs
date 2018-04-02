using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCard : Card
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

	public virtual string getName (){
		return "test failed";
	}
}
