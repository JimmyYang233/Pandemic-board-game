using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroubleShooterOperationController : MonoBehaviour {
	public GameObject troublePanel;
	public Game game;

	// Use this for initialization
	public void useSkill(){
		troublePanel.SetActive (true);
		List<string> infections = game.getInfectionDeckString ();
		int rate = game.getInfectionRate ();
		for (int i = 0; i < rate; i++) {
			GameObject target = troublePanel.transform.GetChild (i).gameObject;
			target.SetActive (true);
			target.GetComponent<Image> ().color=game.findCity(infections[i]).getColor();
			target.transform.GetChild (0).GetComponent<Text> ().text = infections [i];
		}
		for (int i = rate; i < 4; i++) {
			troublePanel.transform.GetChild (0).gameObject.SetActive (false);
		}
	}
}
