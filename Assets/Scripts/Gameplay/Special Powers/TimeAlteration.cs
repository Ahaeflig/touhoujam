using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAlteration : MonoBehaviour {

	public bool AlteredTime;
	public float LimitedSpan;
	public float Cooldown;
	[Range(0.01f, 1f)]
	public float TimeScale;

	private float currentCooldown;
	private float currentSpan;
	// Use this for initialization
	void Start () {
		currentCooldown = Cooldown;
		currentSpan = LimitedSpan;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Spell") && currentCooldown >= Cooldown)
		{
			currentCooldown = 0f;
			currentSpan = currentCooldown;
			AlteredTime = !AlteredTime;
			SlowTime(AlteredTime);
		}

		if (currentSpan > LimitedSpan)
		{
			currentSpan -= Time.deltaTime;
			if (currentSpan <= 0f)
				SlowTime(false);
		}

	}

	void SlowTime(bool active)
	{
		if (active)
		{
			Time.timeScale = TimeScale;
			GetComponent<Movement>().TimeAffect = 1 / TimeScale;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
		else
		{
			Time.timeScale = 1f;
			GetComponent<Movement>().TimeAffect = 1f;
			Time.fixedDeltaTime = 0.02F;
		}
	}
}
