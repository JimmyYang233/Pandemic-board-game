using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Pawn : MonoBehaviour {
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

    }
	public void setRole(RoleKind r){
		rolekind = r;
        if(r == RoleKind.BioTerrorist)
        {
            this.gameObject.GetComponent<Image>().color = Maps.getInstance().getRoleColor(r);
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Maps.getInstance().getRoleColor(r);
        }
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

    
}
