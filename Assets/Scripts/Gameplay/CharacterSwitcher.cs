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

    private string SwitchCharacterString = "SwitchCharacter";
    private string CallCharactersString = "CallCharacters";

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

        if (InputController.instance.IsPS4Controller())
        {
            SwitchCharacterString = "SwitchCharacter";
            CallCharactersString = "CallCharactersPS4";
        }
	}

	internal GameObject Unlink()
	{
		currentCharacter.GetComponent<Rigidbody>().useGravity = true;
		currentCharacter.GetComponent<SphereCollider>().enabled = true;
		currentCharacter.GetComponent<Rigidbody>().isKinematic = false;
		currentCharacter.GetComponent<IAFollower>().isPlayer = false;
		currentCharacter.transform.parent = null;
		GetComponent<Movement>().SetFreeCam(true);
		return currentCharacter;
	}

	internal void Link(GameObject o)
	{
		var i = characters.FindIndex(x => o.Equals(x));
		SwitchCharacter(i);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(SwitchCharacterString) && Ready)
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

		if (Input.GetButtonDown(CallCharactersString))
		{
			var l = new List<GameObject>();
			foreach (var c in GameObject.FindGameObjectsWithTag("Suku"))
			{
				if (!c.GetComponent<IAFollower>().isPlayer && Vector3.Distance(gameObject.transform.position, c.transform.position) <= CallRange)
				{
					l.Add(c);
				}

			}
				var b = true;
				var l2 = new List<GameObject>();
				foreach (var suku in l)
				{
					var b2 = suku.GetComponent<IAFollower>().Following;
					b = b && b2;
					if (!b2)
						l2.Add(suku);
				}

				if (b)
					foreach (var suku in l)
						suku.GetComponent<IAFollower>().Call(false);
				else
					foreach (var suku in l2)
						suku.GetComponent<IAFollower>().Call(true);
		}

	}

	void CompleteChange(bool ready)
	{
		if (ready)
			return;

		this.Ready = true;
		currentCharacter.GetComponent<IAFollower>().isPlayer = true;
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


		UpdateComponents();
	}

	public void UpdateComponents()
	{
		GetComponent<Movement>().animator = currentCharacter.GetComponent<Animator>();
		GetComponent<Movement>().playerModel = currentCharacter.transform;
		GetComponent<Interaction>().PlayerModel = currentCharacter;
	}
}
