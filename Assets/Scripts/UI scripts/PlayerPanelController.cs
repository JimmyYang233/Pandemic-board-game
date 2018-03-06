using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanelController : MonoBehaviour {



    public void addPlayer(Player p)
    {
        foreach(Transform t in this.transform)
        {
            if (!t.gameObject.active)
            {
                t.gameObject.SetActive(true);
                break;
            }
        }
        
        // apply it on current object's material
    }
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
