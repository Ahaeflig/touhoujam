using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour {

	public GameObject Cam;
	public GameObject CameraAnchor;
	public float MaxDistanceCamera;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Physics.Raycast(transform.position,  Cam.transform.position - transform.position, out hit, MaxDistanceCamera);
		//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
		if (hit.collider != null)
		{
			Debug.DrawLine(transform.position, hit.point, Color.red);
			Cam.transform.position = hit.point;
		}
		else
		{
			Cam.transform.position = CameraAnchor.transform.position;
		}

		//else
		//	Camera.transform.position = Vector3.Normalize(transform.position - Camera.transform.position) * MaxDistanceCamera * -1 + transform.position;
	
	}
}
