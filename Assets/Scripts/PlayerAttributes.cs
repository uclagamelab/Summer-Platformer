using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {
	// some common stuff to
	public int health = 5; // starting health
	public int maxHealth = 5;
	public GameObject healthMeter;
	private Counter healthCounter;
	
	private Camera cam;
	
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
	}
	
	void Start() {
		if (healthMeter != null) {
			healthCounter = (Counter) healthMeter.GetComponent("Counter");
			healthCounter.counterValue = health;
		}
		else {
			Debug.LogWarning(gameObject.name + ": PlayerAttributes: healthMeter object has not been assigned in the inspector.");
		}
	}
	
	void OnLevelWasLoaded() {
		// re-enable the cameraObject
		cam.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void DecreaseHealth(int amount) {
		health -= amount;
		healthCounter.UpdateCounter(health);
		if (health < 0) {
			KillPlayer();
		}
		
	}
	
	public void IncreaseHealth(int amount) {
		health += amount;
		if (health > maxHealth) health = maxHealth;
		healthCounter.UpdateCounter(health);
	}
	
	public void ActivateCamera() {
		cam.enabled = true;
	}
	
	void KillPlayer() {
		// do a death animation
	}
}
