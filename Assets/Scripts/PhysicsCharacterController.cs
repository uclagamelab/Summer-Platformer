// derived from http://www.unifycommunity.com/wiki/index.php?title=PhysicsFPSWalker

using UnityEngine;
using System.Collections;

public class PhysicsCharacterController : MonoBehaviour {
	
	public Camera mainCamera;
	
	private Animation animation;
	
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public AnimationClip jumpPoseAnimation;
	public AnimationClip deathAnimation;
	
	public float walkAnimationSpeed = 1.0f;
	public float walkMaxAnimationSpeed  = 0.75f;
	public float runMaxAnimationSpeed = 1.0f;
	public float jumpAnimationSpeed = 1.15f;
	public float landAnimationSpeed  = 1.0f;

	// These variables are for adjusting in the inspector how the object behaves
	public float maxWalkSpeed = 6.0f;
	public float maxRunSpeed  = 7.0f;
	public float walkForce     = 8.0f;
	public float runForce = 10.0f;
	public bool allowRunning = false;
	public float jumpSpeed = 6.0f;
 
	// These variables are there for use by the script and don't need to be edited
	public enum CharacterState {
		Idle = 0,
		Walking = 1,
		Running = 2,
		Jumping = 3,
		Dead = 4
	}
	public CharacterState characterState = 0;
	public bool canJump = false;
	public bool grounded = false;
	public float jumpLimit = 0;
	
	private int groundCounter = 0;
	private bool isMoving = false;
	private Vector3 originalOrientation = Vector3.zero;
	private Vector3 charOrientation = Vector3.zero;
	[HideInInspector]
	public bool isControllable = true; // shut down controls if necesary
	private Vector3 addForce = Vector3.zero;
 
