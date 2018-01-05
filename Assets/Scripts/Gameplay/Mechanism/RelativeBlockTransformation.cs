using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeBlockTransformation : MonoBehaviour, IMechanism {

	public bool Translate;
	public bool Rotate;
	public bool Rescale;
	public Vector3 Translation;
	public Vector3 Rotation;
	public Vector3 Scale;
	public float TransitionSpeed;
	public float Transition;
	private Vector3 previousPosition;
	private Quaternion previousRotation;
	private Vector3 previousScale;
	private Rigidbody rb;


	public bool Activate(MechanismInfo info)
	{
		previousPosition = new Vector3(transform.position.x % 360, transform.position.y % 360, transform.position.z % 360);
		//previousRotation = new Vector3(transform.localEulerAngles.x % 360, transform.localEulerAngles.y % 360, transform.localEulerAngles.z % 360);
		previousRotation = transform.rotation;
		previousScale = new Vector3(transform.localScale.x % 360, transform.localScale.y % 360, transform.localScale.z % 360);
		//print("Translating from " + previousRotation + " to " + (previousRotation + Rotation));
		Transition = 0f;
		return true;
	}

	// Use this for initialization
	void Start () {
		Transition = 1f;
		//rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Transition < 1f)
		{
			Transition = Mathf.Min(Transition + TransitionSpeed * Time.deltaTime, 1f);
			if (Translate)
				transform.position = Vector3.Lerp(previousPosition, previousPosition + Translation, Transition);

			if (Rotate)
			{
				//var targetAngle = previousRotation + Rotation;
				//transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.AngleAxis(Rotation.x, transform.right), Transition);
				transform.rotation = Quaternion.Slerp(previousRotation, previousRotation * Quaternion.Euler(Rotation), Transition);
				//transform.rotation = Quaternion.Lerp(Quaternion.Euler(previousRotation), Quaternion.Euler(previousRotation + Rotation), Transition);
				//transform.Rotate(Rotation * Time.deltaTime * TransitionSpeed);
				//rb.MoveRotation(Quaternion.Euler(Vector3.Lerp(previousRotation, previousRotation + Rotation, Transition)));
				//rb.AddRelativeTorque(Rotation);
			}

			if (Rescale)
				transform.localScale = Vector3.Lerp(previousScale, previousScale + Scale, Transition);
		}
	}
}
