using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoDisplay : MonoBehaviour
{
    public Game game;
    public GameObject outbreak;
    public GameObject infection;
	public GameObject cubeLeft;
	public GameObject researchLabLeft;
	public GameObject cardLeft;
    public List<GameObject> outbreakrates;
    public List<GameObject> infectionrates;

	public void changeDiseaseNumber(Color c, int num){
		Transform t = cubeLeft.transform.GetChild (0);
		if (c == Color.black) {
			t = cubeLeft.transform.GetChild (1);
		} else if (c == Color.blue) {
			t = cubeLeft.transform.GetChild (2);
		} else if(c==Color.red){
			t = cubeLeft.transform.GetChild (3);
		}
		t.GetChild (0).GetComponent<Text> ().text=(System.Int32.Parse (t.GetChild (0).GetComponent<Text> ().text) + num).ToString();


	}

	public void changeCardNumber(int num){
		cardLeft.transform.GetChild (0).GetComponent<Text> ().text=(System.Int32.Parse (cardLeft.transform.GetChild (0).GetComponent<Text> ().text) + num).ToString();

	}
    public void displayOutbreak()
    {
        int outbreakRate = game.getOutbreakRate();
        for(int i = 1; i < 9; i++)
        {
            if(i == outbreakRate)
            {
                outbreakrates[i-1].SetActive(true);
            }
            else{
                outbreakrates[i-1].SetActive(false);
            }
        }
    }

    public void displayInfectionRate()
    {
        int infectionIndex = game.getInfectionIndex();
        for(int i = 0; i < 7; i++)
        {
            if (i == infectionIndex)
            {
                infectionrates[i].SetActive(true);
            }
            else
            {
                infectionrates[i].SetActive(false);
            }
        }
    }


    
}
