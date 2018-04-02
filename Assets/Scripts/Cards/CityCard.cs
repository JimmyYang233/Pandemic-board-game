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
	private string name;
        
    public CityCard(City c): base(CardType.CityCard)
    {
        city = c;
        color = c.getColor();
		name = c.getCityName ().ToString();
    }

    public Color getColor()
    {
        return color;
    }

    public City getCity()
    {
        return city;
    }

	public override string getName(){
		return name;
	}
    
}
