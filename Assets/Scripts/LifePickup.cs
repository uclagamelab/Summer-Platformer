using UnityEngine;
using System.Collections;

public class LifePickup: MonoBehaviour {
	
	public int lifeValue = 1;
	
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player") {
			PlayerAttributes pa = (PlayerAttributes) other.gameObject.GetComponent("PlayerAttributes");
			pa.IncreaseLife(lifeValue);
			Destroy(gameObject);
		}
	}
}
