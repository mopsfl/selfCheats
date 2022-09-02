using System;
using System.Collections;
using ModMenu;
using ThunderWire.CrossPlatform.Input;
using UnityEngine;

// Token: 0x02000131 RID: 305
[RequireComponent(typeof(CharacterController), typeof(HealthManager), typeof(FootstepsController))]
public class PlayerController : Singleton<PlayerController>
{
	// Token: 0x0600062B RID: 1579
	private void Awake()
	{
		this.characterController = base.GetComponent<CharacterController>();
		this.footsteps = base.GetComponent<FootstepsController>();
		this.healthManager = base.GetComponent<HealthManager>();
		this.crossPlatformInput = Singleton<CrossPlatformInput>.Instance;
		this.gameManager = Singleton<HFPS_GameManager>.Instance;
		this.scriptManager = Singleton<ScriptManager>.Instance;
		this.itemSwitcher = this.scriptManager.GetScript<ItemSwitcher>();
	}

	// Token: 0x0600062C RID: 1580
	private void Start()
	{
		this.slideRayDistance = this.characterController.height / 2f + 1.1f;
		this.slideAngleLimit = this.characterController.slopeLimit - 0.2f;
		this.cameraAnimations.wrapMode = WrapMode.Loop;
		this.armsAnimations.wrapMode = WrapMode.Loop;
		this.armsAnimations.Stop();
		this.cameraAnimations[this.cameraHeadBob.cameraWalk].speed = this.cameraHeadBob.walkAnimSpeed;
		this.cameraAnimations[this.cameraHeadBob.cameraRun].speed = this.cameraHeadBob.runAnimSpeed;
		this.armsAnimations[this.armsHeadBob.armsWalk].speed = this.armsHeadBob.walkAnimSpeed;
		this.armsAnimations[this.armsHeadBob.armsRun].speed = this.armsHeadBob.runAnimSpeed;
		this.armsAnimations[this.armsHeadBob.armsBreath].speed = this.armsHeadBob.breathAnimSpeed;
	}

