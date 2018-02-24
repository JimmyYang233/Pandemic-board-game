using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityInformation : MonoBehaviour {
    private Enums.CityName cityName;
    private Enums.DiseaseColor color;
    private List<Enums.CityName> neighbors;

    public CityInformation(Enums.CityName aCityName, Enums.DiseaseColor aColor, List<Enums.CityName> aNeighbors)
    {
        cityName = aCityName;
        color = aColor;
        neighbors = aNeighbors;
    }

    public Enums.DiseaseColor getColor()
    {
        return color;
    }

    public List<Enums.CityName> getNeighbors()
    {
        return neighbors;
    }

    public Enums.CityName getCityName()
    {
        return cityName;
    }
}
