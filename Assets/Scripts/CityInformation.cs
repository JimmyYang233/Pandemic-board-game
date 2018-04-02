using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class CityInformation {
    private CityName cityName;
	[NonSerialized]
    private Color color;
    private List<CityName> neighbors;
	private string colorString;

    public CityInformation(CityName aCityName, Color aColor, List<CityName> aNeighbors)
    {
        cityName = aCityName;
        color = aColor;
        neighbors = aNeighbors;
		colorString = color.ToString ();
    }

	public CityInformation(){
		
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
