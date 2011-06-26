/*

This can cause damage to a player as a touch or melee style attack from a enemy or enemy trap.

An enemy can have this script added to the main rigidboy gameObject and cause damage to the player
on every touch, but the player might not be able to kill without taking damage. If this is added to a
section of the enemy, like the hand or a claw or something, then the collider needs to be a trigger.

*/


using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour {
	public int damageAmount = 1;
	public bool causePushBack = false;
	public float pushBackForce = 100.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player"){ 
			Debug.Log(gameObject.name + ": DamagePlayer: trigger found a player " + other.gameObject.name);
			PlayerAttributes pa = (PlayerAttributes)other.gameObject.GetComponent("PlayerAttributes");
			if (pa != null) pa.DecreaseHealth(damageAmount);
		}
	}
	
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player"){ 
			Debug.Log(gameObject.name + ": DamagePlayer: found a player with collision " + other.gameObject.name);
			PlayerAttributes pa = (PlayerAttributes)other.gameObject.GetComponent("PlayerAttributes");
			pa.DecreaseHealth(damageAmount);
			if (causePushBack) {
				for (int i = 0; i < other.contacts.Length; i++) {
					ContactPoint cp = other.contacts[i];
					Vector3 direction = other.gameObject.transform.position - cp.point;
					direction = direction.normalized * pushBackForce;
					other.rigidbody.AddForceAtPosition(direction, cp.point); 
				}
			}
		}
		//Debug.Log("touch");
	}
}
