using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertSphereScript : MonoBehaviour {

	public float GrowSpeed;
	public Vector3 Goal;
	public float Transition;
	public bool SetToKill;
	private Vector3 fromScale;
	private Camera cam;
	// Use this for initialization
	void Start () {
		SetToKill = false;
		Transition = 0f;
		cam = Camera.allCameras[0];
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
		if (Transition < 1f)
		{
			Transition = Mathf.Min(Transition + Time.deltaTime * GrowSpeed, 1f);
			transform.localScale = Vector3.Lerp(fromScale, Goal, Transition);
		}
		transform.position = cam.transform.position + cam.transform.forward;
		if (SetToKill && Transition == 1f)
			Destroy(gameObject);
	}

	public void Kill()
	{
		Transition = 0f;
		Goal = Vector3.zero;
		fromScale = transform.localScale;
	}


}
