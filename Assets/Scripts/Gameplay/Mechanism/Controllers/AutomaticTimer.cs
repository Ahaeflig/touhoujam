using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticTimer : MonoBehaviour, IActivable {

	public bool IsEnabled;
	public GameObject[] targets;
	private IMechanism[] scripts;
	private float timer;
	public float SequenceTime;
	private bool debugSwitch;
	private MechanismInfo info;

	public void Activate()
	{
		IsEnabled = !IsEnabled;
	}

	// Use this for initialization
	void Start() {
		timer = 0f;
		scripts = new IMechanism[targets.Length];
		for (int i = 0; i < targets.Length; i++)
			scripts[i] = targets[i].GetComponent<IMechanism>();

		debugSwitch = false;
		info = new MechanismInfo();
		info.isRelative = true;
		info.newLocationState = 1;
		info.newRotationState = 1;
		info.newScaleState = 1;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= SequenceTime)
		{

			foreach (var s in scripts)
			{
				s.Activate(info);
			}

			timer = 0f;
			debugSwitch = !debugSwitch;
		}
	}

	public float ActivateSpecial()
	{
		return 0;
	}
}
