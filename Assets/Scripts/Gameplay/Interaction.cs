using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

	public GameObject PlayerModel;
	public Camera Cam;
	public float Range;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;
			Ray ray = Cam.ScreenPointToRay(new Vector3(0.5f * Screen.width, 0.5f * Screen.height, 0));
			//Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
			if (Physics.Raycast(ray, out hit, Range))
			{
				if (hit.collider.gameObject.tag.Equals("Switch"))
					hit.collider.gameObject.GetComponent<IActivable>().Activate();
			}
		}
	}
}
