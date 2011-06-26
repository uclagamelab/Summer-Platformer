using UnityEngine;
using System.Collections;

public class ConstantVelocity : MonoBehaviour {
	
	public float projectileSpeed = 1.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//rigidbody.velocity = new Vector3(projectileSpeed, 0.0f,0.0f);
		//transform.position = new Vector3(transform.position.x + (Time.deltaTime * projectileSpeed), transform.position.y, transform.position.z);
		transform.position += transform.right *projectileSpeed;
	}
}
