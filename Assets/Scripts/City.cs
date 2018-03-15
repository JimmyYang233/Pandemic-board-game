﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PlayerScript requires the GameObject to have a Rigidbody component
public class City : MonoBehaviour {

	GameInfoDisplay gameInfo;
    public Color color;
    public CityName cityName;
    public List<City> neighbors = new List<City>();
    private bool hasResearch = false;
    private List<Disease> diseases = null; //Maybe unnecessary
	private List<Color> colors = new List<Color>(); 	//zhening: maybe unnecessary, but it'll make things easier, might be deleted in the future
    private List<Pawn> pawns = new List<Pawn>();
    private Dictionary<Color, int> numberOfCubes = new Dictionary<Color, int>();
    private string redCube;
    private string blackCube;
    private string blueCube;
    private string yellowCube;
	private void Awake()
	{
		colors = new List<Color>(); 	//zhening: maybe unnecessary, but it'll make things easier, might be deleted in the future
		pawns = new List<Pawn>();
		numberOfCubes = new Dictionary<Color, int>();
        numberOfCubes.Add(Color.black, 0);
        numberOfCubes.Add(Color.blue, 0);
        numberOfCubes.Add(Color.magenta, 0);
        numberOfCubes.Add(Color.yellow, 0);
        numberOfCubes.Add(Color.red, 0);
        redCube = "Cubes/redCube";
        blackCube = "Cubes/blackCube";
        blueCube = "Cubes/blueCube";
        yellowCube = "Cubes/yellowCube";
		gameInfo = GameObject.FindGameObjectWithTag ("gameInfoDisplay").GetComponent<GameInfoDisplay> ();
    }
    public City(CityName name)
    {
        cityName = name;
        color = Maps.getInstance().getCityColor(name);
        numberOfCubes.Add(Color.black, 0);
        numberOfCubes.Add(Color.blue, 0);
        numberOfCubes.Add(Color.magenta, 0);
        numberOfCubes.Add(Color.yellow, 0);
        numberOfCubes.Add(Color.red, 0);

        //UI only
    
    }

    public void setCityColor(Color color)
    {
        this.color = color;
    }

   
    public void addNeighbor(City city)
    {
        neighbors.Add(city);
    }

    public bool contains(RoleKind roleKind)
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

    public CityName getCityName()
    {
        return cityName;
    }

    public Color getColor()
    {
        return color;
    }

    public int getCubeNumber(Disease disease)
    {
        return numberOfCubes[disease.getColor()];
    }

	public int getCubeNumber(Color color)
	{
		return numberOfCubes [color];
	}
    public List<Color> getColors()
    {
        return null; //TO-DO
    }

