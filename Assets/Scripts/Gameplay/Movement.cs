using System.Collections;
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


	public Rigidbody rb;
	public Transform cameraTransform;
	private Vector3 characterDirection;
	// Use this for initialization
	void Start () {
		isJumping = false;

	}

	// Update is called once per frame
	void Update () {
		if (isMoving || Mathf.Abs(Input.GetAxis("RX")) != 0)
		{
			float angle = Vector3.SignedAngle(playerModel.forward, characterDirection, transform.up);
			angle = Mathf.Min(Mathf.Abs(angle), MaxRotationSpeed) * (angle >= 0 ? 1 : -1);
			playerModel.Rotate(0, angle, 0);
			transform.Translate((transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * Speed * Time.deltaTime, Space.World);

		}
		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			isMoving = true;
			characterDirection = Vector3.Normalize(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"));
		}
		else
		{
			isMoving = false;
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
		var cameraRotation = cameraTransform.rotation;
		cameraTransform.Rotate(Input.GetAxis("RY") * transform.right * Time.deltaTime * CameraSpeed, Space.World);
		if (Mathf.Abs(Mathf.Sin(cameraTransform.eulerAngles.x * Mathf.Deg2Rad)) > CameraMaxVerticalAngleSin )
			cameraTransform.rotation = cameraRotation;
		transform.Rotate(Input.GetAxis("RX") * transform.up * Time.deltaTime * CameraSpeed, Space.World);
	}
}
