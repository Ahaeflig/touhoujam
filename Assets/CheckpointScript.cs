using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Player"))
			CheckpointManagerScript.SetNewCheckpoint(transform.position);
	}
}
