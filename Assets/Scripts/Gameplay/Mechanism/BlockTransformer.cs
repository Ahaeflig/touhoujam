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
		ToLocation[0] = transform.localPosition;
		ToRotation[0] = transform.localEulerAngles;
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

		transform.localPosition = Vector3.Lerp(ToLocation[currentLocationState], ToLocation[locationState], Transition);
		transform.localEulerAngles = Vector3.Lerp(ToRotation[currentRotationState], ToRotation[rotationState], Transition);
		//transform.rotation = Quaternion.Slerp(previousRotation, previousRotation * Quaternion.Euler(ToRotation[rotationState]), Transition);
		transform.localScale = Vector3.Lerp(ToScale[currentScaleState], ToScale[scaleState], Transition);
	}

	public bool Activate(MechanismInfo info)
	{
		if (Transition <= 0f || Transition >= 1f)
		{
			this.locationState = ((info.isRelative ? currentLocationState : 0) + info.newLocationState) % ToLocation.Length;
			this.rotationState = ((info.isRelative ? currentRotationState : 0) + info.newRotationState) % ToRotation.Length;
			this.scaleState = ((info.isRelative ? currentScaleState : 0) + info.newScaleState) % ToScale.Length;
			return true;
		}

		return false;
	}
}
