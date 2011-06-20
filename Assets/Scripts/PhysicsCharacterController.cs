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
	public AnimationClip hurtAnimation;
	
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
 
	public float hurtCooldown = 1.5f;
	private float hurtTime = 0.0f;
 
	// These variables are there for use by the script and don't need to be edited
	public enum CharacterState {
		Idle = 0,
		Walking = 1,
		Running = 2,
		Jumping = 3,
		Dead = 4,
		Hurt = 5
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
		if(!hurtAnimation) {
			Debug.Log(gameObject.name + ": PhysicsCharacterController: no hurtAnimation found, assign one in the inspector.");
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
		Transform cameraTransform = mainCamera.transform;

		//float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");
	
		bool wasMoving = isMoving;
		isMoving = rigidbody.velocity.magnitude > 0.1 ;
		
		// Target direction relative to the camera
		if (h != 0.0) charOrientation = h > 0.0 ? cameraTransform.TransformDirection(-1.0f*originalOrientation) : cameraTransform.TransformDirection(originalOrientation);
	
		// Grounded controls
		if (grounded && characterState != CharacterState.Hurt)
		{
			//	determine character state
			characterState = CharacterState.Walking;
			if (rigidbody.velocity.magnitude > maxWalkSpeed && allowRunning) { 
				characterState = CharacterState.Running;
			}
			else if (rigidbody.velocity.sqrMagnitude < 0.01f) {
				characterState = CharacterState.Idle;
			}
		
		}		
		else if (characterState == CharacterState.Hurt) {
			if (Time.time - hurtTime > hurtCooldown) {
				characterState = CharacterState.Idle;
			}				
		}
	}
	
	public void HurtPlayer() {
		hurtTime = Time.time;
		characterState = CharacterState.Hurt;
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
			if (characterState == CharacterState.Hurt) {
				animation.CrossFade(hurtAnimation.name);
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