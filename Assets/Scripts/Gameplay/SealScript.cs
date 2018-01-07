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
	private bool hasToggle;
	private Vector3? otherPosition;
	private GameObject other;
	public GameObject pc;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * Speed);
		dead = false;
		alpha = 1;
		hasToggle = false;
		otherPosition = null;
	}


	void OnCollisionEnter(Collision collision)
	{
		other = collision.collider.gameObject;
		rb.velocity = Vector3.zero;
		rb.isKinematic = false;
		rb.angularVelocity = Vector3.zero;
		GetComponent<BoxCollider>().enabled = false;
		var normal = collision.contacts[0].point - transform.position;
		normal = new Vector3(normal.x, 0, normal.z);
		transform.rotation *= Quaternion.Euler(transform.right * 90);
		otherPosition = other.transform.position;
		hasCollided = true;
		hasEffect = other.tag.Equals("Evil");
		if (hasEffect)
		{
			target = other;
			Instantiate(EffectOnEffective, transform);
		}

		if (!hasEffect) //Switch alternative
		{
			hasEffect = other.tag.Equals("Switch");
			if (hasEffect)
			{
				target = other;
				//var o = Instantiate(EffectOnEffective, transform.position, transform.rotation);
				//o.transform.LookAt(pc.transform.position);
				//o.transform.Rotate(new Vector3(0, 90, 90), Space.World);
			}
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

		if (hasCollided)
		{
			if (other != null)
			{
				var op = (Vector3)otherPosition;
				transform.position += other.transform.position - op;
				otherPosition = other.transform.position;
			}
			else
			{
				rb.useGravity = true;
			}
		}
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
			if (target.tag.Equals("Evil"))
				target.GetComponent<EvilScript>().Damage(gameObject, DamagePerSec * Time.deltaTime);
			if (!hasToggle && target.tag.Equals("Switch"))
			{
				hasToggle = true;
				Lifetime = target.GetComponent<IActivable>().ActivateSpecial();
			}
			
		}

		Renderer rend = GetComponent<Renderer>();
		var material = rend.material;
		rend.material.SetColor("_Color", new Color(material.color.r, material.color.g, material.color.b, alpha));
		
	}
}
