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
	private bool dead;
	public GameObject EffectOnEffective;
	public float DamagePerSec;
	private GameObject target;
	public float Timeout;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * Speed);
		dead = false;
		alpha = 1;
	}


	void OnCollisionEnter(Collision collision)
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
			target = collision.collider.gameObject;
			Instantiate(EffectOnEffective, transform);
		}

	}

	public void FixDirection(Vector3 target)
	{
		transform.LookAt(target);
	}

	public void Kill()
	{
		dead = true;
	}

	// Update is called once per frame
	void Update () {
		Lifetime -= Time.deltaTime;
		if (Lifetime <= 0f && !dead)
			Kill();

		if (hasCollided && alpha <= 0)
		{
			if (hasEffect)
				foreach (var c in GetComponentsInChildren<ParticleSystem>())
					c.Stop();
		}

		if (dead)
			Timeout -= Time.deltaTime;

		if (dead && Timeout <= 0)
			Destroy(gameObject);

		if (alpha > 0f && dead)
			alpha -= Time.deltaTime;

		if (hasEffect)
		{
			target.GetComponent<EvilScript>().Damage(gameObject, DamagePerSec * Time.deltaTime);
		}

		Renderer rend = GetComponent<Renderer>();
		var material = rend.material;
		rend.material.SetColor("_Color", new Color(material.color.r, material.color.g, material.color.b, alpha));
		
	}
}
