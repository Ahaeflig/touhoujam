using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour {

	public GameObject Camera;
	public float MaxDistanceCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Physics.Raycast(transform.position,  Camera.transform.position - transform.position, out hit, MaxDistanceCamera);
		if (hit.collider != null)
		{
			Debug.DrawLine(transform.position, hit.point, Color.red);
			Camera.transform.position = hit.point;
		}

		//else
		//	Camera.transform.position = Vector3.Normalize(transform.position - Camera.transform.position) * MaxDistanceCamera * -1 + transform.position;
	
	}
}
