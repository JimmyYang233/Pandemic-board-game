using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PlayerScript requires the GameObject to have a Rigidbody component
public class City : MonoBehaviour {

    public Enums.DiseaseColor color;
    public Enums.CityName cityName;
    public List<City> neighbors = new List<City>();
    private bool hasResearch = false;
    private List<Disease> diseases = null; //Maybe unnecessary
    private List<Pawn> pawns = new List<Pawn>();
    private Dictionary<Enums.DiseaseColor, int> numberOfCubes = new Dictionary<Enums.DiseaseColor, int>();

    public Sprite test;

    public City(Enums.CityName name)
    {
        cityName = name;
        color = Maps.getInstance().getCityColor(name);
        numberOfCubes.Add(Enums.DiseaseColor.Black, 0);
        numberOfCubes.Add(Enums.DiseaseColor.Blue, 0);
        numberOfCubes.Add(Enums.DiseaseColor.Purple, 0);
        numberOfCubes.Add(Enums.DiseaseColor.Yellow, 0);
        numberOfCubes.Add(Enums.DiseaseColor.Red, 0);

        //UI only
    
    }

    private void Start()
    {
        gameObject.AddComponent(typeof(Button));
        Image i=gameObject.AddComponent(typeof(Image)) as Image;
        Color c = this.GetComponent<SpriteRenderer>().color;
        i.sprite = this.GetComponent<SpriteRenderer>().sprite;
        i.color = c;

        SpriteRenderer r = this.GetComponent<SpriteRenderer>();
        Destroy(r);


    }
    public void addNeighbor(City city)
    {
        neighbors.Add(city);
    }

    public bool contains(Enums.RoleKind roleKind)
    {
        foreach(Pawn pawn in pawns)
        {
            if(pawn.getRoleKind() == roleKind)
            {
                return true;
            }
        }
        return false;
    }

    public bool getHasResearch()
    {
        return hasResearch;
    }

    public Enums.CityName getCityName()
    {
        return cityName;
    }

    public Enums.DiseaseColor getColor()
    {
        return color;
    }

    public int getCubeNumber(Disease disease)
    {
        return numberOfCubes[disease.getColor()];
    }

    public List<Color> getColors()
    {
        return null; //TO-DO
    }

    public List<City> getNeighbors()
    {
        List<City> copy = new List<City>(neighbors);
        return copy;
    }

    public void addDiseases(List<Disease> aDiseases)
    {
        diseases = aDiseases;
    }

    public List<Disease> getDiseases()
    {
        List<Disease> copy = new List<Disease>(diseases);
        return copy;
    }

    public void addCubes(Disease disease, int num)
    {
        Enums.DiseaseColor pColor = disease.getColor();
        int current = numberOfCubes[pColor];
        numberOfCubes.Add(pColor, num+current);
        disease.removeCubes(num);
    }

    public void addPawn(Pawn p)
    {
        pawns.Add(p);
        p.setCity(this);
    }

    public void decrementNumOfDiseaseCubeLeft(int ColorIndex)
    {
        //TO-DO
    }

    public void removeCubes(Disease disease, int num)
    {
        Enums.DiseaseColor pColor = disease.getColor();
        int current = numberOfCubes[pColor];
        numberOfCubes.Add(pColor, current - num);
        disease.addCubes(num);
    }

    public void removePawn(Pawn p)
    {
        pawns.Remove(p);
        p.setCity(null);
    }

    public void setHasResearch(bool b)
    {
        hasResearch = b;
    }
}
