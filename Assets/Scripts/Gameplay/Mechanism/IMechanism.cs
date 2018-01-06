using UnityEngine;

internal interface IMechanism
{
	bool Activate(MechanismInfo info);
	Vector3 GetGluedValue();
}