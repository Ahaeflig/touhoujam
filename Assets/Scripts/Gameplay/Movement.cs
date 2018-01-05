using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float Speed;
	public float CameraSpeed;
	public float JumpForce;
	public float MoreGravity;
	private bool isJumping;

	public Rigidbody rb;
	public Transform cameraTransform;
	private Vector3 characterDirection;
	// Use this for initialization
	void Start () {
		isJumping = false;

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			characterDirection = Vector3.Normalize(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"));
			transform.Translate((transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * Speed * Time.deltaTime, Space.World);
		}
		if (Mathf.Abs(rb.velocity.y) - 0.05f < 0)
		{
			isJumping = false;
		}
		else
		{
			isJumping = true;
		}
		if (isJumping)
			rb.velocity -= MoreGravity * Time.deltaTime * transform.up;

		if (Input.GetButtonDown("Jump") && !isJumping)
		{
			rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
		}
		cameraTransform.Rotate(Input.GetAxis("RY") * transform.right * Time.deltaTime * CameraSpeed, Space.World);
		transform.Rotate(Input.GetAxis("RX") * transform.up * Time.deltaTime * CameraSpeed, Space.World);
	}
}
