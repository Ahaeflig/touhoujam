using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonationBox : MonoBehaviour {


    public AudioClip sound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        int max = MusicPlayer.instance.stings.Length;
        int rnd = (int) Random.Range(0, max);
        MusicPlayer.instance.setSong(rnd);
    }

}