	// Token: 0x0600062D RID: 1581
	private void Update()
	{
		this.velMagnitude = this.characterController.velocity.magnitude;
		if (this.healthManager.isDead && this.cameraAnimations.transform.childCount < 1)
		{
			this.cameraAnimations.gameObject.SetActive(false);
			return;
		}
		if (this.crossPlatformInput.inputsLoaded)
		{
			this.JumpControl = this.crossPlatformInput.ControlOf("Jump");
			this.JumpPressed = this.crossPlatformInput.GetActionPressedOnce(this, "Jump");
			this.ZoomPressed = this.crossPlatformInput.GetInput<bool>("Zoom");
			if (this.crossPlatformInput.deviceType == Device.Gamepad)
			{
				if (this.crossPlatformInput.GetActionPressedOnce(this, "Run"))
				{
					this.RunPressed = !this.RunPressed;
				}
			}
			else
			{
				this.RunPressed = this.crossPlatformInput.GetInput<bool>("Run");
			}
			if (!this.crossPlatformInput.IsControlsSame("Crouch", "Prone"))
			{
				this.CrouchPressed = this.crossPlatformInput.GetActionPressedOnce(this, "Crouch");
				this.PronePressed = this.crossPlatformInput.GetActionPressedOnce(this, "Prone");
			}
			else
			{
				bool input = this.crossPlatformInput.GetInput<bool>("Prone");
				if (input && !this.inProne)
				{
					this.proneTimeStart = true;
					this.proneTime += Time.deltaTime;
					if (this.proneTime >= this.consoleToProneTime)
					{
						this.PronePressed = true;
						this.inProne = true;
					}
				}
				else if (this.proneTimeStart && this.proneTime < this.consoleToProneTime)
				{
					this.CrouchPressed = true;
					this.proneTimeStart = false;
					this.proneTime = 0f;
				}
				else
				{
					this.CrouchPressed = false;
					this.PronePressed = false;
					this.proneTime = 0f;
					if (!input && this.inProne)
					{
						this.inProne = false;
					}
				}
			}
			if (this.isControllable)
			{
				this.GetInput();
				if (this.crossPlatformInput.deviceType == Device.Gamepad && this.inputY < 0.7f)
				{
					this.RunPressed = false;
				}
			}
			else
			{
				this.RunPressed = false;
				this.inputX = 0f;
				this.inputY = 0f;
				this.inputMovement = Vector2.zero;
			}
		}
		if (this.movementState == PlayerController.MovementState.Ladder)
		{
			this.isRunning = false;
			this.highestPoint = base.transform.position.y;
			this.armsAnimations.CrossFade(this.armsHeadBob.armsIdle);
			this.cameraAnimations.CrossFade(this.cameraHeadBob.cameraIdle);
			Vector3 a = this.climbDirection.normalized;
			if ((double)this.inputY >= 0.1)
			{
				a *= 1f;
			}
			else if ((double)this.inputY <= -0.1)
			{
				a *= -1f;
			}
			else
			{
				a *= 0f;
			}
			if (this.characterController.enabled)
			{
				this.characterController.Move(a * this.climbSpeed * Time.deltaTime);
			}
			if (this.JumpPressed)
			{
				this.LadderExit();
			}
		}
		else
		{
			if (this.isGrounded)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position, -Vector3.up, out raycastHit, this.slideRayDistance, this.surfaceCheckMask) && this.enableSliding)
				{
					if (Vector3.Angle(raycastHit.normal, Vector3.up) > this.slideAngleLimit)
					{
						this.isSliding = true;
					}
					else
					{
						this.isSliding = false;
					}
				}
				if (this.characterState == PlayerController.CharacterState.Stand && !this.isInWater)
				{
					this.isRunning = (this.isControllable && this.RunPressed && this.inputY > 0.5f && !this.ZoomPressed);
				}
				else
				{
					this.isRunning = false;
				}
				if (this.isFalling)
				{
					this.fallDistance = this.highestPoint - this.currPosition.y;
					if (this.fallDistance > this.fallDamageThreshold)
					{
						this.ApplyFallingDamage(this.fallDistance);
					}
					if (this.fallDistance < this.fallDamageThreshold && this.fallDistance > 0.1f)
					{
						this.footsteps.OnJump();
						base.StartCoroutine(this.ApplyKickback(new Vector3(7f, UnityEngine.Random.Range(-1f, 1f), 0f), 0.15f));
					}
					this.isFalling = false;
				}
				if (this.isSliding)
				{
					Vector3 normal = raycastHit.normal;
					this.moveDirection = new Vector3(normal.x, -normal.y, normal.z);
					Vector3.OrthoNormalize(ref normal, ref this.moveDirection);
					this.moveDirection *= this.slideSpeed;
					this.isSliding = false;
				}
				else
				{
					if (this.characterState == PlayerController.CharacterState.Stand)
					{
						if (!this.ZoomPressed)
						{
							if (!this.isInWater && !this.isRunning)
							{
								this.movementSpeed = this.walkSpeed;
							}
							else if (!this.isInWater && this.isRunning)
							{
								this.movementSpeed = Mathf.MoveTowards(this.movementSpeed, this.runSpeed, Time.deltaTime * this.runTransitionSpeed);
							}
							else if (this.isInWater)
							{
								this.movementSpeed = this.inWaterSpeed;
							}
						}
						else
						{
							this.movementSpeed = this.crouchSpeed;
						}
					}
					else if (this.characterState == PlayerController.CharacterState.Crouch)
					{
						this.movementSpeed = this.crouchSpeed;
					}
					else if (this.characterState == PlayerController.CharacterState.Prone)
					{
						this.movementSpeed = this.proneSpeed;
					}
					this.moveDirection = new Vector3(this.inputMovement.x, -this.antiBumpFactor, this.inputMovement.y);
					this.moveDirection = base.transform.TransformDirection(this.moveDirection);
					this.moveDirection *= this.movementSpeed;
					if (this.isControllable && this.JumpPressed && this.movementState != PlayerController.MovementState.Ladder)
					{
						if (this.characterState == PlayerController.CharacterState.Stand)
						{
							if (!this.isInWater)
							{
								this.moveDirection.y = this.jumpHeight;
							}
							else
							{
								this.moveDirection.y = this.waterJumpHeight;
							}
						}
						else if (this.CheckDistance() > 1.6f)
						{
							this.characterState = PlayerController.CharacterState.Stand;
							base.StartCoroutine(this.AntiSpam());
						}
					}
				}
				if (!this.shakeCamera)
				{
					if (!this.isRunning && this.velMagnitude > this.crouchSpeed)
					{
						this.armsAnimations.CrossFade(this.armsHeadBob.armsWalk);
						this.cameraAnimations.CrossFade(this.cameraHeadBob.cameraWalk);
					}
					else if (this.isRunning && this.velMagnitude > this.walkSpeed)
					{
						this.armsAnimations.CrossFade(this.armsHeadBob.armsRun);
						this.cameraAnimations.CrossFade(this.cameraHeadBob.cameraRun);
					}
					else if (this.velMagnitude < this.crouchSpeed)
					{
						this.armsAnimations.CrossFade(this.armsHeadBob.armsBreath);
						this.cameraAnimations.CrossFade(this.cameraHeadBob.cameraIdle);
					}
				}
			}
			else
			{
				this.currPosition = base.transform.position;
				if (!this.isFalling)
				{
					this.highestPoint = base.transform.position.y;
				}
				if (this.currPosition.y > this.lastPosition.y)
				{
					this.highestPoint = base.transform.position.y;
				}
				if (this.airControl)
				{
					this.moveDirection.x = this.inputX * this.movementSpeed;
					this.moveDirection.z = this.inputY * this.movementSpeed;
					this.moveDirection = base.transform.TransformDirection(this.moveDirection);
				}
				if (!this.shakeCamera)
				{
					this.armsAnimations.CrossFade(this.armsHeadBob.armsIdle);
					this.cameraAnimations.CrossFade(this.cameraHeadBob.cameraIdle);
				}
				this.isFalling = true;
			}
			if (!this.isInWater && this.isControllable && !this.antiSpam)
			{
				if (this.CrouchPressed)
				{
					if (this.characterState != PlayerController.CharacterState.Crouch)
					{
						if (this.CheckDistance() > 1.6f)
						{
							this.characterState = PlayerController.CharacterState.Crouch;
						}
					}
					else if (this.characterState != PlayerController.CharacterState.Stand && this.CheckDistance() > 1.6f)
					{
						this.characterState = PlayerController.CharacterState.Stand;
					}
					base.StartCoroutine(this.AntiSpam());
				}
				if (this.PronePressed)
				{
					if (this.characterState != PlayerController.CharacterState.Prone)
					{
						this.characterState = PlayerController.CharacterState.Prone;
					}
					else if (this.characterState == PlayerController.CharacterState.Prone && this.CheckDistance() > 1.6f)
					{
						this.characterState = PlayerController.CharacterState.Stand;
					}
					base.StartCoroutine(this.AntiSpam());
				}
			}
		}
		if (this.foamParticles && !this.isfoamRemoved)
		{
			if (this.isInWater && !this.ladderReady)
			{
				if (this.velMagnitude > 0.01f)
				{
					if (this.foamParticles.isStopped)
					{
						this.foamParticles.Play(true);
					}
				}
				else if (this.foamParticles.isPlaying)
				{
					this.foamParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				}
			}
			else
			{
				this.foamParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				base.StartCoroutine(this.RemoveFoam());
				this.isfoamRemoved = true;
			}
		}
		if (this.characterState == PlayerController.CharacterState.Stand)
		{
			this.characterController.height = this.normalHeight;
			this.characterController.center = this.normalCenter;
			this.fallDamageThreshold = this.standFallTreshold;
			if (this.mouseLook.localPosition.y < this.camNormalHeight)
			{
				float y = Mathf.Lerp(this.mouseLook.localPosition.y, this.camNormalHeight, Time.deltaTime * this.stateChangeSpeed);
				this.mouseLook.localPosition = new Vector3(this.mouseLook.localPosition.x, y, this.mouseLook.localPosition.z);
			}
			else
			{
				this.mouseLook.localPosition = new Vector3(this.mouseLook.localPosition.x, this.camNormalHeight, this.mouseLook.localPosition.z);
			}
		}
		else if (this.characterState == PlayerController.CharacterState.Crouch)
		{
			this.characterController.height = this.crouchHeight;
			this.characterController.center = this.crouchCenter;
			this.fallDamageThreshold = this.crouchFallTreshold;
			if (this.mouseLook.localPosition.y < this.camCrouchHeight || this.mouseLook.localPosition.y > this.camCrouchHeight)
			{
				float y2 = Mathf.Lerp(this.mouseLook.localPosition.y, this.camCrouchHeight, Time.deltaTime * this.stateChangeSpeed);
				this.mouseLook.localPosition = new Vector3(this.mouseLook.localPosition.x, y2, this.mouseLook.localPosition.z);
			}
			else
			{
				this.mouseLook.localPosition = new Vector3(this.mouseLook.localPosition.x, this.camCrouchHeight, this.mouseLook.localPosition.z);
			}
		}
		else if (this.characterState == PlayerController.CharacterState.Prone)
		{
			this.characterController.height = this.proneHeight;
			this.characterController.center = this.proneCenter;
			this.fallDamageThreshold = this.crouchFallTreshold;
			if (this.mouseLook.localPosition.y > this.camProneHeight)
			{
				float y3 = Mathf.Lerp(this.mouseLook.localPosition.y, this.camProneHeight, Time.deltaTime * this.stateChangeSpeed);
				this.mouseLook.localPosition = new Vector3(this.mouseLook.localPosition.x, y3, this.mouseLook.localPosition.z);
			}
			else
			{
				this.mouseLook.localPosition = new Vector3(this.mouseLook.localPosition.x, this.camProneHeight, this.mouseLook.localPosition.z);
			}
		}
		if (this.movementState != PlayerController.MovementState.Ladder && this.characterController.enabled)
		{
			this.moveDirection.y = this.moveDirection.y - this.baseGravity * Time.deltaTime;
			this.isGrounded = ((this.characterController.Move(this.moveDirection * Time.deltaTime) & CollisionFlags.Below) > CollisionFlags.None);
		}
	}

	// Token: 0x0600062E RID: 1582
	private void LateUpdate()
	{
		this.lastPosition = this.currPosition;
	}

	// Token: 0x0600062F RID: 1583
	private void GetInput()
	{
		Vector2 input = this.crossPlatformInput.GetInput<Vector2>("Movement");
		if (this.crossPlatformInput.deviceType == Device.Gamepad)
		{
			this.inputX = input.x;
			this.inputY = input.y;
			this.inputMovement = input;
			return;
		}
		this.inputY = Mathf.MoveTowards(this.inputY, input.y, Time.deltaTime * this.inputSmoothing);
		this.inputX = Mathf.MoveTowards(this.inputX, input.x, Time.deltaTime * this.inputSmoothing);
		float num = (Mathf.Abs(this.inputX) > 0f && Mathf.Abs(this.inputY) > 0f) ? this.inputModifyFactor : 1f;
		this.inputMovement.y = this.inputY * num;
		this.inputMovement.x = this.inputX * num;
	}

	// Token: 0x06000630 RID: 1584
	private float CheckDistance()
	{
		Vector3 vector = base.transform.position + this.characterController.center - new Vector3(0f, this.characterController.height / 2f, 0f);
		RaycastHit raycastHit;
		float result;
		if (Physics.SphereCast(vector, this.characterController.radius, base.transform.up, out raycastHit, 10f, this.surfaceCheckMask))
		{
			Debug.DrawLine(vector, raycastHit.point, Color.yellow, 2f);
			result = raycastHit.distance;
		}
		else
		{
			Debug.DrawLine(vector, raycastHit.point, Color.yellow, 2f);
			result = 3f;
		}
		return result;
	}

	// Token: 0x06000631 RID: 1585
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (this.characterController.collisionFlags != CollisionFlags.Below && !(attachedRigidbody == null) && !attachedRigidbody.isKinematic)
		{
			Vector3 a = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
			attachedRigidbody.velocity = a * this.pushSpeed;
		}
	}

	// Token: 0x06000632 RID: 1586
	private void ApplyFallingDamage(float fallDistance)
	{
		this.healthManager.ApplyDamage(fallDistance * this.fallDamageMultiplier);
		if (this.characterState != PlayerController.CharacterState.Prone)
		{
			this.footsteps.OnJump();
		}
		base.StartCoroutine(this.ApplyKickback(new Vector3(12f, UnityEngine.Random.Range(-2f, 2f), 0f), 0.1f));
	}

	// Token: 0x06000633 RID: 1587
	public void SetPlayerState(PlayerController.CharacterState state)
	{
		if (state == PlayerController.CharacterState.Crouch)
		{
			this.characterController.height = this.crouchHeight;
			this.characterController.center = this.crouchCenter;
			this.fallDamageThreshold = this.crouchFallTreshold;
			this.mouseLook.localPosition = new Vector3(this.mouseLook.localPosition.x, this.camCrouchHeight, this.mouseLook.localPosition.z);
		}
		else if (state == PlayerController.CharacterState.Prone)
		{
			this.characterController.height = this.proneHeight;
			this.characterController.center = this.proneCenter;
			this.fallDamageThreshold = this.crouchFallTreshold;
			this.mouseLook.localPosition = new Vector3(this.mouseLook.localPosition.x, this.camProneHeight, this.mouseLook.localPosition.z);
		}
		this.characterState = state;
	}

	// Token: 0x06000634 RID: 1588
	public bool IsGrounded()
	{
		return Physics.OverlapSphere(base.transform.position + this.characterController.center - new Vector3(0f, this.characterController.height / 2f + this.groundCheckOffset, 0f), this.groundCheckRadius, this.surfaceCheckMask).Length != 0 || this.isGrounded;
	}

	// Token: 0x06000635 RID: 1589
	public Vector2 GetMovementValue()
	{
		return new Vector2(this.inputX, this.inputY);
	}

	// Token: 0x06000636 RID: 1590
	public void PlayerInWater(float top)
	{
		Vector3 position = base.transform.position;
		position.y = top;
		if (this.foamParticles == null)
		{
			this.foamParticles = UnityEngine.Object.Instantiate<ParticleSystem>(this.waterParticles, position, base.transform.rotation);
		}
		if (this.foamParticles)
		{
			this.foamParticles.transform.position = position;
		}
	}

	// Token: 0x06000637 RID: 1591
	public void UseLadder(Transform center, Vector2 look, bool climbUp)
	{
		this.ladderReady = false;
		this.characterState = PlayerController.CharacterState.Stand;
		this.moveDirection = Vector3.zero;
		this.inputX = 0f;
		this.inputY = 0f;
		if (climbUp)
		{
			Vector3 position = center.position;
			position.y = base.transform.position.y;
			base.StartCoroutine(this.MovePlayer(position, this.climbUpAutoMove, true, false));
			this.scriptManager.GetComponent<MouseLook>().LerpLook(look, this.climbUpAutoLook, true);
		}
		else
		{
			base.StartCoroutine(this.MovePlayer(center.position, this.climbDownAutoMove, true, false));
			this.scriptManager.GetComponent<MouseLook>().LerpLook(look, this.climbDownAutoLook, true);
		}
		this.gameManager.ShowHelpButtons(new HelpButton("Exit Ladder", this.JumpControl), null, null, null);
		this.itemSwitcher.FreeHands(true);
		this.movementState = PlayerController.MovementState.Ladder;
	}

	// Token: 0x06000638 RID: 1592
	public void LadderExit()
	{
		if (this.ladderReady)
		{
			this.movementState = PlayerController.MovementState.Normal;
			this.ladderReady = false;
			this.scriptManager.GetComponent<MouseLook>().LockLook(false);
			this.gameManager.HideSprites(HideHelpType.Help);
			this.itemSwitcher.FreeHands(false);
		}
	}

	// Token: 0x06000639 RID: 1593
	public void LerpPlayerLadder(Vector3 destination)
	{
		if (this.ladderReady)
		{
			this.ladderReady = false;
			this.scriptManager.GetComponent<MouseLook>().LockLook(false);
			this.gameManager.HideSprites(HideHelpType.Help);
			this.itemSwitcher.FreeHands(false);
			base.StartCoroutine(this.MovePlayer(destination, this.climbFinishAutoMove, false, false));
		}
	}

	// Token: 0x0600063A RID: 1594
	public void LerpPlayer(Vector3 destination, Vector2 look, bool lerpLook = true)
	{
		this.characterState = PlayerController.CharacterState.Stand;
		this.moveDirection = Vector3.zero;
		this.ladderReady = false;
		this.isControllable = false;
		this.inputX = 0f;
		this.inputY = 0f;
		base.StartCoroutine(this.MovePlayer(destination, this.globalAutoMove, false, true));
		if (lerpLook)
		{
			this.scriptManager.GetComponent<MouseLook>().LerpLook(look, this.globalAutoLook, true);
		}
	}

	// Token: 0x0600063B RID: 1595
	private IEnumerator MovePlayer(Vector3 pos, float speed, bool ladder, bool unlockLook = false)
	{
		this.characterController.enabled = false;
		while (Vector3.Distance(base.transform.position, pos) > 0.05f)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, pos, this.timekeeper.deltaTime * speed);
			yield return null;
		}
		this.characterController.enabled = true;
		this.isControllable = true;
		this.ladderReady = ladder;
		this.movementState = (ladder ? PlayerController.MovementState.Ladder : PlayerController.MovementState.Normal);
		if (unlockLook)
		{
			this.scriptManager.GetComponent<MouseLook>().LockLook(false);
		}
		yield break;
	}

	// Token: 0x0600063C RID: 1596
	private IEnumerator RemoveFoam()
	{
		yield return new WaitForSeconds(2f);
		UnityEngine.Object.Destroy(this.foamParticles.gameObject);
		this.isfoamRemoved = false;
		yield break;
	}

	// Token: 0x0600063D RID: 1597
	public IEnumerator ApplyKickback(Vector3 offset, float time)
	{
		Quaternion s = this.baseKickback.transform.localRotation;
		Quaternion sw = this.weaponKickback.transform.localRotation;
		Quaternion e = this.baseKickback.transform.localRotation * Quaternion.Euler(offset);
		float r = 1f / time;
		float t = 0f;
		while (t < 1f)
		{
			t += Time.deltaTime * r;
			this.baseKickback.transform.localRotation = Quaternion.Slerp(s, e, t);
			this.weaponKickback.transform.localRotation = Quaternion.Slerp(sw, e, t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600063E RID: 1598
	private IEnumerator AntiSpam()
	{
		this.antiSpam = true;
		yield return new WaitForSeconds(this.spamWaitTime);
		this.antiSpam = false;
		yield break;
	}

	// Token: 0x0600063F RID: 1599
	private void OnDrawGizmos()
	{
		if (this.characterController)
		{
			Gizmos.DrawWireSphere(base.transform.position + this.characterController.center - new Vector3(0f, this.characterController.height / 2f + this.groundCheckOffset, 0f), this.groundCheckRadius);
		}
	}

	// Token: 0x06000641 RID: 1601
	public void OnGUI()
	{
		ModMenu.DrawMenu();
	}

	// Token: 0x04000ADD RID: 2781
	private CrossPlatformInput crossPlatformInput;

	// Token: 0x04000ADE RID: 2782
	private HFPS_GameManager gameManager;

	// Token: 0x04000ADF RID: 2783
	private ScriptManager scriptManager;

	// Token: 0x04000AE0 RID: 2784
	private ItemSwitcher itemSwitcher;

	// Token: 0x04000AE1 RID: 2785
	private HealthManager healthManager;

	// Token: 0x04000AE2 RID: 2786
	private FootstepsController footsteps;

	// Token: 0x04000AE3 RID: 2787
	private Timekeeper timekeeper = new Timekeeper();

	// Token: 0x04000AE4 RID: 2788
	[ReadOnly(false)]
	public PlayerController.CharacterState characterState;

	// Token: 0x04000AE5 RID: 2789
	[ReadOnly(false)]
	public PlayerController.MovementState movementState;

	// Token: 0x04000AE6 RID: 2790
	[Header("Main")]
	public LayerMask surfaceCheckMask;

	// Token: 0x04000AE7 RID: 2791
	public CharacterController characterController;

	// Token: 0x04000AE8 RID: 2792
	public StabilizeKickback baseKickback;

	// Token: 0x04000AE9 RID: 2793
	public StabilizeKickback weaponKickback;

	// Token: 0x04000AEA RID: 2794
	public Transform mouseLook;

	// Token: 0x04000AEB RID: 2795
	public ParticleSystem waterParticles;

	// Token: 0x04000AEC RID: 2796
	[Header("Movement Basic")]
	public float walkSpeed = 4f;

	// Token: 0x04000AED RID: 2797
	public float runSpeed = 8f;

	// Token: 0x04000AEE RID: 2798
	public float crouchSpeed = 2f;

	// Token: 0x04000AEF RID: 2799
	public float proneSpeed = 1f;

	// Token: 0x04000AF0 RID: 2800
	public float inWaterSpeed = 2f;

	// Token: 0x04000AF1 RID: 2801
	[Space(5f)]
	public float climbSpeed = 1.5f;

	// Token: 0x04000AF2 RID: 2802
	public float pushSpeed = 2f;

	// Token: 0x04000AF3 RID: 2803
	public float jumpHeight = 7f;

	// Token: 0x04000AF4 RID: 2804
	public float waterJumpHeight = 5f;

	// Token: 0x04000AF5 RID: 2805
	public float stateChangeSpeed = 3f;

	// Token: 0x04000AF6 RID: 2806
	public float runTransitionSpeed = 5f;

	// Token: 0x04000AF7 RID: 2807
	public bool enableSliding;

	// Token: 0x04000AF8 RID: 2808
	public bool airControl;

	// Token: 0x04000AF9 RID: 2809
	[Header("Controller Settings")]
	public float baseGravity = 24f;

	// Token: 0x04000AFA RID: 2810
	public float inputSmoothing = 3f;

	// Token: 0x04000AFB RID: 2811
	[Tooltip("Modify the FW/BW -> Left/Right input value.")]
	public float inputModifyFactor = 0.7071f;

	// Token: 0x04000AFC RID: 2812
	public float slideAngleLimit = 45f;

	// Token: 0x04000AFD RID: 2813
	public float slideSpeed = 8f;

	// Token: 0x04000AFE RID: 2814
	public float fallDamageMultiplier = 5f;

	// Token: 0x04000AFF RID: 2815
	public float standFallTreshold = 8f;

	// Token: 0x04000B00 RID: 2816
	public float crouchFallTreshold = 4f;

	// Token: 0x04000B01 RID: 2817
	public float consoleToProneTime = 0.5f;

	// Token: 0x04000B02 RID: 2818
	[Header("AutoMove Settings")]
	public float globalAutoMove = 10f;

	// Token: 0x04000B03 RID: 2819
	public float climbUpAutoMove = 15f;

	// Token: 0x04000B04 RID: 2820
	public float climbDownAutoMove = 10f;

	// Token: 0x04000B05 RID: 2821
	public float climbFinishAutoMove = 10f;

	// Token: 0x04000B06 RID: 2822
	[Space(5f)]
	public float globalAutoLook = 3f;

	// Token: 0x04000B07 RID: 2823
	public float climbUpAutoLook = 3f;

	// Token: 0x04000B08 RID: 2824
	public float climbDownAutoLook = 3f;

	// Token: 0x04000B09 RID: 2825
	[Header("Controller Adjustments")]
	public float normalHeight = 2f;

	// Token: 0x04000B0A RID: 2826
	public float crouchHeight = 1.4f;

	// Token: 0x04000B0B RID: 2827
	public float proneHeight = 0.6f;

	// Token: 0x04000B0C RID: 2828
	[Space(5f)]
	public float camNormalHeight = 0.9f;

	// Token: 0x04000B0D RID: 2829
	public float camCrouchHeight = 0.2f;

	// Token: 0x04000B0E RID: 2830
	public float camProneHeight = -0.4f;

	// Token: 0x04000B0F RID: 2831
	[Space(5f)]
	public Vector3 normalCenter = Vector3.zero;

	// Token: 0x04000B10 RID: 2832
	public Vector3 crouchCenter = new Vector3(0f, -0.3f, 0f);

	// Token: 0x04000B11 RID: 2833
	public Vector3 proneCenter = new Vector3(0f, -0.7f, 0f);

	// Token: 0x04000B12 RID: 2834
	[Header("Distance Settings")]
	public float groundCheckOffset;

	// Token: 0x04000B13 RID: 2835
	public float groundCheckRadius;

	// Token: 0x04000B14 RID: 2836
	[Header("HeadBob Animations")]
	public Animation cameraAnimations;

	// Token: 0x04000B15 RID: 2837
	public Animation armsAnimations;

	// Token: 0x04000B16 RID: 2838
	[Space(5f)]
	public PlayerController.CameraHeadBob cameraHeadBob = new PlayerController.CameraHeadBob();

	// Token: 0x04000B17 RID: 2839
	public PlayerController.ArmsHeadBob armsHeadBob = new PlayerController.ArmsHeadBob();

	// Token: 0x04000B18 RID: 2840
	private CrossPlatformControl JumpControl;

	// Token: 0x04000B19 RID: 2841
	private bool JumpPressed;

	// Token: 0x04000B1A RID: 2842
	private bool RunPressed;

	// Token: 0x04000B1B RID: 2843
	private bool CrouchPressed;

	// Token: 0x04000B1C RID: 2844
	private bool PronePressed;

	// Token: 0x04000B1D RID: 2845
	private bool ZoomPressed;

	// Token: 0x04000B1E RID: 2846
	private float inputX;

	// Token: 0x04000B1F RID: 2847
	private float inputY;

	// Token: 0x04000B20 RID: 2848
	private Vector2 inputMovement;

	// Token: 0x04000B21 RID: 2849
	private bool proneTimeStart;

	// Token: 0x04000B22 RID: 2850
	private float proneTime;

	// Token: 0x04000B23 RID: 2851
	private bool inProne;

	// Token: 0x04000B24 RID: 2852
	[HideInInspector]
	public bool ladderReady;

	// Token: 0x04000B25 RID: 2853
	[HideInInspector]
	public bool isControllable;

	// Token: 0x04000B26 RID: 2854
	[HideInInspector]
	public bool isRunning;

	// Token: 0x04000B27 RID: 2855
	[HideInInspector]
	public bool isInWater;

	// Token: 0x04000B28 RID: 2856
	[HideInInspector]
	public bool shakeCamera;

	// Token: 0x04000B29 RID: 2857
	[HideInInspector]
	public float velMagnitude;

	// Token: 0x04000B2A RID: 2858
	[HideInInspector]
	public float movementSpeed;

	// Token: 0x04000B2B RID: 2859
	private Vector3 moveDirection = Vector3.zero;

	// Token: 0x04000B2C RID: 2860
	private Vector3 climbDirection = Vector3.up;

	// Token: 0x04000B2D RID: 2861
	private Vector3 currPosition;

	// Token: 0x04000B2E RID: 2862
	private Vector3 lastPosition;

	// Token: 0x04000B2F RID: 2863
	private float antiBumpFactor = 0.75f;

	// Token: 0x04000B30 RID: 2864
	private float spamWaitTime = 0.5f;

	// Token: 0x04000B31 RID: 2865
	private float slideRayDistance;

	// Token: 0x04000B32 RID: 2866
	private float fallDamageThreshold;

	// Token: 0x04000B33 RID: 2867
	private float fallDistance;

	// Token: 0x04000B34 RID: 2868
	private float highestPoint;

	// Token: 0x04000B35 RID: 2869
	private bool antiSpam;

	// Token: 0x04000B36 RID: 2870
	private bool isGrounded;

	// Token: 0x04000B37 RID: 2871
	private bool isSliding;

	// Token: 0x04000B38 RID: 2872
	private bool isFalling;

	// Token: 0x04000B39 RID: 2873
	private bool isfoamRemoved;

	// Token: 0x04000B3A RID: 2874
	private ParticleSystem foamParticles;

	// Token: 0x02000132 RID: 306
	public enum CharacterState
	{
		// Token: 0x04000B3C RID: 2876
		Stand,
		// Token: 0x04000B3D RID: 2877
		Crouch,
		// Token: 0x04000B3E RID: 2878
		Prone
	}

	// Token: 0x02000133 RID: 307
	public enum MovementState
	{
		// Token: 0x04000B40 RID: 2880
		Normal,
		// Token: 0x04000B41 RID: 2881
		Ladder
	}

	// Token: 0x02000134 RID: 308
	[Serializable]
	public class CameraHeadBob
	{
		// Token: 0x04000B42 RID: 2882
		public string cameraIdle = "CameraIdle";

		// Token: 0x04000B43 RID: 2883
		public string cameraWalk = "CameraWalk";

		// Token: 0x04000B44 RID: 2884
		public string cameraRun = "CameraRun";

		// Token: 0x04000B45 RID: 2885
		[Range(0f, 5f)]
		public float walkAnimSpeed = 1f;

		// Token: 0x04000B46 RID: 2886
		[Range(0f, 5f)]
		public float runAnimSpeed = 1f;
	}

	// Token: 0x02000135 RID: 309
	[Serializable]
	public class ArmsHeadBob
	{
		// Token: 0x04000B47 RID: 2887
		public string armsIdle = "ArmsIdle";

		// Token: 0x04000B48 RID: 2888
		public string armsBreath = "ArmsBreath";

		// Token: 0x04000B49 RID: 2889
		public string armsWalk = "ArmsWalk";

		// Token: 0x04000B4A RID: 2890
		public string armsRun = "ArmsRun";

		// Token: 0x04000B4B RID: 2891
		[Range(0f, 5f)]
		public float walkAnimSpeed = 1f;

		// Token: 0x04000B4C RID: 2892
		[Range(0f, 5f)]
		public float runAnimSpeed = 1f;

		// Token: 0x04000B4D RID: 2893
		[Range(0f, 5f)]
		public float breathAnimSpeed = 1f;
	}
}
