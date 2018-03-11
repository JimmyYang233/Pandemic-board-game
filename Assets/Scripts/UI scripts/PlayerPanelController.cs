using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanelController : MonoBehaviour
{



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

        // apply it on current object's material
    }

    public void addPlayerCardToOtherPlayer(RoleKind k, Card c)
    {
        if (c is EventCard)
        {
            foreach (Transform t in this.transform)
            {
                if (t.gameObject.active && t.GetChild(2).GetComponent<Text>().text.Equals(k.ToString()))
                {
                    t.GetComponent<otherPlayerController>().addEventCard(((EventCard)c).getEventKind());
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
                    t.GetComponent<otherPlayerController>().addCityCard(((CityCard)c).getCity().getCityName());
                    break;
                }

            }
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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
