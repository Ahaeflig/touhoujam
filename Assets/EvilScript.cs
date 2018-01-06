using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilScript : MonoBehaviour {

	private List<GameObject> seals;
	public float Health;
	public float Timeout;
	private bool dead;
	// Use this for initialization
	void Start () {
		seals = new List<GameObject>();
		dead = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Health <= 0f && !dead)
		{
			dead = true;
			foreach(var o in seals)
				o.GetComponent<SealScript>().Kill();

			foreach (var c in gameObject.GetComponentsInChildren<ParticleSystem>())
				c.Stop();

			GetComponent<BoxCollider>().enabled = false;
		}

		if (dead)
			Timeout -= Time.deltaTime;
	}

	public void Damage(GameObject gameObject, float v)
	{
		if (!seals.Contains(gameObject))
			seals.Add(gameObject);

		Health -= v;
	}
}
