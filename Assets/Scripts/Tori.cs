using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tori : MonoBehaviour {


    public string levelToLoadName;
    public AudioClip sfxWhenTeleporting;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {

            MusicPlayer.instance.playSong(sfxWhenTeleporting);
            SceneManager.LoadScene(levelToLoadName);
        }
    }

}
