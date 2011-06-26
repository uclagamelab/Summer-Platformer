using UnityEngine;
using System.Collections;

public class DamageEnemyMelee: MonoBehaviour {
	
	public int damage = 1;
	public bool damageActive = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// make melee attack only work when fire button is down
		if (Input.GetAxis("Fire1") >0.01f) {
			damageActive = true;
		}
		else {
			damageActive = false;
		}
	}
	
	void OnCollisionEnter(Collision other) {
		//Debug.Log(gameObject.name + ": DamageEnemy: " + other.gameObject.name + " OnCollisionEnter");
		if (damageActive && other.gameObject.tag == "Enemy") {
			other.gameObject.BroadcastMessage("DamageEnemy", damage);
			Debug.Log(other.gameObject.name + " has tag of enemy");
		}
	}
	
	void OnTriggerEnter(Collider other) {
		//Debug.Log(gameObject.name + ": DamageEnemy: trigger enter for " + other.name);
		if (damageActive && other.tag == "Enemy" || other.name == "Enemy") {
			//other.gameObject.BroadcastMessage("DamageEnemy", damage);
			other.gameObject.SendMessageUpwards("DamageEnemy", damage);
			Debug.Log("damage enemy");
		}
	}
}
