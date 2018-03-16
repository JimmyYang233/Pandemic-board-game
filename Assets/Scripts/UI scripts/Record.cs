using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour {
    public GameObject textDisplay;
    public GameObject scrollBar;

    public void displayRecord(RoleKind role, string text)
    {
            string previousText = textDisplay.GetComponent<Text>().text;
            textDisplay.GetComponent<Text>().text = previousText + role.ToString() + ": " + text + " \n";
            scrollBar.GetComponent<Scrollbar>().value = 0;
    }
}
