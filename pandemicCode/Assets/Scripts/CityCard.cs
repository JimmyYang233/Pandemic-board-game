using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCard : MonoBehaviour {
    private Enums.CardType cardType= Enums.CardType.CityCard;
    private City city;
    private Enums.DiseaseColor color;
        
    public CityCard(City c)
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
