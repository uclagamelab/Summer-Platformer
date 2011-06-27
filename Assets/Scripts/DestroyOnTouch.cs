using UnityEngine;
using System.Collections;

public class DestroyOnTouch : MonoBehaviour {
	
	public string objectTagToDestroy = "";
	public float destroyAfterSeconds = 1.0f;
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == objectTagToDestroy) {
			Destroy(gameObject, destroyAfterSeconds);
		}
	}
	
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == objectTagToDestroy) {
			Destroy(gameObject, destroyAfterSeconds);
		}
	}
	
}
