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
	public float TimeAffect;


	public Rigidbody rb;
	public Transform cameraTransform;
	private Vector3 characterDirection;
	// Use this for initialization
	void Start () {
		isJumping = false;
		characterDirection = transform.forward;
		TimeAffect = 1f;
	}

	// Update is called once per frame
	void Update () {
		float time = Time.deltaTime * TimeAffect;
		if (isMoving || Mathf.Abs(Input.GetAxis("RX")) != 0)
		{
			float angle = Vector3.SignedAngle(playerModel.forward, characterDirection, transform.up);
			angle = Mathf.Min(Mathf.Abs(angle), MaxRotationSpeed) * (angle >= 0 ? 1 : -1);
			playerModel.Rotate(0, angle, 0);
			transform.Translate((transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * Speed * time, Space.World);

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
		if (Input.GetButtonDown("Fire2"))
		{
			Time.timeScale = 0.2f;
			TimeAffect = 5f;
		}
		if (isJumping)
			rb.velocity -= MoreGravity * time * transform.up;

		if (Input.GetButtonDown("Jump") && !isJumping)
		{
			rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
		}
		var cameraRotation = cameraTransform.rotation;
		cameraTransform.Rotate(Input.GetAxis("RY") * transform.right * time * CameraSpeed, Space.World);
		if (Mathf.Abs(Mathf.Sin(cameraTransform.eulerAngles.x * Mathf.Deg2Rad)) > CameraMaxVerticalAngleSin )
			cameraTransform.rotation = cameraRotation;
		transform.Rotate(Input.GetAxis("RX") * transform.up * time * CameraSpeed, Space.World);
	}
}
