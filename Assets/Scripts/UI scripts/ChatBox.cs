using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    public InputField input;
    public GameObject textDisplay;
    public Game game;

    private string text = string.Empty;
    private RoleKind roleToDisplay;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        roleToDisplay = game.getCurrentPlayer().getRoleKind();
    }

    public void enterButtonClicked()
    {
        text = input.text;
        game.sendChatMessage(roleToDisplay, text);
    }

    public void displayText(RoleKind role, string text)
    {
        string previousText = textDisplay.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = previousText + role.ToString() + ": " + text + " \n";
    }


}
