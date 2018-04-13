using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agreePanelController : MonoBehaviour {
	public GameObject agreePanel;
	public Game game;
	private enum Status {SHARE,REEXAMINEDRESEARCH,NEWASSIGNMENT};
	private Status status=Status.SHARE;

	//
	public ShareOperation share;

	public void clickYes(){
		if (status == Status.SHARE) {
			share.acceptRequest ();
		}
	}
	public void clickNo(){
		if (status == Status.SHARE) {
			share.declineRequest ();
		}

	}
}
