using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMaster : MonoBehaviour {

	public float Cooldown;
	public GameObject seal;
	public Camera Cam;
	private float currentCooldown;

	// Use this for initialization
	void Start () {
		currentCooldown = Cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Cam.ScreenPointToRay(new Vector3(0.5f * Screen.width, 0.5f * Screen.height, 0));
		Debug.DrawRay(ray.origin, ray.direction * 40, Color.green);
		if (Input.GetButtonDown("Fire") && currentCooldown >= Cooldown)
		{
			currentCooldown = 0f;
			var o = Instantiate(seal, transform.parent.transform.position + transform.parent.transform.forward * 2, transform.rotation);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
				o.GetComponent<SealScript>().FixDirection(hit.point);
		}

		if (currentCooldown <= Cooldown)
			currentCooldown += Time.deltaTime;
	}
}
