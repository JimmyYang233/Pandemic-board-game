using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PlayerScript requires the GameObject to have a Rigidbody component
[System.Serializable]
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
    private int quarantineMarker = 0; // Todo new field

    public int getNumPlayers()
    {
        return pawns.Count;
    }

    public void flipMarkerTo(int num)
    {
        quarantineMarker = num;
    }

    public int getMarker()
    {
        return quarantineMarker;
    }

    public void putMarker()
    {
        quarantineMarker = 2;
    }
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


	public void restoreCityInfo(CityInfo cityInfo){
		if (cityInfo.hasResearch) {
			hasResearch = true;
			displayResearch ();
		}
		using (var e1 = cityInfo.cubesColor.GetEnumerator ())
		using (var e2 = cityInfo.cubesNumber.GetEnumerator ()) {
			while (e1.MoveNext () && e2.MoveNext ()) {
				numberOfCubes[Game.stringToColor(e1.Current)] =  e2.Current;
				Debug.Log ("There are cubes in city " + cityInfo.cityName);
				Debug.Log (e1.Current);
				Debug.Log ("Number " + e2.Current);
			}
		}
		foreach(RoleKind roleKind in cityInfo.playerRoleKindInCity){
			this.addPawn (Game.Instance.FindPlayerPawnWithRoleKind(roleKind));
			Debug.Log (roleKind);
		}
		displayCube();
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
        if (quarantineMarker > 0)
        {
            quarantineMarker--;
            return;
        }
        Color pColor = disease.getColor();
        int current = numberOfCubes[pColor];
        numberOfCubes.Remove(pColor);
        numberOfCubes.Add(pColor, num+current);
        displayCube();
    }

    public void addCubes(int num)
    {
        if (quarantineMarker > 0)
        {
            quarantineMarker--;
            return;
        }
        int current = numberOfCubes[color];
        numberOfCubes.Remove(color);
        numberOfCubes.Add(color, num + current);
        displayCube();
    }

    public void removeCubes(Disease disease, int num)
    {
        Color pColor = disease.getColor();
        int current = numberOfCubes[pColor];
        numberOfCubes.Remove(pColor);
        numberOfCubes.Add(pColor, current - num);
        displayCube();
    }

    public bool hasCubesOfSpecificColor(Color aColor)
    {
        if (numberOfCubes[aColor] > 0)
        {
            return true;
        }
        return false;
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
        displayPawn ();
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
        List<string> cubes = new List<string>();
        foreach (Color color in numberOfCubes.Keys)
        {
            for (int i = 0; i < numberOfCubes[color]; i++)
            {
                if (color == Color.black)
                {
                    cubes.Add(blackCube);
                }
                else if (color == Color.red)
                {
                    cubes.Add(redCube);
                }
                else if (color == Color.blue)
                {
                    cubes.Add(blueCube);
                }
                else
                {
                    cubes.Add(yellowCube);
                }
            }
        } 
        if(cubes.Count == 1)
        {
            GameObject cube = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 7, transform.position.y + 6, transform.position.z), gameObject.transform.rotation);
            cube.transform.parent = gameObject.transform;
        }
        else if (cubes.Count == 2)
        {
            GameObject cube1 = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 7, transform.position.y + 6, transform.position.z), gameObject.transform.rotation);
            cube1.transform.parent = gameObject.transform;
            GameObject cube2 = (GameObject)Instantiate(Resources.Load(cubes[1]), new Vector3(transform.position.x + 7, transform.position.y + 6, transform.position.z), gameObject.transform.rotation);
            cube2.transform.parent = gameObject.transform;
        }
        else if (cubes.Count == 3)
        {
            GameObject cube1 = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 7, transform.position.y + 6, transform.position.z), gameObject.transform.rotation);
            cube1.transform.parent = gameObject.transform;
            GameObject cube2 = (GameObject)Instantiate(Resources.Load(cubes[1]), new Vector3(transform.position.x + 7, transform.position.y + 6, transform.position.z), gameObject.transform.rotation);
            cube2.transform.parent = gameObject.transform;
            GameObject cube3 = (GameObject)Instantiate(Resources.Load(cubes[2]), new Vector3(transform.position.x, transform.position.y - 6, transform.position.z), gameObject.transform.rotation);
            cube3.transform.parent = gameObject.transform;
        }
        else if(cubes.Count == 4)
        {
            GameObject cube1 = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 6, transform.position.y + 6, transform.position.z), gameObject.transform.rotation);
            cube1.transform.parent = gameObject.transform;
            GameObject cube2 = (GameObject)Instantiate(Resources.Load(cubes[1]), new Vector3(transform.position.x + 6, transform.position.y + 6, transform.position.z), gameObject.transform.rotation);
            cube2.transform.parent = gameObject.transform;
            GameObject cube3 = (GameObject)Instantiate(Resources.Load(cubes[2]), new Vector3(transform.position.x - 6, transform.position.y - 6, transform.position.z), gameObject.transform.rotation);
            cube3.transform.parent = gameObject.transform;
            GameObject cube4 = (GameObject)Instantiate(Resources.Load(cubes[3]), new Vector3(transform.position.x + 6, transform.position.y - 6, transform.position.z), gameObject.transform.rotation);
            cube4.transform.parent = gameObject.transform;
        }

        else if(cubes.Count == 5)
        {
            GameObject cube1 = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 5, transform.position.y + 7, transform.position.z), gameObject.transform.rotation);
            cube1.transform.parent = gameObject.transform;
            GameObject cube2 = (GameObject)Instantiate(Resources.Load(cubes[1]), new Vector3(transform.position.x + 5, transform.position.y + 7, transform.position.z), gameObject.transform.rotation);
            cube2.transform.parent = gameObject.transform;
            GameObject cube3 = (GameObject)Instantiate(Resources.Load(cubes[2]), new Vector3(transform.position.x - 8, transform.position.y - 3, transform.position.z), gameObject.transform.rotation);
            cube3.transform.parent = gameObject.transform;
            GameObject cube4 = (GameObject)Instantiate(Resources.Load(cubes[3]), new Vector3(transform.position.x + 8, transform.position.y - 3, transform.position.z), gameObject.transform.rotation);
            cube4.transform.parent = gameObject.transform;
            GameObject cube5 = (GameObject)Instantiate(Resources.Load(cubes[4]), new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), gameObject.transform.rotation);
            cube5.transform.parent = gameObject.transform;
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

	public Dictionary<Color, int> getNumOfCubes(){
		return numberOfCubes;
	}
}
