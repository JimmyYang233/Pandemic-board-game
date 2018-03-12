using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork Instance;

	private void Awake(){
		Instance = this;
	}	
}
