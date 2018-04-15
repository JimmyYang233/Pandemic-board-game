using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroubleShooterOperationController : MonoBehaviour {
	public GameObject troublePanel;
	// Use this for initialization
	public void useSkill(){
		troublePanel.SetActive (true);
	}
}
