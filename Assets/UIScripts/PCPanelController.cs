using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPanelController : MonoBehaviour {
    public int CardNumber;
    public GameObject PlayerCardPrefab;
    public GameObject circleCenter;
    public float radius;
    public float totalDegree;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < CardNumber; i++)
        {
            addCard();
        }
        changePosition();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void addCard()
    {
        GameObject g=Instantiate(PlayerCardPrefab, new Vector3(0,0,0), Quaternion.identity);
        g.transform.parent = this.gameObject.transform;




    }
    public void changePosition()
    {
        float partDegree = this.totalDegree / (this.CardNumber-1);
        float initialDegree = -this.totalDegree / 2;
        for(int i = 0; i < CardNumber; i++)
        {
            float turnDegree = i * partDegree + initialDegree;
            turnDegree = (turnDegree / 360) * 2 * Mathf.PI; 
            float x = Mathf.Sin(turnDegree)*radius;
            float y = Mathf.Cos(turnDegree)*radius;
            Debug.Log(x + " " + y);
            this.transform.GetChild(i + 1).transform.position = circleCenter.transform.position + new Vector3(x, y, 0);
        }
    }
}