	void Awake ()
	{
			// Don't let the Physics Engine rotate this physics object so it doesn't fall over when running

		rigidbody.freezeRotation = true;
		charOrientation = transform.TransformDirection(Vector3.forward);
		originalOrientation = transform.TransformDirection(Vector3.forward);
		
		// TODO: set up main camera again when level loads
		
		animation = (Animation) GetComponent("Animation");
		if(!animation) Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		if(!idleAnimation) {
			animation = null;
			Debug.Log("No idle animation found. Turning off animations.");
		}
		if(!walkAnimation) {
			animation = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if(!runAnimation) {
			animation = null;
			Debug.Log("No run animation found. Turning off animations.");
		}
		if(!jumpPoseAnimation && canJump) {
			animation = null;
			Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
		}
		if(!deathAnimation) {
			animation = null;
			Debug.Log("No death animation found and the character has canJump enabled. Turning off animations.");
		}
		//GameObject cam = GameObject.Find("MainCamera");
		//mainCamera = (Camera) cam.GetComponent("Camera") as Camera;
	}
	
	void OnLevelWasLoaded() {
		Debug.Log("Player level load");
		GameObject cam = GameObject.Find("Main Camera");
		if (cam == null) {
			Debug.LogError(gameObject.name + ": PhysicsCharacterController: can't find 'Main Camera' in scene. Quitting.");
			Application.Quit();
		}
		mainCamera = (Camera) cam.GetComponent("Camera");
	}
 
	// This part detects whether or not the object is grounded and stores it in a variable
	void OnCollisionEnter ()
	{
		groundCounter ++;
		if(groundCounter > 0)
		{
			grounded = true;
		}
	}
 
	void OnCollisionExit ()
	{
		groundCounter --;
		if(groundCounter < 1)
		{
			grounded = false;
			characterState = CharacterState.Jumping;
		}
	}

	public virtual bool jump
	{
		get
		{
			return Input.GetButtonDown ("Jump");
		}
	}
 
	public virtual float horizontal
	{
		get
		{
			//float force = characterState == CharacterState.Running ? runForce : walkForce;
			return Input.GetAxis("Horizontal");
		}
	}
	
	// ignore vertical for now
	/*public virtual float vertical
	{
		get
		{
		
			return Input.GetAxis("Vertical") * force;
		}
	}*/
	void UpdateSmoothedMovementDirection() {
	{
		Transform cameraTransform = mainCamera.transform;

		//float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		// Are we moving backwards or looking backwards
		/*if (v < -0.2)
			movingBack = true;
		else
			movingBack = false;*/
	
		bool wasMoving = isMoving;
		isMoving = rigidbody.velocity.magnitude > 0.1 ;
		
		// Target direction relative to the camera
		//var targetDirection = h * right + v * forward;
		//Vector3 targetDirection = h*cameraRight;//*charOrientation; //right;
		if (h != 0.0) charOrientation = h > 0.0 ? cameraTransform.TransformDirection(-1.0f*originalOrientation) : cameraTransform.TransformDirection(originalOrientation);
	
		// Grounded controls
		if (grounded)
		{
			// Lock camera for short period when transitioning moving & standing still
			//lockCameraTimer += Time.deltaTime;
			//if (isMoving != wasMoving) lockCameraTimer = 0.0;
			/*
			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				// If we are really slow, just snap to the target direction
				if (moveSpeed < walkSpeed * 0.9 && grounded)
				{
					moveDirection = targetDirection.normalized;
				}
				// Otherwise smoothly turn towards it
				else
				{
					//moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					moveDirection = targetDirection;//, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);

					moveDirection = moveDirection.normalized;
				}
			}*/
		
			// Smooth the speed based on the current target direction
			//float curSmooth = speedSmoothing * Time.deltaTime;
		
			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			/*float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0);
	
			characterState = CharacterState.Idle;
		
			// Pick speed modifier
			if (Input.GetKey (KeyCode.LeftShift) | Input.GetKey (KeyCode.RightShift))
			{
				targetSpeed *= runSpeed;
				characterState = CharacterState.Running;
			}
			else if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
				characterState = CharacterState.Trotting;
			}
			else
			{
				targetSpeed *= walkSpeed;
				characterState = CharacterState.Walking;
			}
		
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		
			// Reset walk time start when we slow down
			if (moveSpeed < walkSpeed * 0.3)
				walkTimeStart = Time.time;*/
			
			//	determine character state
			characterState = CharacterState.Walking;
			if (rigidbody.velocity.magnitude > maxWalkSpeed && allowRunning) { 
				characterState = CharacterState.Running;
			}
			else if (rigidbody.velocity.sqrMagnitude < 0.01f) {
				characterState = CharacterState.Idle;
			}
				
		}
		// In air controls
		/*else
		{
			// Lock camera while in air
			if (jumping)
				lockCameraTimer = 0.0;

			if (isMoving) {
				inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
			}
		}*/
	}		

	}

	// This is called every physics frame
	void FixedUpdate ()
	{
		if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
			return;
		}
		
		/*if (Input.GetButtonDown ("Jump"))
		{
			lastJumpButtonTime = Time.time;
		}*/

		UpdateSmoothedMovementDirection();
		Vector3 lookTarget = new Vector3(charOrientation.x+transform.position.x, transform.position.y, transform.position.z + charOrientation.z);
		transform.LookAt(lookTarget);
 
		// If the object is grounded and isn't moving at the max speed or higher apply force to move it
		if (characterState == CharacterState.Running && rigidbody.velocity.magnitude < maxRunSpeed && grounded == true) {
			addForce = transform.rotation * Vector3.forward * Mathf.Abs(horizontal) ;
			rigidbody.AddForce(addForce*runForce);
		}
		else if(jump && grounded  && jumpLimit >= 10)
		{
			rigidbody.velocity = rigidbody.velocity + (Vector3.up * jumpSpeed) + (transform.rotation *Vector3.forward *Mathf.Abs(horizontal));
			jumpLimit = 0;
			characterState = CharacterState.Jumping;
		}
		else if(Mathf.Abs(horizontal) > 0.0f && rigidbody.velocity.magnitude < maxWalkSpeed && grounded == true)
		{
			//rigidbody.AddForce (transform.rotation * Vector3.forward * vertical);
			//rigidbody.AddForce (transform.rotation * Vector3.right * -horizontal);
			addForce = transform.rotation * Vector3.forward * Mathf.Abs(horizontal) ;
			addForce.y = 0.1f;
			rigidbody.AddForce(addForce*walkForce);
		}
 
		// This part is for jumping. I only let jump force be applied every 10 physics frames so
		// the player can't somehow get a huge velocity due to multiple jumps in a very short time
		if(jumpLimit < 10) jumpLimit ++;
 
		//~ if(jump && grounded  && jumpLimit >= 10)
		//~ {
			//~ rigidbody.velocity = rigidbody.velocity + (Vector3.up * jumpSpeed);
			//~ jumpLimit = 0;
			//~ characterState = CharacterState.Jumping;
		//~ }
		
		
		
		// ANIMATION sector
		if(animation) {
			if(characterState == CharacterState.Jumping) 
			{
				//if(!jumpingReachedApex) {
				if (rigidbody.velocity.y > 0.0f) {
					animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
					animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					animation.CrossFade(jumpPoseAnimation.name);
				} else {
					animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
					animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					animation.CrossFade(jumpPoseAnimation.name);				
				}
				animation.CrossFade(jumpPoseAnimation.name);
			} 
			else 
			{
				//if(rigidbody.velocity.sqrMagnitude < 0.01) {
				if (characterState == CharacterState.Idle) {
					animation.CrossFade(idleAnimation.name);
				}
				else 
				{
					if(characterState == CharacterState.Running) {
						animation[runAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
						animation.CrossFade(runAnimation.name);	
					}
					/*else if(characterState == CharacterState.Trotting) {
						animation[walkAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude, 0.0f, trotMaxAnimationSpeed);
						animation.CrossFade(walkAnimation.name);	
					}*/
					else if(characterState == CharacterState.Walking) {
						animation[walkAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude*walkAnimationSpeed, 0.0f, walkMaxAnimationSpeed);
						animation.CrossFade(walkAnimation.name);	
					}
					else if(characterState == CharacterState.Dead) {
						animation.CrossFade(deathAnimation.name);
					}
				}
			}
		}
	// ANIMATION sector
	}
	
	public void PlayDeathAnimation() {
		animation.Play(deathAnimation.name);
		Debug.Log("Play death animation");
	}
	
	public bool DeathAnimationFinished() {
		if (animation.IsPlaying(deathAnimation.name)) {
			return false;
		}
		return true;
	}
}