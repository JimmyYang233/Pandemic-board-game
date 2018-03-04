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
        Vector3 offset = new Vector2(Time.time * speed, 0);
        transform.Translate(new Vector3(-1, 0, 0) * speed * Time.time);
        if(transform.position.x == startPos.x)
        {
            Instantiate(gameObject, new Vector3(startPos.x+1920, startPos.y, startPos.z), gameObject.transform.rotation);
        }
        
	}
}
