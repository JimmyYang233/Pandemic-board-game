using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseController : MonoBehaviour {

    public Game game;
    public GameObject mainCanavas;
    public GameObject winPanel;
    public GameObject losePanel;
	

    public void notifyWin()
    {
        mainCanavas.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
    }

    public void notifyLose()
    {
        mainCanavas.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
    }
}
