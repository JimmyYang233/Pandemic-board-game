using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCardUI : MonoBehaviour {
	bool start=false;
	Vector3 target;
	public float speed;
	public void Awake(){
		start = false;
		speed = 40;
	}
	public void Update(){
		if (start) {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target, step);
			if (target == transform.position) {
				start = false;
			}
		}
	}
	public void setDestination(Vector3 target){
		this.target = target;
		start = true;
	}
    public void mouseOn()
    {
        this.transform.position += new Vector3(0,40,0);
        this.transform.localScale = this.transform.localScale * 1.5f;
    }

    public void mouseLeave()
    {
        this.transform.position += new Vector3(0, -40, 0);
        this.transform.localScale = this.transform.localScale /1.5f;
    }
}
