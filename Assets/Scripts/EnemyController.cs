// derived from http://www.unifycommunity.com/wiki/index.php?title=PhysicsFPSWalker

using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
		
	private Animation anim;
	
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip deathAnimation;
	public AnimationClip hurtAnimation;
	
	public float walkAnimationSpeed = 1.0f;
	public float walkMaxAnimationSpeed  = 0.75f;
	public float jumpAnimationSpeed = 1.15f;

	// These variables are for adjusting in the inspector how the object behaves
	public float maxWalkSpeed = 6.0f;
	public float walkForce     = 8.0f;
 
	public float hurtCooldown = 1.5f;
	private float hurtTime = 0.0f;
	
	public int health = 2;
	public float killDelayTime = 1.5f;
	public int scoreValue = 1;
	
	public bool enemyCanShoot = false;
	public AnimationClip enemyShootAnimation;
	public GameObject projectilePrefab;
	public GameObject shootSpawnPoint;
	public float shootCooldown = 2.0f;
	private float shootTime = 0.0f;
	public bool switchShootDirection = false;
 
	// These variables are there for use by the script and don't need to be edited
	public enum EnemyState {
		Idle = 0,
		Walking = 1,
		Dead = 2,
		Hurt = 3
	}
	public EnemyState enemyState = 0;
	
	//private bool isMoving = false;
	private Vector3 originalOrientation = Vector3.zero;
	private Vector3 charOrientation = Vector3.zero;
	//[HideInInspector]
	public bool isControllable = true; // shut down controls if necesary
	private Vector3 addForce = Vector3.zero;
 
	private GameObject mainCamera;
	public Vector3 attackDirection;
	
	//private bool printPos = true;
	private GameObject thePlayer;
	
	private ScoreUpdate scoreUpdate;
	
	void Awake ()
	{
			// Don't let the Physics Engine rotate this physics object so it doesn't fall over when running

		rigidbody.freezeRotation = true;
		charOrientation = transform.TransformDirection(Vector3.forward);
		originalOrientation = transform.TransformDirection(Vector3.forward);
				
		anim = (Animation) GetComponent("Animation");
		if(!anim) Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		if(!idleAnimation) {
			anim = null;
			Debug.Log("No idle animation found. Turning off animations.");
		}
		if(!walkAnimation) {
			anim = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if(!deathAnimation) {
			anim = null;
			Debug.Log("No death animation found and the character has canJump enabled. Turning off animations.");
		}
		if(!hurtAnimation) {
			Debug.Log(gameObject.name + ": EnemyController: no hurtAnimation found, assign one in the inspector.");
		}
		
	}
	
	void Start() {
		thePlayer= GameObject.Find("Player");
	}
	
	void OnLevelWasLoaded() {

	}
 
	void OnTriggerStay (Collider other)
	{
		//Debug.Log(gameObject.name + ": EnemyController: " + other.gameObject.name + " detected");

		if (other.gameObject.tag == "Player") {
			//Debug.Log(gameObject.name + ": EnemyController: player " + other.gameObject.name + " detected");
			attackDirection = other.gameObject.transform.position - transform.position;

			//attackDirection = transform.position - other.gameObject.transform.position;
			//if (printPos) Debug.Log(attackDirection);
			if (enemyCanShoot && Time.time - shootTime > shootCooldown) {
				if (projectilePrefab != null) {
					//Quaternion q = Quaternion.AngleAxis(90-transform.eulerAngles.y, Vector3.up);
					Quaternion q = Quaternion.identity;
					q.SetLookRotation(attackDirection ) ;
					
					GameObject g = (GameObject) Instantiate(projectilePrefab, shootSpawnPoint.transform.position, q);
					if (switchShootDirection) g.transform.Rotate(Vector3.up*-90.0f);
					else g.transform.Rotate(Vector3.up*90.0f);
				}				
				shootTime = Time.time;
			}
		}
	}
	
	void OnTriggerExit( Collider other) {
		if (other.gameObject.tag == "Player") {
			//Debug.Log("player detected");
			attackDirection =Vector3.zero;
		}
	}
	
	void OnCollisionEnter(Collision other) {
		
	}
 
	void OnCollisionExit (Collision other)
	{

	}
 
	public virtual float horizontal
	{
		get
		{
			return attackDirection.normalized.x;
		}
	}
	
	void UpdateSmoothedMovementDirection() {
		
		if (mainCamera == null) {
			mainCamera = (GameObject) GameObject.FindWithTag("MainCamera");
		}
		Transform cameraTransform = mainCamera.transform;

		float h = horizontal; //Input.GetAxisRaw("Horizontal");
		//if (h > 0.0f) Debug.Log(h);
	
		//bool wasMoving = isMoving;
		//isMoving = rigidbody.velocity.magnitude > 0.1 ;
		
		// Target direction relative to the camera
		if (h != 0.0) charOrientation = h > 0.0 ? cameraTransform.TransformDirection(originalOrientation) : cameraTransform.TransformDirection(-1.0f*originalOrientation);
	
		// Grounded controls
		if (enemyState != EnemyState.Hurt)
		{
			//	determine character state
			enemyState = EnemyState.Walking;
			if (rigidbody.velocity.sqrMagnitude < 0.01f) {
				enemyState = EnemyState.Idle;
			}
		
		}		
		else if (enemyState == EnemyState.Hurt) {
			if (Time.time - hurtTime > hurtCooldown) {
				enemyState = EnemyState.Idle;
			}				
		}
	}
	
	public void DamageEnemy(int damage ) {
		//Debug.Log(gameObject.name + " EnemyController: DamageEnemy " + damage);
		health -= damage;
		HurtEnemy();
	}
	
	public void HurtEnemy() {
		hurtTime = Time.time;
		enemyState = EnemyState.Hurt;
		if (health < 1) {
			KillEnemy();
		}
	}

	// This is called every physics frame
	void FixedUpdate ()
	{
		if (!isControllable)
		{
			return;
		}
		
		//attackDirection = Vector3.zero;

		UpdateSmoothedMovementDirection();
		Vector3 lookTarget = new Vector3(charOrientation.x+transform.position.x, transform.position.y, -(transform.position.z + charOrientation.z));
		transform.LookAt(lookTarget);
 
		// If the object is grounded and isn't moving at the max speed or higher apply force to move it
		if(Mathf.Abs(horizontal) > 0.0f && rigidbody.velocity.magnitude < maxWalkSpeed)
		{
			addForce = Vector3.left*  -horizontal ;
			//if (printPos) Debug.Log("h " + horizontal);
			addForce.y = 0.05f;
			rigidbody.AddForce(addForce*walkForce);
			//if (printPos) Debug.Log("add force " + addForce);
		}
		
		// ANIMATION sector
		if(anim) {
				if (enemyState == EnemyState.Idle) {
					animation.CrossFade(idleAnimation.name);
				}
				else if(enemyState == EnemyState.Walking) {
						anim[walkAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude*walkAnimationSpeed, 0.0f, walkMaxAnimationSpeed);
						anim.CrossFade(walkAnimation.name);	
				}
				else if(enemyState == EnemyState.Dead) {
						anim.CrossFade(deathAnimation.name);
				}
				if (enemyState == EnemyState.Hurt) {
					anim.CrossFade(hurtAnimation.name);
				}
		}
		// ANIMATION sector
	}
	
	public void KillEnemy() {
		if (isControllable) {
			enemyState = EnemyState.Dead;
			isControllable = false;
			PlayDeathAnimation();
			thePlayer = GameObject.Find("Player");
			thePlayer.BroadcastMessage("UpdateScore", scoreValue);
			Destroy(gameObject, killDelayTime);
		}
	}
	
	public void PlayDeathAnimation() {
		anim.Play(deathAnimation.name);
		//Debug.Log("Play death animation");
	}
	
	public bool DeathAnimationFinished() {
		if (anim.IsPlaying(deathAnimation.name)) {
			return false;
		}
		return true;
	}
}