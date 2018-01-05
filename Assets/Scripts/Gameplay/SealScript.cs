using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealScript : MonoBehaviour {

	public float Speed;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * Speed);
	}

	private void OnCollisionEnter(Collision collision)
	{
		rb.velocity = Vector3.zero;
		rb.isKinematic = false;
		rb.angularVelocity = Vector3.zero;
		GetComponent<BoxCollider>().enabled = false;
		var normal = collision.contacts[0].point - transform.position;
		normal = new Vector3(normal.x, 0, normal.z);
		transform.rotation *= Quaternion.Euler(transform.right * 90);

		var m = collision.collider.gameObject.GetComponent<IActivable>();
		if (m != null)
			m.Activate();
	}

	public void FixDirection(Vector3 target)
	{
		transform.LookAt(target);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
