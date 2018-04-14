using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainPlayerUI : MonoBehaviour {
    public Game game;
    public Button card;
    public GameObject cubeHolder;
    public eventCardUI eventCard;
    private string redCube;
    private string blackCube;
    private string blueCube;
    private string yellowCube;
    Player me;


    // Use this for initialization
    void Start () {
		me = game.FindPlayer(PhotonNetwork.player);
        redCube = "Cubes/redCube";
        blackCube = "Cubes/blackCube";
        blueCube = "Cubes/blueCube";
        yellowCube = "Cubes/yellowCube";
    }
	
	// Update is called once per frame
	void Update () {
		if(me.getRoleKind() == RoleKind.ContingencyPlanner&&me.hasEventCardOnTopOfRoleCard())
        {
            EventCard theCard = me.getEventCardOnTopOfRoleCard();
            string cardName = theCard.getName().ToString();
            card.gameObject.SetActive(true);
            card.gameObject.GetComponent<Button>().onClick.AddListener(() => eventCard.click());
            card.transform.GetChild(0).gameObject.GetComponent<Text>().text = cardName;
        }
        else
        {
            card.gameObject.SetActive(false);
            card.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
    public void displayCubes()
    {
        if (me.getRoleKind() == RoleKind.FieldOperative)
        {
            Dictionary<Color, int> numberOfCubes = new Dictionary<Color, int>(); //TO-DO a way to get them
            foreach (Transform child in cubeHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
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
            if (cubes.Count == 1)
            {
                GameObject cube = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 7, transform.position.y + 6, transform.position.z), cubeHolder.transform.rotation);
                cube.transform.parent = cubeHolder.transform;
            }
            else if (cubes.Count == 2)
            {
                GameObject cube1 = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 7, transform.position.y + 6, transform.position.z), cubeHolder.transform.rotation);
                cube1.transform.parent = cubeHolder.transform;
                GameObject cube2 = (GameObject)Instantiate(Resources.Load(cubes[1]), new Vector3(transform.position.x + 7, transform.position.y + 6, transform.position.z), cubeHolder.transform.rotation);
                cube2.transform.parent = cubeHolder.transform;
            }
            else if (cubes.Count == 3)
            {
                GameObject cube1 = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 7, transform.position.y + 6, transform.position.z), cubeHolder.transform.rotation);
                cube1.transform.parent = cubeHolder.transform;
                GameObject cube2 = (GameObject)Instantiate(Resources.Load(cubes[1]), new Vector3(transform.position.x + 7, transform.position.y + 6, transform.position.z), cubeHolder.transform.rotation);
                cube2.transform.parent = cubeHolder.transform;
                GameObject cube3 = (GameObject)Instantiate(Resources.Load(cubes[2]), new Vector3(transform.position.x, transform.position.y - 6, transform.position.z), cubeHolder.transform.rotation);
                cube3.transform.parent = cubeHolder.transform;
            }
            else if (cubes.Count == 4)
            {
                GameObject cube1 = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 6, transform.position.y + 6, transform.position.z), cubeHolder.transform.rotation);
                cube1.transform.parent = cubeHolder.transform;
                GameObject cube2 = (GameObject)Instantiate(Resources.Load(cubes[1]), new Vector3(transform.position.x + 6, transform.position.y + 6, transform.position.z), cubeHolder.transform.rotation);
                cube2.transform.parent = cubeHolder.transform;
                GameObject cube3 = (GameObject)Instantiate(Resources.Load(cubes[2]), new Vector3(transform.position.x - 6, transform.position.y - 6, transform.position.z), cubeHolder.transform.rotation);
                cube3.transform.parent = cubeHolder.transform;
                GameObject cube4 = (GameObject)Instantiate(Resources.Load(cubes[3]), new Vector3(transform.position.x + 6, transform.position.y - 6, transform.position.z), cubeHolder.transform.rotation);
                cube4.transform.parent = cubeHolder.transform;
            }

            else if (cubes.Count == 5)
            {
                GameObject cube1 = (GameObject)Instantiate(Resources.Load(cubes[0]), new Vector3(transform.position.x - 5, transform.position.y + 7, transform.position.z), cubeHolder.transform.rotation);
                cube1.transform.parent = cubeHolder.transform;
                GameObject cube2 = (GameObject)Instantiate(Resources.Load(cubes[1]), new Vector3(transform.position.x + 5, transform.position.y + 7, transform.position.z), cubeHolder.transform.rotation);
                cube2.transform.parent = cubeHolder.transform;
                GameObject cube3 = (GameObject)Instantiate(Resources.Load(cubes[2]), new Vector3(transform.position.x - 8, transform.position.y - 3, transform.position.z), cubeHolder.transform.rotation);
                cube3.transform.parent = cubeHolder.transform;
                GameObject cube4 = (GameObject)Instantiate(Resources.Load(cubes[3]), new Vector3(transform.position.x + 8, transform.position.y - 3, transform.position.z), cubeHolder.transform.rotation);
                cube4.transform.parent = cubeHolder.transform;
                GameObject cube5 = (GameObject)Instantiate(Resources.Load(cubes[4]), new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), cubeHolder.transform.rotation);
                cube5.transform.parent = cubeHolder.transform;
            }
        }
    }
    
}
