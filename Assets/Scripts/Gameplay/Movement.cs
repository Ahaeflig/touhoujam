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
	public Animator animator;
	public float RaycastDownRange;

	//Spell
	public float TimeAffect;

    private string RY = "RY";
    private string RX = "RX";
    private string jump = "Jump";


    public Rigidbody rb;
	public Transform cameraTransform;
	private Vector3 characterDirection;
	private bool isFalling;
	private float jumpIF;
	public float JumpNoCheck;
	private CharacterSwitcher cs;
	private bool rbFix;
	public Vector3 trueVelocity { get; set; }
	public float gravityAcc;
	public Vector3 JumpImpulse { get; set; }
	public float AlteredTimeJumpMagicFactor;

	// Use this for initialization
	void Start () {
		gravityAcc = 0f;
		isJumping = false;
		characterDirection = transform.forward;
		TimeAffect = 1f;
		rbFix = false;
		trueVelocity = rb.velocity;

        if (InputController.instance.IsPS4Controller())
        {
            RY = "RYPS4";
            RX = "RXPS4";
            jump = "JumpPS4";
        }

        Debug.Log(RY);

		cs = GetComponent<CharacterSwitcher>();
    }

	void FixedUpdate()
	{

		if (TimeAffect != 1f)
		{
			if (isFalling || isJumping)
				gravityAcc += Time.deltaTime / Time.timeScale;
			else
				gravityAcc = 0f;

			rbFix = true;
			rb.velocity = new Vector3(trueVelocity.x, trueVelocity.y, trueVelocity.z) / Time.timeScale;
			rb.velocity += Physics.gravity * gravityAcc * ((1 / Time.timeScale) - 1) + JumpImpulse;
		}
	}

	void LateUpdate()
	{
		trueVelocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
		//gravityAcc = 0f;
	}

	// Update is called once per frame
	void Update () {
		rb.velocity = rb.velocity * Time.timeScale;
		rbFix = false;
		// For curious maids
		float time = Time.deltaTime * TimeAffect;

		//RaycastHit col;
		//if (Physics.Raycast(ray.origin, ray.direction, out col))
		//	Debug.DrawLine(ray.origin, col.point, Color.red);
		//else
		//	Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
		Debug.DrawRay(transform.position, transform.forward, Color.blue);
        if (isMoving || Mathf.Abs(Input.GetAxis(RX)) != 0)
		{
			float angle = Vector3.SignedAngle(playerModel.forward, characterDirection, Vector3.up);
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

		jumpIF = Mathf.Max(jumpIF - Time.deltaTime / Time.timeScale, 0f);
		RaycastHit ray;
		var grounded = Physics.Raycast(transform.position, -transform.up, out ray, RaycastDownRange);
		Debug.DrawRay(transform.position, -transform.up * RaycastDownRange, Color.yellow);
		if (!grounded || (!ray.collider.gameObject.tag.Equals("Wall") && !ray.collider.gameObject.tag.Equals("Switch")))
		{
			if (rb.velocity.y > 0f)
			{
				animator.SetBool("isJumping", true);
				animator.SetBool("isFalling", false);
				isJumping = true;
				isFalling = false;
			}
			else
			{
				animator.SetBool("isJumping", false);
				animator.SetBool("isFalling", true);
				isJumping = false;
				isFalling = true;
			}
		}
		else if ((ray.collider.gameObject.tag.Equals("Wall") || ray.collider.gameObject.tag.Equals("Switch")) && jumpIF <= 0f)
		{
			animator.SetBool("isJumping", false);
			animator.SetBool("isFalling", false);
			isJumping = false;
			isFalling = false;
			JumpImpulse = Vector3.zero;
		}

		if (isFalling || isJumping)
			rb.velocity -= MoreGravity * Time.deltaTime * transform.up;

		if (Input.GetButtonDown(jump) && !isJumping && !isFalling)
		{
			if (TimeAffect != 1f)
				JumpImpulse = transform.up * JumpForce * TimeAffect / AlteredTimeJumpMagicFactor;
			else
				rb.AddForce(transform.up * JumpForce * TimeAffect, ForceMode.Impulse);

			jumpIF = JumpNoCheck;
			//animator.SetBool("isJumping", true);
		}

		var cameraRotation = cameraTransform.rotation;
		if (cs.Ready)
		{
			cameraTransform.Rotate(Input.GetAxis(RY) * Vector3.right * time * CameraSpeed, Space.Self);
		if (Mathf.Abs(Mathf.Sin(cameraTransform.eulerAngles.x * Mathf.Deg2Rad)) > CameraMaxVerticalAngleSin )
			cameraTransform.rotation = cameraRotation;
		else
			transform.Rotate(Input.GetAxis(RX) * Vector3.up * time * CameraSpeed, Space.Self);

		}


	}
      
}