    public List<City> getNeighbors()
    {
        return neighbors;
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

	//note: we have removed cubes when adding cubes, don't remvoe twice!
	//to be discussed 
    public void addCubes(Disease disease, int num)
    {
        Color pColor = disease.getColor();
        int current = numberOfCubes[pColor];
        numberOfCubes.Remove(pColor);
        numberOfCubes.Add(pColor, num+current);
		gameInfo.changeDiseaseNumber(pColor, -num);
        displayCube();
    }

    public void addCubes(int num)
    {
        int current = numberOfCubes[color];
        numberOfCubes.Remove(color);
        numberOfCubes.Add(color, num + current);
        gameInfo.changeDiseaseNumber(color, -num);
        displayCube();
    }

    public void removeCubes(Disease disease, int num)
    {
        Color pColor = disease.getColor();
        int current = numberOfCubes[pColor];
        numberOfCubes.Remove(pColor);
        numberOfCubes.Add(pColor, current - num);
		gameInfo.changeDiseaseNumber(pColor, num);
        displayCube();
    }

    public void removeNormalCubes(int num)
    {

    }

    public bool hasCubes()
    {
        foreach(int i in numberOfCubes.Values)
        {
            if (i > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void addPawn(Pawn p)
    {
		//Debug.Log (pawns.ToString ());
        this.pawns.Add(p);
        p.setCity(this);
		displayPawn ();
    }

    public void removePawn(Pawn p)
    {
        pawns.Remove(p);
		//Debug.Log (pawns.ToString());
        p.setCity(null);
    }

    public List<Pawn> getPawns()
    {
        List<Pawn> copy = new List<Pawn>(pawns);
        return copy;
    }

    public void decrementNumOfDiseaseCubeLeft(int ColorIndex)
    {
        //TO-DO
    }
    public void setHasResearch(bool b)
    {
        hasResearch = b;
        if(b == true)
        {
            displayResearch();
        }
        else
        {
            undisplayResearch();
        }
    }

    /// <summary>
    /// All below values and operations are only used in the client system. 
    /// </summary>
    public void displayButton()
    {
        Button theButton = gameObject.GetComponent<Button>();
        theButton.interactable = true;
    }
    
    public void undisplayButton()
    {
        Button theButton = gameObject.GetComponent<Button>();
        theButton.interactable = false;
    }

    public bool interactable()
    {
        Button theButton = gameObject.GetComponent<Button>();
        return theButton.interactable;
    }


    public void displayCube()
    {
        foreach (Transform child in transform)
        {
            if (child.tag != "researchStation")
            {
                GameObject.Destroy(child.gameObject);
            }
            
        }
        string currentCube;
        foreach(Color color in numberOfCubes.Keys)
        {
            if(color == Color.black)
            {
                currentCube = blackCube;
                //Debug.Log(currentCube);
            }
            else if(color == Color.red)
            {
                currentCube = redCube;
                //Debug.Log(currentCube);
            }
            else if(color == Color.blue)
            {
                currentCube = blueCube;
                //Debug.Log(currentCube);
            }
            else
            {
                currentCube = yellowCube;
                //Debug.Log(currentCube);
            }
            if(numberOfCubes[color] >= 1)
            {
                //Debug.Log(Resources.Load(currentCube));
                GameObject cube = (GameObject)Instantiate(Resources.Load(currentCube), new Vector3(transform.position.x - 8, transform.position.y + 7, transform.position.z), gameObject.transform.rotation);
                //cube.transform.localScale = new Vector3(1, 1, 1);
                cube.transform.parent = gameObject.transform;
            }
            if (numberOfCubes[color] >= 2)
            {
                //Debug.Log(Resources.Load(currentCube));
                GameObject cube = (GameObject)Instantiate(Resources.Load(currentCube), new Vector3(transform.position.x + 8, transform.position.y + 7, transform.position.z), gameObject.transform.rotation);
                //cube.transform.localScale = new Vector3(1, 1, 1);
                cube.transform.parent = gameObject.transform;
            }
            if (numberOfCubes[color] >= 3)
            {
                //Debug.Log(Resources.Load(currentCube));
                GameObject cube = (GameObject)Instantiate(Resources.Load(currentCube), new Vector3(transform.position.x, transform.position.y - 7, transform.position.z), gameObject.transform.rotation);
                //cube.transform.localScale = new Vector3(1, 1, 1);
                cube.transform.parent = gameObject.transform;
            }

        }
    }

    public void displayResearch()
    {
        if(hasResearch == true)
        {
            GameObject station = (GameObject)Instantiate(Resources.Load("researchStation"), new Vector3(0, 0, 0), gameObject.transform.rotation);
            station.transform.parent = gameObject.transform;
            Vector3 aPosition = transform.position;
            aPosition.y = aPosition.y - 5;
            station.transform.position = aPosition;
            //Debug.Log(station.transform.position);
        }
    }
	public void displayPawn(){
		int num = 0;
		foreach (Pawn pawn in pawns) {
			num++;
			if (num == 1) {
				Vector3 position = transform.position;
				position.y = position.y + 10;
				pawn.transform.position = position;
			} 
			else if (num == 2) {
				Vector3 position = transform.position;
				position.y = position.y + 10;
				position.x = position.x - 5;
				pawn.transform.position = position;
			}
			else if (num == 3) {
				Vector3 position = transform.position;
				position.y = position.y + 10;
				position.x = position.x + 5;
				pawn.transform.position = position;
			}
			else if (num == 4) {
				Vector3 position = transform.position;
				position.y = position.y + 10;
				position.x = position.x - 10;
				pawn.transform.position = position;
			}
			else if (num == 5) {
				Vector3 position = transform.position;
				position.y = position.y + 10;
				position.x = position.x + 10;
				pawn.transform.position = position;
			}
		}
	}

    public void undisplayResearch()
    {
        if(hasResearch == false)
        {
            foreach (Transform child in transform)
            {
                if (child.tag == "researchStation")
                {
                    GameObject.Destroy(child.gameObject);
                }

            }
        }
    }
}
