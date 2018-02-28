using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionCard : Card 
{
	private CardType cardType;
	private City city;
    private Color color;

	public InfectionCard(City c)
    {
        cardType = CardType.InfectionCard;
        city = c;
        color = c.getColor();
    }

	public Color getColor()
    {
        return color;
    }

    public City getCity()
    {
        return city;
    }

	public CardType getType()
    {
        return cardType;
    }
}
