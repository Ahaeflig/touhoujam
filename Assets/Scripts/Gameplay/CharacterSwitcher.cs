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
	private bool ready;
	private Vector3 fromPosition;
	private Quaternion fromRotation;
	public Vector3 PositionOffset;
	
	// Use this for initialization
	void Start () {
		currentIndex = 0;
		characters = new List<GameObject>(InitialCharacters);
		currentCharacter = characters[0];
		//currentCharacter.transform.position = transform.position;
		//currentCharacter.transform.parent = transform;
		currentCharacter.GetComponent<Rigidbody>().useGravity = false;
		currentCharacter.GetComponent<BoxCollider>().enabled = false;
		currentCharacter.GetComponent<Rigidbody>().isKinematic = true;
		transition = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("SwitchCharacter"))
			SwitchCharacter(currentIndex + 1);
		if (transition < 1f)
		{
			transition += Time.deltaTime;
			transform.position = Vector3.Lerp(fromPosition, currentCharacter.transform.position, transition);
			transform.rotation = Quaternion.Slerp(fromRotation, currentCharacter.transform.rotation, transition);
		}
		else
		{
			CompleteChange(ready);
		}
	}

	void CompleteChange(bool ready)
	{
		if (ready)
			return;

		ready = true;
		currentCharacter.transform.parent = transform;
		currentCharacter.transform.localPosition = PositionOffset;
		currentCharacter.GetComponent<Rigidbody>().useGravity = false;
		currentCharacter.GetComponent<BoxCollider>().enabled = false;
		currentCharacter.GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<BoxCollider>().enabled = true;
		var c = currentCharacter.GetComponent<ISpell>();
		if (c != null)
			c.HandleSwitch(false);
	}

	void SwitchCharacter(int index)
	{
		var c = currentCharacter.GetComponent<ISpell>();
		if (c != null)
			c.HandleSwitch(true);

		currentCharacter.transform.parent = null;
		index = index % characters.Count;
		currentCharacter.GetComponent<Rigidbody>().useGravity = true;
		currentCharacter.GetComponent<BoxCollider>().enabled = true;
		currentCharacter.GetComponent<Rigidbody>().isKinematic = false;
		currentCharacter = characters[index];
		currentIndex = index;
		fromPosition = transform.position;
		fromRotation= transform.rotation;
		ready = false;
		transition = 0f;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<BoxCollider>().enabled = false;

		UpdateComponents();
	}

	public void UpdateComponents()
	{
		GetComponent<Movement>().animator = currentCharacter.GetComponent<Animator>();
		GetComponent<Movement>().playerModel = currentCharacter.transform;
		GetComponent<Interaction>().PlayerModel = currentCharacter;
	}
}
