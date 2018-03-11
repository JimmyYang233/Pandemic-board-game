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
    public List<GameObject> outbreakrates;
    public List<GameObject> infectionrates;

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
        int infectionRate = game.getInfectionRate();
        for(int i = 0; i < 7; i++)
        {
            if (i + 1 == infectionRate)
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
