using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agreePanelController : MonoBehaviour {
	public GameObject agreePanel;
	public eventCardController eventController;
	public MoveOperation move;
	public Game game;
	public enum Status {SHARE,REEXAMINEDRESEARCH,NEWASSIGNMENT,DISPATCHER};
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
		} else if (status == Status.DISPATCHER) {
			move.acceptTheRequest ();
		}
		this.gameObject.SetActive (false);
	}
	public void clickNo(){
		if (status == Status.SHARE) {
			share.declineRequest ();
		} else if (status == Status.DISPATCHER) {
			move.declineTheRequest ();
		} else {
			eventController.rejectTheRequest ();
		}
		this.gameObject.SetActive (false);
	}
}
