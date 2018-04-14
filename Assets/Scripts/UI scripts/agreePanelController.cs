﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agreePanelController : MonoBehaviour {
	public GameObject agreePanel;
	public eventCardController eventController;
	public Game game;
	public enum Status {SHARE,REEXAMINEDRESEARCH,NEWASSIGNMENT};
	public Status status=Status.SHARE;

	//
	public ShareOperation share;

	public void clickYes(){
		if (status == Status.SHARE) {
			share.acceptRequest ();
		} else if (status == Status.NEWASSIGNMENT) {
			eventController.acceptTheRequest ();
		} else if (status == Status.REEXAMINEDRESEARCH) {
			eventController.acceptTheRequest ();
		}
	}
	public void clickNo(){
		if (status == Status.SHARE) {
			share.declineRequest ();
		} else {
			eventController.rejectTheRequest ();
		}
	}
}
