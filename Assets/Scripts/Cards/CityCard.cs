using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CityCard : PlayerCard {
	[NonSerialized]
    private City city;
	[NonSerialized]
    private Color color;
	private CityName name;
        
    public CityCard(City c): base(CardType.CityCard)
    {
        city = c;
        color = c.getColor();
		name = c.getCityName ();
    }

    public Color getColor()
    {
        return color;
    }

    public City getCity()
    {
        return city;
    }

	public CityName getName(){
		return name;
	}
    
}
