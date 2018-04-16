using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioEscapeOperation : MonoBehaviour {

    public Game game;
    public GameObject playerCardPanel;
    Player currentPlayer;
	
	// Update is called once per frame
	void Update () {
        currentPlayer = game.getCurrentPlayer();
	}

    public void escapeButtonClicked()
    {
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            child.GetComponent<Button>().interactable = true;
            child.GetComponent<Button>().onClick.AddListener(() => escapeNow(name));
        }
    }

    public void escapeNow(string cardName)
    {
        game.BioTerroristEscape(cardName);
        int num = playerCardPanel.transform.GetChild(1).childCount;
        for (int i = 0; i < num; i++)
        {
            Debug.Log(i);
            GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
            string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
            child.GetComponent<Button>().interactable = false;
            child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
