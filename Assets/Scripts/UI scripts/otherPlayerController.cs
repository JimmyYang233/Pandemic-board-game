using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class otherPlayerController : MonoBehaviour {
    //for test use only, please don't delete it
    /*
    private void Start()
    {
        setRole(RoleKind.Archivist);
    }*/
    public void setRole(RoleKind k)
    {
        this.transform.GetChild(2).GetComponent<Text>().text = k.ToString();
    }
    //add city card to gui of other player
    
    public void addCityCard(CityName c)
    {

    }
    //delete city card from gui of other player
    public void deleteCityCard(CityName c)
    {
        
    }
    // add event card to gui of other player
    public void addEventCard(EventKind e)
    {

    }
    // delete event card to gui of 
    public void deleteEventCard(EventKind e)
    {

    }
}
