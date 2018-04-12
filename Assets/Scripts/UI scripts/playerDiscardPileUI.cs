using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerDiscardPileUI : MonoBehaviour {


    public Game game;
    public GameObject scroll;
    private Transform content;
    private int cardNum = 0;
    private Maps map;
	public bool eventCardTime;

    private void Awake()
    {
		eventCardTime = false;
        content = this.transform.GetChild(0).GetChild(0);
        map = Maps.getInstance();
    }

    private void Update()
    {
        List < PlayerCard > cards = game.getPlayerDiscardPile();
        if (cards.Count > cardNum)
        {
            PlayerCard card = cards[cardNum];
            if(card.getType() == CardType.CityCard)
            {
                addCityCard(((CityCard)card).getCity().getCityName());
            }
            else if(card.getType() == CardType.EventCard)
            {
                addEventCard(((EventCard)card).getEventKind());
            }
        }
        
    }

    public void setBar()
    {
        if (cardNum > 9)
        {
            gameObject.GetComponent<ScrollRect>().vertical = true;
            transform.GetChild(1).gameObject.GetComponent<Scrollbar>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<ScrollRect>().vertical = false;
            transform.GetChild(1).gameObject.GetComponent<Scrollbar>().interactable = false;
        }
    }

    public void mouseOn()
    {
        scroll.SetActive(true);
    }

    public void mouseLeave()
    {
		if (!eventCardTime) {
			scroll.SetActive (false);
		}
    }
    public void addCityCard(CityName c)
    {
        content.GetChild(cardNum).gameObject.SetActive(true);
        cardNum++;

        //adjust text color 
        content.GetChild(cardNum - 1).GetChild(0).GetComponent<Text>().text = c.ToString();
        content.GetChild(cardNum - 1).GetComponent<Image>().color = map.getCityColor(c);
        if (content.GetChild(cardNum - 1).GetComponent<Image>().color == Color.black)
        {
            content.GetChild(cardNum - 1).GetChild(0).GetComponent<Text>().color = Color.white;
        }
        else
        {
            content.GetChild(cardNum - 1).GetChild(0).GetComponent<Text>().color = Color.black;
        }

        setBar();
    }
		
   
	//delete city card from gui of other player
	public void deleteCityCard(string c)
	{
		Debug.Log ("delete c");
		int i;
		for (i = 0; i < cardNum; i++)
		{
			if (c.ToString().Equals(content.GetChild(i).GetChild(0).GetComponent<Text>().text))
			{
				break;
			}
		}
		for (int j = i; j < cardNum; j++)
		{
			content.GetChild(j).name = content.GetChild(j + 1).name;
			content.GetChild(j).GetChild(0).GetComponent<Text>().text = content.GetChild(j + 1).GetChild(0).GetComponent<Text>().text;
			content.GetChild(j).GetChild(0).GetComponent<Text>().color = content.GetChild(j + 1).GetChild(0).GetComponent<Text>().color;
			content.GetChild(j).GetComponent<Image>().color = content.GetChild(j + 1).GetComponent<Image>().color;
		}
		content.GetChild(cardNum - 1).GetChild(0).GetComponent<Text>().text = "";
		content.GetChild(cardNum - 1).name = "";
		cardNum--;
		setBar();
		content.GetChild(cardNum).gameObject.SetActive(false);
		buttonUninteractable ();
	}
	public void buttonUninteractable(){
		foreach (Transform t in content) {
			/*if (t.gameObject.GetComponent<Button> () != null) {
				t.gameObject.GetComponent<Button> ().interactable=false;
			}*/
			Destroy (t.gameObject.GetComponent<Button> ());
			this.gameObject.SetActive (false);
		}
	}
    public void addEventCard(EventKind e)
    {
        content.GetChild(cardNum).gameObject.SetActive(true);
        content.GetChild(cardNum).GetChild(0).GetComponent<Text>().text = e.ToString();
        content.GetChild(cardNum).GetComponent<Image>().color = Color.green;
        content.GetChild(cardNum).GetChild(0).GetComponent<Text>().color = Color.black;
        cardNum++;
        setBar();
    }

}
