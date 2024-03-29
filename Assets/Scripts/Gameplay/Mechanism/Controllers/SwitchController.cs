﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour, IActivable {

	public GameObject Target;
	public bool isEnabled;
	private IMechanism script;
	public Material MaterialOn;
	public Material MaterialOff;

    public AudioClip soundFx;
    public AudioClip soundFx2;

    private MechanismInfo info;
	public float Timeout;
	private float currentTimeout;
	// Use this for initialization
	void Start () {
		script = Target.GetComponent<IMechanism>();
		info = new MechanismInfo
		{
			newLocationState = 1,
			newRotationState = 1,
			newScaleState = 1,
			isRelative = true
		};
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetButtonDown("Fire1"))
		//{
		//	isEnabled = !isEnabled;
		//	SetState(isEnabled);
		//}
		if (Timeout > 0f)
		{
			if (isEnabled && currentTimeout <= 0f)
				Activate();

			currentTimeout -= Time.deltaTime;
		}
	}

	public void Activate()
	{
		currentTimeout = Timeout;
		if (!script.Activate(info))
			return;

		isEnabled = !isEnabled;
		if (isEnabled) { 
			GetComponent<Renderer>().material = MaterialOn;
            MusicPlayer.instance.playSong(soundFx);
        }
        else { 
			GetComponent<Renderer>().material = MaterialOff;
            MusicPlayer.instance.playSong(soundFx2);
        }
    }

	public float ActivateSpecial()
	{
		Activate();
		return Timeout;
	}
}
