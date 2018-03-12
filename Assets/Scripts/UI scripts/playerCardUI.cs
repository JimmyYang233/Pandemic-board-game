using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCardUI : MonoBehaviour {

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
