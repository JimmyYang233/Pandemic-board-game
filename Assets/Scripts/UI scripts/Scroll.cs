﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {
    public float speed = 0.5f;
    Vector3 startPos;
    Vector3 initialPosition;

    // Use this for initialization
	void Start () {
        startPos = transform.position;
       
    }
	
	// Update is called once per frame
	void Update () {

        if(transform.position.x < -557)
        {
            transform.position = startPos;
        }
        else if(transform.position.x > 557)
        {
            transform.position = startPos;
        }
        
	}

    public void beginDrag()
    {
        initialPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }


    public void move()
    {
        Vector3 mouseMovement = new Vector3((Camera.main.ScreenToViewportPoint(Input.mousePosition).x - initialPosition.x), 0, 0);
        transform.position = new Vector3(mouseMovement.x * 557 + transform.position.x, transform.position.y, transform.position.z);
        initialPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
}
