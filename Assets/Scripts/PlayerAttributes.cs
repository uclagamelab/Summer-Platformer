using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {
	// some common stuff to
	public int health = 5; // starting health
	public int defaultHealth = 5;
	public int maxHealth = 5;
	
	public int playerLives = 3;
	public int maxPlayerLives = 5;
	public string deathLevelName = "";
	private bool playerIsDead = false;
	
	private Vector3 lastRespawn;
	
	public GameObject healthMeter;
	private Counter healthCounter;
	
	public GameObject livesMeter;
	private Counter livesCounter;
	
	private Camera cam;
	
	private GameObject player; 
	private PhysicsCharacterController pcc;
	
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(transform.parent.gameObject);
		
		// start the game with camera disabled
		foreach (Transform child in transform.parent) {
			if (child.name == "Main Camera") {
				GameObject cameraObject = child.gameObject;
				if (cameraObject != null) {
					cam = (Camera) cameraObject.GetComponent("Camera");
					if (cam != null) {
						cam.enabled = false;
					}
					else {
						Debug.LogWarning(gameObject.name + ": PlayerAttributes: 'Main Camera' does not have a 'Camera' component. That should break everything.");
					}
				}
				else {
					Debug.LogWarning(gameObject.name + ": PlayerAttributes: could not find a 'Main Camera' object. Please make sure there is one in the scene.");
				}
			}
		}
		
		player = GameObject.Find("Player");
	}
	
	void Start() {
		if (healthMeter != null) {
			healthCounter = (Counter) healthMeter.GetComponent("Counter");
			healthCounter.UpdateCounter(health);
		}
		else {
			Debug.LogWarning(gameObject.name + ": PlayerAttributes: healthMeter object has not been assigned in the inspector.");
		}
		
		if (livesMeter != null) {
			livesCounter = (Counter) livesMeter.GetComponent("Counter");
			livesCounter.UpdateCounter(playerLives-1);
		}
		else {
			Debug.LogWarning(gameObject.name + ": PlayerAttributes: livesMeter object has not been assigned in the inspector.");
		}
		pcc = (PhysicsCharacterController) GetComponent("PhysicsCharacterController");

	}
	
	void OnLevelWasLoaded() {
		// re-enable the cameraObject
		cam.enabled = true;
		
		if (healthMeter != null) {
			healthCounter = (Counter) healthMeter.GetComponent("Counter");
			healthCounter.UpdateCounter(health);
		}
		else {
			Debug.LogWarning(gameObject.name + ": PlayerAttributes: healthMeter object has not been assigned in the inspector.");
		}
		
		if (livesMeter != null) {
			livesCounter = (Counter) livesMeter.GetComponent("Counter");
			livesCounter.UpdateCounter(playerLives-1);
		}
		else {
			Debug.LogWarning(gameObject.name + ": PlayerAttributes: livesMeter object has not been assigned in the inspector.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerIsDead) {
			// check if player death animation is finished
			if (pcc.DeathAnimationFinished()) {
				playerLives -= 1;
				livesCounter.UpdateCounter(playerLives-1);
				if (playerLives < 1) {
					LoseGame();
				}
				// else we respawn the player
				RespawnPlayer();
				playerIsDead = false;
			}
		}
		
		healthCounter.UpdateCounter(health);
		livesCounter.UpdateCounter(playerLives-1);
	}
	
	void RespawnPlayer() {
		gameObject.transform.position = lastRespawn;
		// stop any movement
		player.rigidbody.velocity = Vector3.zero;
		//rigidbody.inertiaTensor = Vector3.zero;
		pcc.isControllable = true;
		health = defaultHealth;
		healthCounter.UpdateCounter(health);
	}
	
	public void SetRespawn(Vector3 newRespawn) {
		Debug.Log("set respawn to " + newRespawn);
		lastRespawn = newRespawn;
	}
	
	public void DecreaseHealth(int amount) {
		pcc.HurtPlayer();
		health -= amount;
		healthCounter.UpdateCounter(health);
		if (health < 1) {
			KillPlayer();
		}
		
	}
	
	public void IncreaseHealth(int amount) {
		health += amount;
		if (health > maxHealth) health = maxHealth;
		healthCounter.UpdateCounter(health);
	}
	
	public void IncreaseLife(int amount) {
		playerLives += amount;
		if (playerLives > maxPlayerLives) playerLives = maxPlayerLives;
		livesCounter.UpdateCounter(playerLives);
	}
	
	public void ActivateCamera() {
		cam.enabled = true;
	}
	
	public void KillPlayer() {
		playerIsDead = true;
		
		// make sure player can't move
		pcc.isControllable = false;
		pcc.characterState = PhysicsCharacterController.CharacterState.Dead;
		pcc.PlayDeathAnimation();
		
	}
	
	public void LoseGame() {
		Debug.Log(gameObject.name + ": PlayerAttributes: lose the game and send to level " + deathLevelName);
		cam.enabled = false;
		Destroy(player);
		Application.LoadLevel(deathLevelName);
	}
}
