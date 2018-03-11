using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSelectionPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addOtherPlayer(RoleKind k)
    {
        foreach (Transform t in this.transform)
        {
            if (!t.gameObject.active)
            {
                t.gameObject.SetActive(true);
                t.GetComponent<otherPlayerController>().setRole(k);
                break;
            }
        }

        // apply it on current object's material
    }
}
