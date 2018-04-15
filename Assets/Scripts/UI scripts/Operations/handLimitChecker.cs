using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handLimitChecker : MonoBehaviour
{

    public Game game;
    Player me;
    public GameObject playerCardPanel;
    public GameObject waitForDiscardPanel;

    void Update()
    {
        me = game.FindPlayer(PhotonNetwork.player);
        if (me.getHandSize() >= me.getHandLimit())
        {
            waitForDiscardPanel.gameObject.SetActive(true);
            int num = playerCardPanel.transform.GetChild(1).childCount;
            for (int i = 0; i < num; i++)
            {
                Debug.Log(i);
                GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
                string name = playerCardPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text;
                child.GetComponent<Button>().onClick.RemoveAllListeners();
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().onClick.AddListener(() => discardCard(name));
            }
        }
        else
        {
            waitForDiscardPanel.gameObject.SetActive(false);
            int num = playerCardPanel.transform.GetChild(1).childCount;
            for (int i = 0; i < num; i++)
            {
                GameObject child = playerCardPanel.transform.GetChild(1).GetChild(i).gameObject;
                child.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void discardCard(string cardName)
    {
        game.Discard(cardName);
    }
}
