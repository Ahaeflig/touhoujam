using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour, IMechanism {

	public bool isActivated = false;
	public float Transition = 0f;
	public float TransitionSpeed = 0.1f;
	public Vector3 From;
	public Vector3 To;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isActivated)
			Transition = Mathf.Min(Transition + TransitionSpeed * Time.deltaTime, 1f);
		else
			Transition = Mathf.Max(Transition - TransitionSpeed * Time.deltaTime, 0f);

		transform.position = Vector3.Lerp(From, To, Transition);
	}

	public bool Activate()
	{
		if (Transition <= 0f || Transition >= 1f)
		{
			isActivated = !isActivated;
			return true;
		}

		return false;
	}
}
