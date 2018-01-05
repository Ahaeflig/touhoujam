using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTransformer : MonoBehaviour, IMechanism {

	public bool isActivated = false;
	public float Transition = 0f;
	public float TransitionSpeed = 0.1f;
	public Vector3[] ToLocation;
	public Vector3[] ToRotation;
	public Vector3[] ToScale;
	private int currentLocationState = 0;
	private int currentRotationState = 0;
	private int currentScaleState = 0;
	private int locationState = 0;
	private int rotationState = 0;
	private int scaleState = 0;

	// Use this for initialization
	void Start () {

		//Simply set the initial transform state as we see in the Scene before game launch to (greatly) simplify states management
		var trueToLocation = new Vector3[ToLocation.Length + 1];
		var trueToRotation = new Vector3[ToRotation.Length + 1];
		var trueToScale = new Vector3[ToScale.Length + 1];
		ToLocation.CopyTo(trueToLocation, 1);
		ToRotation.CopyTo(trueToRotation, 1);
		ToScale.CopyTo(trueToScale, 1);
		ToLocation = trueToLocation;
		ToRotation = trueToRotation;
		ToScale = trueToScale;
		ToLocation[0] = transform.position;
		ToRotation[0] = transform.eulerAngles;
		ToScale[0] = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentLocationState != locationState || currentRotationState != rotationState || currentScaleState != scaleState)
			Transition = Mathf.Min(Transition + TransitionSpeed * Time.deltaTime, 1f);

		if (Transition == 1f)
		{
			currentLocationState = locationState;
			currentRotationState = rotationState;
			currentScaleState = scaleState;
			Transition = 0f;
		}

		transform.position = Vector3.Lerp(ToLocation[currentLocationState], ToLocation[locationState], Transition);
		transform.eulerAngles = Vector3.Lerp(ToRotation[currentRotationState], ToRotation[rotationState], Transition);
		transform.localScale = Vector3.Lerp(ToScale[currentScaleState], ToScale[scaleState], Transition);
	}

	public bool Activate(int locationState = 1, int rotationState = 1, int scaleState = 1, bool relative = true)
	{
		if (Transition <= 0f || Transition >= 1f)
		{
			this.locationState = ((relative ? currentLocationState : 0) + locationState) % ToLocation.Length;
			this.rotationState = ((relative ? currentRotationState : 0) + rotationState) % ToRotation.Length;
			this.scaleState = ((relative ? currentScaleState : 0) + scaleState) % ToScale.Length;
			return true;
		}

		return false;
	}
}
