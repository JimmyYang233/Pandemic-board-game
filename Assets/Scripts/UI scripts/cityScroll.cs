using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cityScroll : MonoBehaviour {
    Vector3 startPos;
    // Use this for initialization
    void Start () {
        startPos = transform.position;
        //Debug.Log(gameObject.name + "" + transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -278.5)
        {
            //Debug.Log(transform.parent.transform.position.x);
            //Debug.Log(gameObject.name + "" + transform.position.x);
            transform.position = new Vector3(transform.position.x + 557, transform.position.y, transform.position.z);
        }
        else if (transform.parent.transform.position.x == 0)
        {
            Debug.Log(transform.parent.transform.position.x);
            transform.position = startPos;
        }
    }
}
