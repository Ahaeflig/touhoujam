using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsEffect : MonoBehaviour {

	public float Timeout;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Timeout -= Time.deltaTime;
		if (Timeout <= 0f)
			Destroy(gameObject);
	}
}
