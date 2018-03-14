using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

    private Color color;
    public RoleKind rolekind;
    private City currentCity;
    /*
    void Awake()
    {
		/*
        City[] cities = GetComponentsInParent<City>();
        foreach (City city in cities)
        {
            if (city.getCityName() == CityName.Atlanta)
            {
				Debug.Log (city.cityName);
                currentCity = city;
				Debug.Log ("find");
            }
        }
        Debug.Log(currentCity.getCityName().ToString());
        
		Transform g = this.transform.parent;
		//Debug.Log (g.name);
		foreach (Transform child in g) {
			City city = child.gameObject.GetComponent<City> ();
			if (city!=null && city.getCityName() == CityName.Atlanta)
			{

				currentCity = city;

			}
		}
		//Debug.Log(currentCity.getCityName().ToString());
    }
    */
    public Pawn(RoleKind aRolekind)
    {
        rolekind = aRolekind;
        color = Maps.getInstance().getRoleColor(aRolekind);
    }

    public void setCity(City c)
    {
        currentCity = c;
    }

    public City getCity()
    {
        return currentCity;
    }

    public RoleKind getRoleKind()
    {
        return rolekind;
    }

    public Color getColor()
    {
        return color;
    }
}
