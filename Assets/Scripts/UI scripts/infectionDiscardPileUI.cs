using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infectionDiscardPileUI : MonoBehaviour {

	public Game game;
	public GameObject scroll;
	private Transform content;
	private int cardNum = 0;
	private Maps map;

	private void Awake()
	{
		content = this.transform.GetChild(0).GetChild(0);
		map = Maps.getInstance();
	}

	private void Update()
	{
		List < string > cards = game.getInfectionDiscardPileString();
		if (cards.Count > cardNum)
		{
			string card = cards[cardNum];
			addCityCard(card);
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
		scroll.SetActive(false);
	}
	public void addCityCard(string c)
	{
		content.GetChild(cardNum).gameObject.SetActive(true);


		//adjust text color 
		content.GetChild(cardNum ).GetChild(0).GetComponent<Text>().text = c;
		content.GetChild(cardNum ).GetComponent<Image>().color = map.getCityColor(game.findCity(c).cityName);
		if (content.GetChild(cardNum ).GetComponent<Image>().color == Color.black)
		{
			content.GetChild(cardNum ).GetChild(0).GetComponent<Text>().color = Color.white;
		}
		else
		{
			content.GetChild(cardNum ).GetChild(0).GetComponent<Text>().color = Color.black;
		}
		cardNum++;
		setBar();
	}


    //delete city card from gui of other player
	public void deleteCityCard(string c)
	{
		int i;
		for (i = 0; i < cardNum; i++)
		{
			if (c.Equals(content.GetChild(i).GetChild(0).GetComponent<Text>().text))
			{
				break;
			}
		}
		for (int j = i; j < cardNum; j++)
		{
			content.GetChild(j).GetChild(0).GetComponent<Text>().text = content.GetChild(j + 1).GetChild(0).GetComponent<Text>().text;
			content.GetChild(j).GetChild(0).GetComponent<Text>().color = content.GetChild(j + 1).GetChild(0).GetComponent<Text>().color;
			content.GetChild(j).GetComponent<Image>().color = content.GetChild(j + 1).GetComponent<Image>().color;
		}
		content.GetChild(cardNum - 1).GetChild(0).GetComponent<Text>().text = "";
		cardNum--;
		setBar();
		content.GetChild(cardNum).gameObject.SetActive(false);
	}
	public void buttonUninteractable(){
		foreach (Transform t in content) {
			if (t.gameObject.GetComponent<Button> () != null) {
				t.gameObject.GetComponent<Button> ().interactable=false;
			}
		}
	}
	// add event card to gui of other player


}
