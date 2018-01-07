using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAFollower : MonoBehaviour {

	public GameObject pc;
	public float Speed;
	public float frontRayRange;
	public float belowRayRange;
	public float hazardRayRange;
	public Vector3 hazardRayAngle;
	public float safeZoneFromPC;
	public Animator animator;
	private Rigidbody rb;
	public Vector3 raycastOffset;
	public GameObject body;
	private Quaternion initialRotation;

	public GameObject EffectLove;
	public GameObject EffectDislike;

	public float JumpForce;
	private bool isJumping;
	private bool isFalling;
	private float jumpIF;
	public float JumpNoCheck;

	public bool Following;

	public float MoreGravity;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		var pos = transform.position + raycastOffset;
		var rot = transform.rotation;
		var pcPos = pc.transform.position;
		jumpIF = Mathf.Max(jumpIF - Time.deltaTime / Time.timeScale, 0f);

		var projXZdistance = new Vector3(pcPos.x - pos.x, 0, pcPos.z - pos.z);
		var projXZdirection = Vector3.Normalize(projXZdistance);
		var movement = projXZdirection * Mathf.Min(Speed, Vector3.Magnitude(projXZdistance) - safeZoneFromPC) * Time.deltaTime;
		if (Following)
			transform.position += movement;

		RaycastHit rayFront;
		var collisionFront = Physics.Raycast(pos, projXZdirection, out rayFront, frontRayRange);
		RaycastHit rayBelow;
		var collisionGround = Physics.Raycast(pos, -Vector3.up, out rayBelow, belowRayRange);
		RaycastHit rayHazard;
		var collisionHazard = Physics.Raycast(pos, projXZdirection + hazardRayAngle, out rayHazard, hazardRayRange);
		var cSpeed = Vector3.Magnitude(movement) / Time.deltaTime;
		if (Following)
			transform.LookAt(pcPos + new Vector3(0, -pcPos.y + transform.position.y, 0), Vector3.up);

		if (collisionGround && rayBelow.collider.gameObject.tag == "Wall" && rayBelow.collider.gameObject.GetComponent<IMechanism>() != null)
			transform.position += rayBelow.collider.gameObject.GetComponent<IMechanism>().GetGluedValue();
		//var b = transform.rotation;
		//transform.LookAt(pcPos, Vector3.up);
		//body.transform.rotation = transform.rotation;
		//GetComponent<BoxCollider>().transform.rotation = initialRotation;
		//body.transform.rotation *= Quaternion.FromToRotation(body.transform.forward, pcPos - pos);

		Debug.DrawRay(pos, projXZdirection * frontRayRange, Color.blue);
		Debug.DrawRay(pos, -Vector3.up * belowRayRange, Color.green);
		Debug.DrawRay(pos, (projXZdirection + hazardRayAngle) * hazardRayRange, Color.red);
		

		if (!collisionGround)
		{
			if (rb.velocity.y > 0.1f)
			{
				isJumping = true;
				isFalling = false;
			}
			else
			{
				isJumping = false;
				isFalling = true;
			}
		}
		else
		{
			isFalling = false;
			isJumping = false;
		}

		if (cSpeed > 0.1f)
		{
			animator.SetBool("isRunning", false);
			animator.SetBool("isWalking", true);
		}
		if (cSpeed > Speed / 2)
		{
			animator.SetBool("isRunning", true);
		}
		else
		{
			animator.SetBool("isRunning", false);
			animator.SetBool("isWalking", false);
		}

		if (isFalling || isJumping)
			rb.velocity -= MoreGravity * Time.deltaTime * Vector3.up;

		if (Following)
			if ((collisionFront && rayFront.collider.gameObject.tag == "Wall" && collisionGround && rayBelow.collider.gameObject.tag == "Wall") || (!collisionHazard && collisionGround && rayBelow.collider.gameObject.tag == "Wall"))
			{
				if (!isJumping && !isFalling && jumpIF <= 0f)
				{
					print("jump");
					rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
					isJumping = true;
					jumpIF = JumpNoCheck;
				}
			}
	}

	public void Call(bool like)
	{
		if (like)
			Instantiate(EffectLove, gameObject.transform);
		else
			Instantiate(EffectDislike, gameObject.transform);
	}
}
