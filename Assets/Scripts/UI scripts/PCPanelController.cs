using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCPanelController : MonoBehaviour {
    int cityCardNum;
    int eventCardNum;
    public GameObject PlayerCardPrefab;
	public GameObject eventCardPrefab;
    public GameObject playerCardStart;
    public Transform eventCardStart;

	public GameObject eventCardPanel;

    public float radius;
    public float totalDegree;
    public float maxSpace;

    Maps mapInstance;
    //only for test use, plase don't delete it
    /*
    private void Start()
    {
        addCityCard(CityName.Atlanta);
        addCityCard(CityName.Beijing);
        addCityCard(CityName.SanFrancisco);
        addCityCard(CityName.Riyadh);
    }
    */
    //add city card to gui
    private void Awake()
    {
        mapInstance = Maps.getInstance();
    }
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
    //for test use only
    public void addAtalantic()
    {
        addCityCard(CityName.Atlanta);
        addEventCard(EventKind.Airlift);
    }
    //for test use only 
    public void deleteAtlanta()
    {
        deleteCityCard(CityName.Atlanta);
        deleteEventCard(EventKind.Airlift);
    }

    public void addCityCard(CityName c)
    {
        cityCardNum++;
        GameObject g = Instantiate(PlayerCardPrefab, new Vector3(0,0,0), Quaternion.identity);
        Text t = g.transform.GetChild(0).gameObject.GetComponent<Text>();

        t.text = c.ToString();
        g.GetComponent<Image>().color = mapInstance.getCityColor(c);

        if (cityCardNum != 1)
        {

            foreach (Transform child in transform.GetChild(1))
            {
                child.position = child.position - new Vector3(maxSpace / 2, 0, 0);
            }
        }

        g.transform.parent = this.gameObject.transform;
		g.transform.position = playerCardStart.transform.position + new Vector3 (0, 100, 0);
		g.GetComponent<playerCardUI> ().setDestination (playerCardStart.transform.position);
        //g.transform.position = playerCardStart.transform.position;
        playerCardStart.transform.position += new Vector3(maxSpace/2, 0, 0);

        

        g.transform.parent = transform.GetChild(1);



    }
    //delete city card from gui
    public void deleteCityCard(CityName c)
    {
        bool find = false;
        foreach (Transform child in transform.GetChild(1))
        {
           
            if (!find && child.GetChild(0).GetComponent<Text>().text.Equals(c.ToString()))
            {
                find = true;
                Destroy(child.gameObject);

            }
            else if (!find)
            {
                child.position = child.position + new Vector3(maxSpace / 2, 0, 0);
            }
            else
            {
                child.position = child.position -new Vector3(maxSpace / 2, 0, 0);
            }
        }
        playerCardStart.transform.position -= new Vector3(maxSpace / 2, 0, 0);
        cityCardNum--;
    }

    public void addEventCard(EventKind e)
    {
        eventCardNum++;

        GameObject g = Instantiate(eventCardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		g.GetComponent<eventCardUI> ().setEventCardPanel (eventCardPanel);
        Text t = g.transform.GetChild(0).gameObject.GetComponent<Text>();

        t.text = e.ToString();
        g.GetComponent<Image>().color = Color.green;

        if (eventCardNum != 1)
        {

            foreach (Transform child in transform.GetChild(3))
            {
                child.position = child.position - new Vector3(maxSpace / 2, 0, 0);
            }
        }

        g.transform.parent = this.gameObject.transform;
        //g.transform.position = eventCardStart.transform.position;

		g.transform.position = eventCardStart.transform.position + new Vector3 (0, 100, 0);
		g.GetComponent<playerCardUI> ().setDestination (eventCardStart.transform.position);
        eventCardStart.transform.position += new Vector3(maxSpace / 2, 0, 0);



        g.transform.parent = transform.GetChild(3);

    }

    public void deleteEventCard(EventKind e)
    {
        bool find = false;
        foreach (Transform child in transform.GetChild(3))
        {

            if (!find && child.GetChild(0).GetComponent<Text>().text.Equals(e.ToString()))
            {
                find = true;
                Destroy(child.gameObject);

            }
            else if (!find)
            {
                child.position = child.position + new Vector3(maxSpace / 2, 0, 0);
            }
            else
            {
                child.position = child.position - new Vector3(maxSpace / 2, 0, 0);
            }
        }
        eventCardStart.transform.position -= new Vector3(maxSpace / 2, 0, 0);
        eventCardNum--;
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
            this.transform.GetChild(i + 1).transform.position = playerCardStart.transform.position + new Vector3(x, y, 0);
            this.transform.GetChild(i + 1).transform.rotation = Quaternion.Euler(0,0,-turnDegree);
        }
    }*/

}
