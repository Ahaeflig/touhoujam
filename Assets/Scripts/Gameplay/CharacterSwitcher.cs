using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour {

	public GameObject[] InitialCharacters;

	private List<GameObject> characters;
	private int currentIndex;
	private GameObject currentCharacter;
	private float transition;
	public bool Ready { get; set; }
	private Vector3 fromPosition;
	private Quaternion fromRotation;
	public Vector3 PositionOffset;

	public float CallRange;
	
	// Use this for initialization
	void Start () {
		currentIndex = 0;
		characters = new List<GameObject>(InitialCharacters);
		currentCharacter = characters[0];
		//currentCharacter.transform.position = transform.position;
		//currentCharacter.transform.parent = transform;
		currentCharacter.GetComponent<Rigidbody>().useGravity = false;
		currentCharacter.GetComponent<SphereCollider>().enabled = false;
		currentCharacter.GetComponent<Rigidbody>().isKinematic = true;
		transition = 1f;
		Ready = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("SwitchCharacter") && Ready)
			SwitchCharacter(currentIndex + 1);
		if (transition < 1f)
		{
			transition += Time.deltaTime;
			transform.position = Vector3.Lerp(fromPosition, currentCharacter.transform.position, transition);
			transform.rotation = Quaternion.Slerp(fromRotation, currentCharacter.transform.rotation, transition);
		}
		else
		{
			CompleteChange(Ready);
		}

		if (Input.GetButtonDown("CallCharacters"))
		{
			foreach (var c in characters)
			{
				if (c != currentCharacter)
				{
					if (Vector3.Distance(gameObject.transform.position, c.transform.position) <= CallRange)
					{
						var ia = c.GetComponent<IAFollower>();
						if (!ia.Following)
						{
							ia.Call(true);
							ia.Following = true;
						}
					}
				}
			}
		}

		if (Input.GetButtonDown("DismissCharacters"))
		{
			foreach (var c in characters)
			{
				if (c != currentCharacter)
				{
					if (Vector3.Distance(gameObject.transform.position, c.transform.position) <= CallRange)
					{
						var ia = c.GetComponent<IAFollower>();
						if (ia.Following)
						{
							ia.Call(false);
							ia.Following = false;
						}
					}
				}
			}
		}
	}

	void CompleteChange(bool ready)
	{
		if (ready)
			return;

		this.Ready = true;
		currentCharacter.transform.parent = transform;
		transform.Translate(-PositionOffset);
		currentCharacter.transform.localPosition = PositionOffset;
		currentCharacter.GetComponent<Rigidbody>().useGravity = false;
		currentCharacter.GetComponent<SphereCollider>().enabled = false;
		currentCharacter.GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<SphereCollider>().enabled = true;
		var c = currentCharacter.GetComponent<ISpell>();
		if (c != null)
			c.HandleSwitch(false);
	}

	void SwitchCharacter(int index)
	{
		var c = currentCharacter.GetComponent<ISpell>();
		if (c != null)
			c.HandleSwitch(true);


		foreach (var p in characters)
		{
			p.GetComponent<IAFollower>().Following = false;
		}
		currentCharacter.transform.parent = null;
		index = index % characters.Count;
		currentCharacter.GetComponent<Rigidbody>().useGravity = true;
		currentCharacter.GetComponent<SphereCollider>().enabled = true;
		currentCharacter.GetComponent<Rigidbody>().isKinematic = false;
		currentCharacter.GetComponent<IAFollower>().isPlayer = false;

		currentCharacter = characters[index];
		currentIndex = index;
		fromPosition = transform.position;
		fromRotation= transform.rotation;
		Ready = false;
		transition = 0f;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<SphereCollider>().enabled = false;
		currentCharacter.GetComponent<IAFollower>().isPlayer = true;


		UpdateComponents();
	}

	public void UpdateComponents()
	{
		GetComponent<Movement>().animator = currentCharacter.GetComponent<Animator>();
		GetComponent<Movement>().playerModel = currentCharacter.transform;
		GetComponent<Interaction>().PlayerModel = currentCharacter;
	}
}
