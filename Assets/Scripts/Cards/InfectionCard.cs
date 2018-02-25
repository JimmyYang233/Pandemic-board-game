using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionCard : Card 
{
	private Enums.CardType cardType;
	private City city;
    private Enums.DiseaseColor color;

	public InfectionCard(City c)
    {
        cardType = Enums.CardType.InfectionCard;
        city = c;
        color = c.getColor();
    }

	public Enums.DiseaseColor getColor()
    {
        return color;
    }

    public City getCity()
    {
        return city;
    }

	public Enums.CardType getType()
    {
        return cardType;
    }
}
