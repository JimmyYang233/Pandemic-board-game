using UnityEngine;

public class DDOL : MonoBehaviour {
	public static DDOL instance = null;

	private void Awake (){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this);
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}
}
