using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

	public GameObject PlayerModel;
	public float Range;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;
			if (Physics.Raycast(PlayerModel.transform.position, PlayerModel.transform.forward, out hit, Range))
			{
				print(hit);
				if (hit.collider.gameObject.tag.Equals("Switch"))
					hit.collider.gameObject.GetComponent<IActivable>().Switch();
			}
		}
	}
}
