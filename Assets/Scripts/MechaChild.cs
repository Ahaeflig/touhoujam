using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaChild : MonoBehaviour, IMechanism{

	private IMechanism parentMechanism;

	public bool Activate(MechanismInfo info)
	{
		if (parentMechanism != null)
			return parentMechanism.Activate(info);
		else
			return true;
	}

	public Vector3 GetGluedValue()
	{
		if (parentMechanism != null)
			return parentMechanism.GetGluedValue();
		else
			return Vector3.zero;

	}

	// Use this for initialization
	void Start () {
		parentMechanism = transform.parent.GetComponent<IMechanism>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
