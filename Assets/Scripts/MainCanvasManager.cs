
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainCanvasManager : MonoBehaviour
{

	public static MainCanvasManager Instance;

	[SerializeField]
	private CurrentRoomCanvas _currentRoomCanvas;
	public CurrentRoomCanvas CurrentRoomCanvas
	{
		get { return _currentRoomCanvas; }
	}

	private void Awake()
	{
		Instance = this;
	}

}