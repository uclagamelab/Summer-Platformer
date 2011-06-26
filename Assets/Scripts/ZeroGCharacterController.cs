// derived from http://www.unifycommunity.com/wiki/index.php?title=PhysicsFPSWalker

using UnityEngine;
using System.Collections;

public class ZeroGCharacterController : MonoBehaviour {
	
	public Camera mainCamera;
	
	private Animation animation;
	
	public AnimationClip idleAnimation;
	public AnimationClip flyAnimation;
	public AnimationClip deathAnimation;
	public AnimationClip hurtAnimation;
	public AnimationClip projectileAnimation;
	
	public float flyAnimationSpeed = 1.0f;
	public float flyMaxAnimationSpeed  = 0.75f;
	public float projectileAnimationSpeed = 1.0f;
	
	public GameObject projectilePrefab;
	public GameObject projectileCreationPoint;

	// These variables are for adjusting in the inspector how the object behaves
	public float maxFlySpeed = 6.0f;
	public float flyForce     = 8.0f;
 
	public float hurtCooldown = 1.5f;
	private float hurtTime = 0.0f;
	public float shootCooldown = 1.0f;
	private float shootTime = 0.0f;
	 
	// These variables are there for use by the script and don't need to be edited
	public enum CharacterState {
		Idle = 0,
		Flying = 1,
		Dead = 4,
		Hurt = 5,
		Shooting = 6
	}
	public CharacterState characterState = 0;
	
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
		if(!flyAnimation) {
			animation = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if(!deathAnimation) {
			animation = null;
			Debug.Log("No death animation found and the character has canJump enabled. Turning off animations.");
		}
		if(!hurtAnimation) {
			Debug.Log(gameObject.name + ": PhysicsCharacterController: no hurtAnimation found, assign one in the inspector.");
		}
		if (!projectileAnimation) {
			Debug.Log(gameObject.name + ": PhysicsCharacterController: no projectile animation found. Turning off animations");
			animation = null;
		}
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

	}
 
	void OnCollisionExit ()
	{

	}
	
	// we'll map unity "jump" to projectile
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
	
	public virtual float vertical
	{
		get
		{
		
			return Input.GetAxis("Vertical") ;
		}
	}
	
	void UpdateSmoothedMovementDirection() {
		Transform cameraTransform = mainCamera.transform;

		//float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");
	
		//bool wasMoving = isMoving;
		//isMoving = rigidbody.velocity.magnitude > 0.1 ;
		
		// Target direction relative to the camera
		if (h != 0.0) charOrientation = h > 0.0 ? cameraTransform.TransformDirection(-1.0f*originalOrientation) : cameraTransform.TransformDirection(originalOrientation);
	
		// Grounded controls
		if (characterState != CharacterState.Hurt && characterState != CharacterState.Shooting)
		{
			//	determine character state
			characterState = CharacterState.Flying;
			if (rigidbody.velocity.sqrMagnitude < 0.01f) {
				characterState = CharacterState.Idle;
			}
		
		}		
		else if (characterState == CharacterState.Hurt) {
			if (Time.time - hurtTime > hurtCooldown) {
				characterState = CharacterState.Idle;
			}				
		}
		if (jump && (Time.time - shootTime > shootCooldown) ) { // remapped jump to projectile
			//Debug.Log("shoot");
			// do projectile animation and prefab 
			characterState = CharacterState.Shooting;
			animation.CrossFade(projectileAnimation.name);
			shootTime = Time.time;
			// instantiate the projectile
			if (projectilePrefab != null) {
				Instantiate(projectilePrefab, projectileCreationPoint.transform.position, Quaternion.AngleAxis(90-transform.eulerAngles.y, Vector3.up));//gameObject.transform.rotation);
				//Debug.Log("projectile");
			}
			else Debug.LogWarning(gameObject.name + ":PhysicsCharacterController: tried to shoot but no prefab present");
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
 
		 if(Mathf.Abs(vertical) > 0.0f  && rigidbody.velocity.magnitude < maxFlySpeed )
		{
			addForce = transform.rotation * Vector3.up * Mathf.Abs(vertical) ;
			rigidbody.AddForce(addForce*flyForce);		}
		else if(Mathf.Abs(horizontal) > 0.0f && rigidbody.velocity.magnitude < maxFlySpeed )
		{
			addForce = transform.rotation * Vector3.forward * Mathf.Abs(horizontal) ;
			addForce.y = 0.1f;
			rigidbody.AddForce(addForce*flyForce);
		}
		
		// ANIMATION sector
		if(animation) {
				//if(rigidbody.velocity.sqrMagnitude < 0.01) {
				if (characterState == CharacterState.Idle) {
					animation.CrossFade(idleAnimation.name);
				}
				else 
				{
					 if(characterState == CharacterState.Flying) {
						animation[flyAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude*flyAnimationSpeed, 0.0f, flyMaxAnimationSpeed);
						animation.CrossFade(flyAnimation.name);	
					}
					else if(characterState == CharacterState.Dead) {
						animation.CrossFade(deathAnimation.name);
					}
				}
			if (characterState == CharacterState.Hurt) {
				animation.CrossFade(hurtAnimation.name);
			}
			else if (characterState == CharacterState.Shooting) {
				if (!animation.IsPlaying(projectileAnimation.name)) {
					characterState = CharacterState.Idle;
				}
			}
		}
	// ANIMATION sector
	}
	
	public void PlayDeathAnimation() {
		animation.Play(deathAnimation.name);
		//Debug.Log("Play death animation");
	}
	
	public bool DeathAnimationFinished() {
		if (animation.IsPlaying(deathAnimation.name)) {
			return false;
		}
		return true;
	}
}