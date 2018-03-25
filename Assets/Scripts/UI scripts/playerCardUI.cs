using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCardUI : MonoBehaviour {
	bool start=false;
	Vector3 target;
	public float speed;
	private Vector3 addSize;
	private Vector3 addPosition;
	private Vector3 originalPosition;
	private Vector3 originalSize;
	bool isEventCard=false;
	bool isSelected=false;
	public void Awake(){
		isEventCard = false;
		start = false;
		speed = 40;
		addSize = new Vector3(1.5f,1.5f,1.5f);
		originalSize = new Vector3(1,1,1);
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
		this.transform.localScale = addSize;
    }
	public void mouseClick(){
		if (!isSelected) {
			isSelected = true;
		} else {
			isSelected = false;
			mouseLeave();
		}
	}
    public void mouseLeave()
    {
		if (!isSelected) {
			this.transform.position += new Vector3 (0, -40, 0);
			this.transform.localScale = originalSize;
		}
    }
}
