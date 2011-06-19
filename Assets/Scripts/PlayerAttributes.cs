using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {
	// some common stuff to
	public int health = 5;
	public GameObject healthMeter;
	private Counter healthCounter;
	
	// Use this for initialization
	void Awake () {
		Debug.Log("player awake");
		DontDestroyOnLoad(transform.parent.gameObject);
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
	
	void KillPlayer() {
		// do a death animation
	}
}
