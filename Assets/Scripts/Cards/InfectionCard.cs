using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionCard : PlayerCard 
{
	private City city;
    private Color color;

	public InfectionCard(City c) : base(CardType.InfectionCard)
    {
        
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
}
