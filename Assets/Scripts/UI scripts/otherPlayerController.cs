using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class otherPlayerController : MonoBehaviour
{
    //for test use only, please don't delete it
    int cityCardNum;
    int eventCardNum;
    Transform content;
    Maps map;
    private void Awake()
    {
        cityCardNum = 0;
        eventCardNum = 0;
        map = Maps.getInstance();
        content = this.transform.GetChild(3).GetChild(0).GetChild(0);
    }
    private void Start()
    {


        this.transform.GetChild(3).gameObject.GetComponent<ScrollRect>().vertical = false;
        this.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Scrollbar>().interactable = false;
        //below for test only, can comment
        /*
        setRole(RoleKind.Archivist);
        addCityCard(CityName.Algiers);
        addCityCard(CityName.Baghdad);
        addCityCard(CityName.Beijing);
        addEventCard(EventKind.Airlift);
        addEventCard(EventKind.Forecast);
        addEventCard(EventKind.GovernmentGrant);
        */
    }

    public void setRole(RoleKind k)
    {
        this.transform.GetChild(2).GetComponent<Text>().text = k.ToString();
        if (map.getRoleColor(k) != null)
        {

            this.transform.GetChild(1).GetComponent<Image>().color = map.getRoleColor(k);
        }
    }

    //add city card to gui of other player
    public void setBar()
    {
        if (cityCardNum + eventCardNum > 4)
        {
            this.transform.GetChild(3).gameObject.GetComponent<ScrollRect>().vertical = true;
            this.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Scrollbar>().interactable = true;
        }
        else
        {
            this.transform.GetChild(3).gameObject.GetComponent<ScrollRect>().vertical = false;
            this.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Scrollbar>().interactable = false;
        }
    }
    public void addCityCard(CityName c)
    {
        content.GetChild(cityCardNum + eventCardNum).gameObject.SetActive(true);
        cityCardNum++;

        if (eventCardNum != 0)
        {
            for (int i = eventCardNum; i > 0; i--)
            {
                content.GetChild(cityCardNum + i).GetChild(0).GetComponent<Text>().text = content.GetChild(cityCardNum + i - 1).GetChild(0).GetComponent<Text>().text;
                content.GetChild(cityCardNum + i).GetChild(0).GetComponent<Text>().color = content.GetChild(cityCardNum + i - 1).GetChild(0).GetComponent<Text>().color;
                content.GetChild(cityCardNum + i).GetComponent<Image>().color = content.GetChild(cityCardNum + i - 1).GetComponent<Image>().color;

            }
        }
        //adjust text color 
        content.GetChild(cityCardNum - 1).GetChild(0).GetComponent<Text>().text = c.ToString();
        content.GetChild(cityCardNum - 1).GetComponent<Image>().color = map.getCityColor(c);

        if (content.GetChild(cityCardNum - 1).GetComponent<Image>().color == Color.black)
        {
            content.GetChild(cityCardNum - 1).GetChild(0).GetComponent<Text>().color = Color.white;
        }
        else
        {
            content.GetChild(cityCardNum - 1).GetChild(0).GetComponent<Text>().color = Color.black;
        }

        setBar();
    }
    //delete city card from gui of other player
    public void deleteCityCard(CityName c)
    {
        int i;
        for (i = 0; i < cityCardNum + eventCardNum; i++)
        {
            if (c.ToString().Equals(content.GetChild(i).GetChild(0).GetComponent<Text>().text))
            {
                break;
            }
        }
        for (int j = i; j < cityCardNum + eventCardNum; j++)
        {
            content.GetChild(j).GetChild(0).GetComponent<Text>().text = content.GetChild(j + 1).GetChild(0).GetComponent<Text>().text;
            content.GetChild(j).GetChild(0).GetComponent<Text>().color = content.GetChild(j + 1).GetChild(0).GetComponent<Text>().color;
            content.GetChild(j).GetComponent<Image>().color = content.GetChild(j + 1).GetComponent<Image>().color;
        }
        content.GetChild(cityCardNum + eventCardNum - 1).GetChild(0).GetComponent<Text>().text = "";
        cityCardNum--;
        setBar();
        content.GetChild(cityCardNum + eventCardNum).gameObject.SetActive(false);
    }
    // add event card to gui of other player
    public void addEventCard(EventKind e)
    {
		Debug.Log (e.ToString);
        content.GetChild(cityCardNum + eventCardNum).gameObject.SetActive(true);
        content.GetChild(cityCardNum + eventCardNum).GetChild(0).GetComponent<Text>().text = e.ToString();

        content.GetChild(cityCardNum + eventCardNum).GetComponent<Image>().color = Color.green;
        content.GetChild(cityCardNum + eventCardNum).GetChild(0).GetComponent<Text>().color = Color.black;

        eventCardNum++;
        setBar();
    }
    // delete event card to gui of 
    public void deleteEventCard(EventKind e)
    {

        int i;
        for (i = 0; i < cityCardNum + eventCardNum; i++)
        {
            if (e.ToString().Equals(content.GetChild(i).GetChild(0).GetComponent<Text>().text))
            {
                break;
            }
        }
        for (int j = i; j < cityCardNum + eventCardNum; j++)
        {
            content.GetChild(j).GetChild(0).GetComponent<Text>().text = content.GetChild(j + 1).GetChild(0).GetComponent<Text>().text;
        }
        content.GetChild(cityCardNum + eventCardNum - 1).GetChild(0).GetComponent<Text>().text = "";
        eventCardNum--;
        setBar();
        content.GetChild(cityCardNum + eventCardNum).gameObject.SetActive(false);
    }

}