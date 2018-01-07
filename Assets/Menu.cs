using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject startButton;
    public GameObject quitButton;
    public GameObject optionsButton;

    public GameObject sound;
    public GameObject optionReturn;

    private string volumeKey = "intensityVolume";

    // Use this for initialization
    void Start () {

        float startVolume = PlayerPrefs.GetFloat(volumeKey, 1f);
        sound.GetComponentInChildren<Slider>().value = startVolume;

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ToggleOptionsMenu()
    {

        startButton.SetActive(!startButton.activeSelf);
        quitButton.SetActive(!quitButton.activeSelf);
        optionsButton.SetActive(!optionsButton.activeSelf);

        sound.SetActive(!sound.activeSelf);
        optionReturn.SetActive(!optionReturn.activeSelf);
    }

    public void SetSoundVolume()
    {

        float volume = sound.GetComponentInChildren<Slider>().value;
        PlayerPrefs.SetFloat(volumeKey, volume);
        MusicPlayer.instance.changeIntensityVolume(volume);
    }


}
