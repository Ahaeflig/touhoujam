using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManagerScript : MonoBehaviour {

	static public Vector3 LastPosition;
	public Transform startingPos;
	// Use this for initialization
	void Start () {
		LastPosition = startingPos.position;
	}
	
	public static void SetNewCheckpoint(Vector3 pos)
	{
		LastPosition = pos;
	}

	public static Vector3 GetLastCheckpoint()
	{
		return LastPosition;
	}
}
