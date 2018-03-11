using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCPanelController : MonoBehaviour {
    public int CardNumber;
    public GameObject PlayerCardPrefab;
    public GameObject circleCenter;
    public float radius;
    public float totalDegree;
    //only for test use, plase don't delete it
    /*
    private void Start()
    {
        addCityCard(CityName.Atlanta);
    }
    */
    //add city card to gui

    public void addPlayerCard(Card c)
    {
        if(c is EventCard)
        {
            addEventCard(((EventCard)c).getEventKind());
        }
        else
        {
            addCityCard(((CityCard)c).getCity().getCityName());
        }
    }

    public void deletePlayerCard(Card c)
    {
        if (c is EventCard)
        {
            deleteEventCard(((EventCard)c).getEventKind());
        }
        else
        {
            deleteCityCard(((CityCard)c).getCity().getCityName());
        }
    }
    public void addCityCard(CityName c)
    {
        GameObject g = Instantiate(PlayerCardPrefab, new Vector3(0,0,0), Quaternion.identity);
        g.transform.parent = this.gameObject.transform;
        Text t = g.transform.GetChild(0).gameObject.GetComponent<Text>();
        t.text = c.ToString();
        g.transform.position = circleCenter.transform.position;
        circleCenter.transform.position += new Vector3(40, 0, 0);


        //set color of the card
        Maps mapInstance = Maps.getInstance();
      
        Color newColor = mapInstance.getCityColor(c);
        // apply it on current object's material
        g.GetComponent<Image>().color = newColor;
    }
    //delete city card from gui
    public void deleteCityCard(CityName c)
    {
        foreach (Transform child in transform)
        {
           
            if (child.childCount!=0 && child.GetChild(0).GetComponent<Text>().text.Equals(c.ToString()))
            {

                Destroy(child.gameObject);
                break;

            }
        }
    }

    public void addEventCard(EventKind e)
    {

    }

    public void deleteEventCard(EventKind e)
    {

    }
    // Use for shape, no need for demo
    /*void Start () {
	    for(int i = 0; i < CardNumber; i++)
        {
            addCard();
        }
        changePosition();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void addCard()
    {
        GameObject g=Instantiate(PlayerCardPrefab, new Vector3(0,0,0), Quaternion.identity);
        g.transform.parent = this.gameObject.transform;




    }
    public void changePosition()
    {
        float partDegree = this.totalDegree / (this.CardNumber-1);
        float initialDegree = -this.totalDegree / 2;
        for(int i = 0; i < CardNumber; i++)
        {
            float turnDegree = i * partDegree + initialDegree;
            float turnDegreef = (turnDegree / 360) * 2 * Mathf.PI; 
            float x = Mathf.Sin(turnDegreef)*radius;
            float y = Mathf.Cos(turnDegreef)*radius;
            this.transform.GetChild(i + 1).transform.position = circleCenter.transform.position + new Vector3(x, y, 0);
            this.transform.GetChild(i + 1).transform.rotation = Quaternion.Euler(0,0,-turnDegree);
        }
    }*/

}
