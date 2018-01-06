using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAlteration : MonoBehaviour, ISpell {

	public bool AlteredTime;
	public float LimitedSpan;
	public float Cooldown;
	[Range(0.01f, 1f)]
	public float TimeScale;
	public bool Available;

	private float currentCooldown;
	private float currentSpan;
	// Use this for initialization
	void Start () {
		currentCooldown = Cooldown;
		currentSpan = LimitedSpan;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Available)
			return;
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
			transform.parent.GetComponent<Movement>().TimeAffect = 1 / TimeScale;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
			GetComponent<Animator>().speed = 1 / TimeScale;
		}
		else
		{
			Time.timeScale = 1f;
			transform.parent.GetComponent<Movement>().TimeAffect = 1f;
			Time.fixedDeltaTime = 0.02F;
			GetComponent<Animator>().speed = 1f;
		}
	}

	public void HandleSwitch(bool idle)
	{
		if (idle)
		{
			Available = false;
			AlteredTime = false;
			SlowTime(false);
		}
		else
		{
			Available = true;
		}
	}
}
