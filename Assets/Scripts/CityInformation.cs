using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityInformation {
    private CityName cityName;
    private Color color;
    private List<CityName> neighbors;

    public CityInformation(CityName aCityName, Color aColor, List<CityName> aNeighbors)
    {
        cityName = aCityName;
        color = aColor;
        neighbors = aNeighbors;
    }

    public Color getColor()
    {
        return color;
    }

    public List<CityName> getNeighbors()
    {
        return neighbors;
    }

    public CityName getCityName()
    {
        return cityName;
    }
}
