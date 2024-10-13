using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FP_Input))]
[RequireComponent(typeof(FP_CameraLook))]
[RequireComponent(typeof(FP_FootSteps))]
public class FP_Controller : MonoBehaviour
{
	public bool canControl = true;

	public float gravity = 20f;

	public float walkSpeed = 6f;

	public float runSpeed = 11f;

	public float jumpForce = 8f;

	public float crouchSpeed = 2f;

	public float crouchHeight = 1f;

	[HideInInspector]
	public float velocidadCaminar_Inicial;

	[HideInInspector]
	public float velocidadAgachado_Inicial;

	[HideInInspector]
	public float bobFrequency_Inicial;

	[HideInInspector]
	public float bobHeight_Inicial;

	[HideInInspector]
	public float bobSwayAngle_Inicial;

	public KeyCode crouchKey = KeyCode.LeftControl;

	public KeyCode runKey = KeyCode.LeftShift;

	public KeyCode jumpKey = KeyCode.Space;

	public bool airControl = true;

	public bool canCrouch = true;

	public bool canJump = true;

	public bool canRun = true;

	[HideInInspector]
	public float inputX;

	[HideInInspector]
	public float inputZ;

	[HideInInspector]
	public bool crouch;

	[HideInInspector]
	public CharacterController controller;

	private Vector3 moveDirection;

	private Vector3 contactPoint;

	private Vector3 hitNormal;

	private AudioSource JumpLandSource;

	private FP_FootSteps footSteps;

	private Transform myTransform;

	private FP_Input playerInput;

	private RaycastHit hit;

	private bool playerControl;

	private bool isCrouching;

	private bool grounded;

	private bool sliding;

	private bool jump;

	private bool run;

	private int antiBunnyHopFactor = 1;

	private int jumpTimer;

	private int landTimer;

	private int jumpState;

	private int runState;

	private float antiBumpFactor = 0.75f;

	private float inputModifyFactor;

	private float slideSpeed = 2f;

	private float minCrouchHeight;

	private float fallStartLevel;

	private float defaultHeight;

	private float rayDistance;

	private float slideLimit;

	private float speed;

	private string surfaceTag;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
		playerInput = GetComponent<FP_Input>();
		footSteps = GetComponent<FP_FootSteps>();
		velocidadCaminar_Inicial = walkSpeed;
		velocidadAgachado_Inicial = crouchSpeed;
		bobFrequency_Inicial = GetComponent<FP_CameraLook>().headBob.BobFrequency;
		bobHeight_Inicial = GetComponent<FP_CameraLook>().headBob.BobHeight;
		bobSwayAngle_Inicial = GetComponent<FP_CameraLook>().headBob.BobSwayAngle;
	}

	private void Start()
	{
		defaultHeight = controller.height;
		minCrouchHeight = ((crouchHeight > controller.radius * 2f) ? crouchHeight : (controller.radius * 2f));
		myTransform = base.transform;
		speed = walkSpeed;
		rayDistance = controller.height * 0.5f + controller.radius;
		slideLimit = controller.slopeLimit - 0.1f;
		jumpTimer = antiBunnyHopFactor;
		JumpLandSource = base.gameObject.AddComponent<AudioSource>();
	}

	private void OnEnable()
	{
		inputX = 0f;
		inputZ = 0f;
		moveDirection = default(Vector3);
	}

	private void FixedUpdate()
	{
		inputModifyFactor = ((inputX.Adaptar() != 0f && inputZ.Adaptar() != 0f) ? 0.7071f : 1f);
		if (grounded)
		{
			sliding = false;
			if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
			{
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit && CanSlide())
				{
					sliding = true;
				}
			}
			else
			{
				Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit && CanSlide())
				{
					sliding = true;
				}
			}
			speed = (isCrouching ? crouchSpeed : ((!run) ? walkSpeed : (canRun ? runSpeed : walkSpeed)));
			if (sliding)
			{
				hitNormal = hit.normal;
				moveDirection = new Vector3(hitNormal.x, 0f - hitNormal.y, hitNormal.z);
				Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
				moveDirection *= slideSpeed;
				playerControl = false;
			}
			else
			{
				moveDirection = new Vector3(inputX.Adaptar() * inputModifyFactor, 0f - antiBumpFactor, inputZ.Adaptar() * inputModifyFactor);
				moveDirection = myTransform.TransformDirection(moveDirection) * speed;
				playerControl = true;
			}
		}
		if (canControl)
		{
			moveDirection.y -= gravity * Time.deltaTime;
			grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
		}
	}

	private void Update()
	{
		if (!canControl)
		{
			return;
		}
		switch (playerInput.UseMobileInput)
		{
		case true:
			runState = ((playerInput.Run() && canRun && !isCrouching) ? 1 : 0);
			inputX = playerInput.MoveInput().x;
			inputZ = playerInput.MoveInput().z + (float)runState;
			break;
		case false:
		{
			float num = 10f;
			inputX = Mathf.Lerp(inputX, Input.GetAxisRaw("Horizontal"), num * Time.deltaTime);
			inputZ = Mathf.Lerp(inputZ, Input.GetAxisRaw("Vertical"), num * Time.deltaTime);
			if (Input.GetKeyDown(crouchKey))
			{
				crouch = !crouch;
			}
			break;
		}
		}
		if (Mathf.Abs((base.transform.position - contactPoint).magnitude) > 2f)
		{
			landTimer = 1;
		}
		isCrouching = crouch && canCrouch;
		if (grounded)
		{
			if (isCrouching)
			{
				controller.center = Vector3.Lerp(controller.center, new Vector3(controller.center.x, (0f - (defaultHeight - minCrouchHeight)) / 2f, controller.center.z), 7f * Time.deltaTime);
				controller.height = Mathf.Lerp(controller.height, minCrouchHeight, 7f * Time.deltaTime);
			}
			else if (CanStand())
			{
				controller.center = Vector3.Lerp(controller.center, Vector3.zero, 7f * Time.deltaTime);
				controller.height = Mathf.Lerp(controller.height, defaultHeight, 7f * Time.deltaTime);
			}
		}
	}

	public void Cancel_Crouch()
	{
		crouch = false;
		controller.center = Vector3.zero;
		controller.height = defaultHeight;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!IsGrounded() && landTimer == 1)
		{
			PlaySound(footSteps.landSound, JumpLandSource);
		}
		landTimer = 0;
		jumpState = 0;
		contactPoint = hit.point;
		surfaceTag = hit.collider.tag;
	}

	private void PlaySound(AudioClip audio, AudioSource source)
	{
		source.clip = audio;
		if ((bool)audio)
		{
			source.Play();
		}
	}

	public bool IsGrounded()
	{
		return grounded;
	}

	public bool IsCrouching()
	{
		return crouch;
	}

	public bool IsRunning()
	{
		return run;
	}

	public bool CanStand()
	{
		RaycastHit hitInfo = default(RaycastHit);
		return !Physics.Raycast(controller.bounds.center, Vector3.up, out hitInfo, controller.height / 2f + 0.5f, LayerMask.GetMask(ES_Tags.Estatico));
	}

	private bool CanSlide()
	{
		return new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude < walkSpeed / 2f;
	}

	public string SurfaceTag()
	{
		return surfaceTag;
	}
}
