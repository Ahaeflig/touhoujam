using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour, IActivable {

	public GameObject Target;
	public bool isEnabled;
	private IMechanism script;
	public Material MaterialOn;
	public Material MaterialOff;
	// Use this for initialization
	void Start () {
		script = Target.GetComponent<IMechanism>();
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetButtonDown("Fire1"))
		//{
		//	isEnabled = !isEnabled;
		//	SetState(isEnabled);
		//}
	}

	public void Activate()
	{
		if (!script.Activate())
			return;

		isEnabled = !isEnabled;
		if (isEnabled)
			GetComponent<Renderer>().material = MaterialOn;
		else
			GetComponent<Renderer>().material = MaterialOff;
	}
}
