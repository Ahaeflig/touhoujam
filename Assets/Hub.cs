using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MusicPlayer.instance.setSong(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
