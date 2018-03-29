using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eventCardController : MonoBehaviour {
	string name;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void useEvent(){
		name = this.transform.GetChild (1).GetComponent<Text>().text;
		Debug.Log (name);

	}
}
