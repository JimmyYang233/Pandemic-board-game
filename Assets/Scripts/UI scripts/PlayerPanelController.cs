using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanelController : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public playerSelectionPanel playerSelect;
	/*
	public string findPlayerWithSpecificCity(string cName){
		foreach (Transform t in this.transform) {
			if (t.gameObject.active == true) {
				foreach (Transform tr in t.GetChild(3).GetChild(1)) {
					if (tr.GetChild (0).GetComponent<Text> ().text.Equals (cName)) {
						return 
					}
				}
				
			}
		}
	}
	*/
	public void swapRoleSelf(RoleKind roleKindNew){
		this.transform.GetChild (4).GetChild (0).GetComponent<Image> ().color = Maps.getInstance ().getRoleColor (roleKindNew);
		this.transform.GetChild (4).GetChild (1).GetComponent<Text> ().text = roleKindNew.ToString ();
	}
	public void swapRoleOther(RoleKind roleKindOld,RoleKind roleKindNew){
		foreach (Transform t in this.transform)
		{
			if (t.gameObject.activeSelf) {
				Debug.Log (t.transform.GetChild (2).GetComponent<Text> ().text);
				Debug.Log (roleKindOld);
				if (t.transform.GetChild (2).GetComponent<Text> ().text.Equals (roleKindOld.ToString())) {
					Debug.Log ("FIND");
					t.transform.GetChild (2).GetComponent<Text> ().text = roleKindNew.ToString ();
					t.transform.GetChild (1).GetComponent<Image> ().color = Maps.getInstance ().getRoleColor (roleKindNew);
					break;
				}
			}

		}		
	}
	public void addMainPlayer(RoleKind k){
		this.transform.GetChild (4).gameObject.SetActive (true);
		this.transform.GetChild (4).GetChild (0).GetComponent<Image> ().color = Maps.getInstance ().getRoleColor (k);
		this.transform.GetChild (4).GetChild (1).GetComponent<Text> ().text = k.ToString ();
	}
    public void addOtherPlayer(RoleKind k)
    {
        foreach (Transform t in this.transform)
        {
            if (!t.gameObject.active)
            {
                t.gameObject.SetActive(true);
                t.GetComponent<otherPlayerController>().setRole(k);
                break;
            }
        }

		playerSelect.addOtherPlayer (k);

        // apply it on current object's material
    }

    public void addPlayerCardToOtherPlayer(RoleKind k, Card c)
    {
		if (c.getType()==CardType.EventCard)
        {
            foreach (Transform t in this.transform)
            {
                if (t.gameObject.active && t.childCount>=3 && t.GetChild(2).GetComponent<Text>().text.Equals(k.ToString()))
                {
                    t.GetComponent<otherPlayerController>().addEventCard(((EventCard)c).getEventKind());
                    break;
                }
            }
        }
        else
        {
            //for test use
            bool find = false;

            foreach (Transform t in this.transform)
            {
                
                
                if (t.gameObject.activeSelf && t.childCount>=3 && t.GetChild(2).GetComponent<Text>().text.Equals(k.ToString()))
                {
                    t.GetComponent<otherPlayerController>().addCityCard(((CityCard)c).getCity().getCityName());
                    //Debug.Log("find"+k.ToString()+" "+ ((CityCard)c).getCity().getCityName().ToString());
                    find = true;
                    break;
                }

            }
            /*
            if (!find)
            {
                Debug.Log("notfind" + k.ToString() + " " + ((CityCard)c).getCity().getCityName().ToString());
            }*/
        }
    }

    public void deletePlayerCardFromOtherPlayer(RoleKind k,Card c)
    {
        if (c is EventCard)
        {
            foreach (Transform t in this.transform)
            {
                if (t.gameObject.active && t.GetChild(2).GetComponent<Text>().text.Equals(k.ToString()))
                {
                    t.GetComponent<otherPlayerController>().deleteEventCard(((EventCard)c).getEventKind());
                    break;
                }
            }
        }
        else
        {
            foreach (Transform t in this.transform)
            {

                if (t.gameObject.active && t.GetChild(2).GetComponent<Text>().text.Equals(k.ToString()))
                {
                    t.GetComponent<otherPlayerController>().deleteCityCard(((CityCard)c).getCity().getCityName());
                    break;
                }

            }
        }
    }
    
}
