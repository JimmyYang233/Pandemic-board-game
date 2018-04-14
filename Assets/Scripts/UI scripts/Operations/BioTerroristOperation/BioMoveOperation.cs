using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioMoveOperation : MonoBehaviour {

    public Button driveButton;
    public Button directFlightButton;
    public Button charterFlightButton;
    public Game game;


    public void moveButtonClicked()
    {
        Bioterrorist bioterrorist = game.getBioterrorist();
        driveButton.gameObject.setActive(true);
        if()
    }
}
