using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMaster : MonoBehaviour {

	public float Cooldown;
	public GameObject seal;
	public Camera Cam;
	private float currentCooldown;
	public float RangeMax;

	// Use this for initialization
	void Start () {
		currentCooldown = Cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Cam.ScreenPointToRay(new Vector3(0.5f * Screen.width, 0.5f * Screen.height));
		Debug.DrawRay(ray.origin, ray.direction * 40, Color.green);
		if (transform.parent != null && Input.GetButtonDown("Fire") && currentCooldown >= Cooldown)
		{
			currentCooldown = 0f;
			var o = Instantiate(seal, transform.position + Vector3.up * 0.5f + transform.parent.transform.forward * 1, transform.rotation);
			o.GetComponent<SealScript>().pc = gameObject;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, RangeMax, ~( 1 << 2)))
			{
				o.GetComponent<SealScript>().FixDirection(hit.point);
			}
			else
			{
				o.transform.position = transform.position + transform.forward + Vector3.up * 0.5f;
				//o.transform.rotation = transform.rotation;
				o.transform.rotation = transform.parent.transform.rotation;
			}
		}

		if (currentCooldown <= Cooldown)
			currentCooldown += Time.deltaTime;
	}
}
