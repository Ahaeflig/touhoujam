using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaChild : MonoBehaviour, IMechanism{
	public bool Activate(MechanismInfo info)
	{
		return transform.parent.GetComponent<IMechanism>().Activate(info);
	}

	public Vector3 GetGluedValue()
	{
		return transform.parent.GetComponent<IMechanism>().GetGluedValue();

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
