﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float Speed;
	public float CameraSpeed;
	public float MaxRotationSpeed;
	public float JumpForce;
	public float MoreGravity;
	private bool isMoving;
	public Transform playerModel;
	public float CameraMaxVerticalAngleSin;
	private bool isJumping;
	public Animator animator;

	//Spell
	public float TimeAffect;

    private string RY = "RY";
    private string RX = "RX";
    private string jump = "Jump";


    public Rigidbody rb;
	public Transform cameraTransform;
	private Vector3 characterDirection;
	private bool isFalling;

	private CharacterSwitcher cs;

	// Use this for initialization
	void Start () {
		isJumping = false;
		characterDirection = transform.forward;
		TimeAffect = 1f;

        if (InputController.instance.IsPS4Controller())
        {
            RY = "RYPS4";
            RX = "RXPS4";
            jump = "JumpPS4";
        }

        Debug.Log(RY);

		cs = GetComponent<CharacterSwitcher>();
    }

    // Update is called once per frame
    void Update () {

		// For curious maids
		float time = Time.deltaTime * TimeAffect;

		//RaycastHit col;
		//if (Physics.Raycast(ray.origin, ray.direction, out col))
		//	Debug.DrawLine(ray.origin, col.point, Color.red);
		//else
		//	Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        if (isMoving || Mathf.Abs(Input.GetAxis(RX)) != 0)
		{
			float angle = Vector3.SignedAngle(playerModel.forward, characterDirection, transform.up);
			angle = Mathf.Min(Mathf.Abs(angle), MaxRotationSpeed) * (angle >= 0 ? 1 : -1);
			playerModel.Rotate(0, angle, 0);
			transform.Translate((transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * Speed * time, Space.World);

		}
		if (cs.Ready && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
		{
			isMoving = true;
			characterDirection = Vector3.Normalize(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"));
			animator.speed = Mathf.Min(1, Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))) * TimeAffect;
			animator.SetBool("isWalking", true);
			if (Mathf.Abs(Input.GetAxis("Vertical")) >= 0.6f || Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.6f)
				animator.SetBool("isRunning", true);
			else
				animator.SetBool("isRunning", false);


		}
		else
		{
			isMoving = false;
			animator.SetBool("isWalking", false);
			animator.SetBool("isRunning", false);

			animator.speed = 1 * TimeAffect;
		}
		Debug.DrawRay(playerModel.position, rb.velocity, Color.cyan);
		if (Mathf.Abs(rb.velocity.y) < 0.1f * TimeAffect)
		{
			isJumping = false;
			animator.SetBool("isJumping", false);
		}
		else
		{
			isJumping = true;
		}
		
		if (rb.velocity.y < -1f)
		{
			isFalling = true;
			animator.SetBool("isFalling", true);
		}
		else
		{
			isFalling = false;
			animator.SetBool("isFalling", false);

		}

		if (isFalling || isJumping)
			rb.velocity -= MoreGravity * Time.deltaTime * transform.up / Time.timeScale;

		if (Input.GetButtonDown(jump) && !isJumping)
		{
			rb.AddForce(transform.up * JumpForce , ForceMode.Impulse);
			animator.SetBool("isJumping", true);
		}

		var cameraRotation = cameraTransform.rotation;
		if (cs.Ready)
		{
			cameraTransform.Rotate(Input.GetAxis(RY) * Vector3.right * time * CameraSpeed, Space.Self);
			transform.Rotate(Input.GetAxis(RX) * Vector3.up * time * CameraSpeed, Space.Self);
		}
		if (Mathf.Abs(Mathf.Sin(cameraTransform.eulerAngles.x * Mathf.Deg2Rad)) > CameraMaxVerticalAngleSin )
			cameraTransform.rotation = cameraRotation;


	}
      
}
