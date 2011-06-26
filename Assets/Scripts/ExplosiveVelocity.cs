using UnityEngine;
using System.Collections;

public class ExplosiveVelocity : MonoBehaviour {
	
	public float projectileForce = 100.0f;
	
	// Use this for initialization
	void Start () {
		//rigidbody.AddForce(projectileForce * Vector3.right); 
	}
	
	// Update is called once per frame
	void Update () {
		if (projectileForce != 0.0f) {
			rigidbody.AddForce(projectileForce * Vector3.right); 
			projectileForce = 0.0f;
		}
	}
}
