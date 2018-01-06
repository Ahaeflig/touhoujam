using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealScript : MonoBehaviour {

	public float Speed;
	public float Lifetime;
	private Rigidbody rb;
	private bool hasCollided;
	private bool hasEffect;
	private float alpha;
	public Material material;

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

		hasCollided = true;
		hasEffect = collision.collider.gameObject.tag.Equals("Evil");
		if (hasEffect)
		{
			//Particles or something
		}

	}

	public void FixDirection(Vector3 target)
	{
		transform.LookAt(target);
	}

	public void Kill()
	{
		alpha = 1;
	}

	// Update is called once per frame
	void Update () {
		Lifetime -= Time.deltaTime;
		if (Lifetime <= 0f)
			Kill();

		if (hasCollided && alpha <= 0)
			Destroy(gameObject);

		if (alpha >= 1f)
			alpha -= Time.deltaTime;
		material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
		
	}
}
