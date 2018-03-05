using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {
    public float speed = 0.5f;
    Vector3 startPos;
	// Use this for initialization
	void Start () {
        startPos = transform.position;
       
    }
	
	// Update is called once per frame
	void Update () {

        transform.Translate(new Vector3(-1, 0, 0) * speed);
        if(transform.position.x < -557)
        {
            transform.position = startPos;
        }
        
	}

}
