using UnityEngine;
using System.Collections;

public class HealthPickup: MonoBehaviour {
	
	public int healthValue = 1;
	
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player") {
			PlayerAttributes pa = (PlayerAttributes) other.gameObject.GetComponent("PlayerAttributes");
			pa.IncreaseHealth(healthValue);
			Destroy(gameObject);
		}
	}
}
