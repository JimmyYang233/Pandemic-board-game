using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCard : PlayerCard {
    private City city;
    private Enums.DiseaseColor color;
        
    public CityCard(City c): base(Enums.CardType.CityCard)
    {
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

    
}
