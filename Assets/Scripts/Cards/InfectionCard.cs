using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InfectionCard : PlayerCard 
{
	[NonSerialized]
	private City city;
	[NonSerialized]
    private Color color;
	private CityName name;

	public InfectionCard(City c) : base(CardType.InfectionCard)
    {
        
        city = c;
        color = c.getColor();
		name = city.cityName;
    }

	public Color getColor()
    {
        return color;
    }

    public City getCity()
    {
        return city;
    }

	public string getName(){
		return name.ToString();
	}
}
