using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public static MusicPlayer instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private int level = 3;                                  //Current level number, expressed in game as "Day 1".

    public AudioClip[] stings;
    public AudioSource stingSource;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    void Start () {
        stingSource.clip = stings[0];
        stingSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setSong(int id)
    {
        stingSource.clip = stings[id];
        stingSource.Play();
    }

    public void changeIntensityVolume(float volume)
    {
        stingSource.volume = volume;
    }

}
