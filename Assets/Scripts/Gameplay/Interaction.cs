using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

	public GameObject PlayerModel;
	public Camera Cam;
	public float Range;
	public float Deadzone;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			RaycastHit hit;
			Ray ray = Cam.ScreenPointToRay(new Vector3(0.5f * Screen.width, 0.5f * Screen.height, 0));
			Debug.DrawRay(ray.origin, ray.direction * Range, Color.red);
			Debug.DrawRay(ray.origin, ray.direction * Deadzone, Color.gray);
		if (Input.GetButtonDown("Activate"))
		{
			if (Physics.Raycast(ray, out hit, Range))
			{
				if (hit.distance >= Deadzone && hit.collider.gameObject.tag.Equals("Switch"))
					hit.collider.gameObject.GetComponent<IActivable>().Activate();
			}
		}
	}
}
