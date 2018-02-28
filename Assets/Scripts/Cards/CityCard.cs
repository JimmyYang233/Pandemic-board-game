using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCard : PlayerCard {
    private City city;
    private Color color;
        
    public CityCard(City c): base(CardType.CityCard)
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
