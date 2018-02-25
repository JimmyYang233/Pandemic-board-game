using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingController : MonoBehaviour {

	public GameObject settingDetail;

	void Start(){
		settingDetail = gameObject.transform.GetChild (1).gameObject;
	}

	public void changeSettingDetail(){
		if (settingDetail.activeSelf == true) {
			settingDetail.SetActive (false);
		} 
		else {
			settingDetail.SetActive (true);
		}
	}
}
