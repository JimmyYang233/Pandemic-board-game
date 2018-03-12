using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class playerSelectionPanel : MonoBehaviour {
	Maps map;
	//TODO- other operation
	private enum Status {SHARE,OTHER};
	public ShareOperation share;
	private Status selectStatus = Status.SHARE; 
	void Awake(){
		map = Maps.getInstance ();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void setShareStatus(){
		selectStatus = Status.SHARE;
	}
	//todo other status reimplement
	public void setOtherStatus(){
		selectStatus = Status.OTHER;
	}

    public void addOtherPlayer(RoleKind k)
    {
        foreach (Transform t in this.transform)
        {
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(true);
				t.GetChild(0).GetComponent<Text>().text = k.ToString();
				t.name = k.ToString ();
				if (map.getRoleColor(k) != null)
				{

					t.GetComponent<Image>().color = map.getRoleColor(k);
				}

                break;
            }
        }

        // apply it on current object's material
    }
	public void characterSelect(){
		if (selectStatus == Status.SHARE) {
			share.selectRole(EventSystem.current.currentSelectedGameObject.name);
		}
	}
	public void cancelButtonClick(){
		if (selectStatus == Status.SHARE) {
			share.cancel ();
		}
	}
}
