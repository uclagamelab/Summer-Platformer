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
	
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player"){ 
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
		Debug.Log("touch");
	}
}
