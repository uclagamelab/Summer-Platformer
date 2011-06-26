using UnityEngine;
using System.Collections;

public class DestroyAfterSeconds : MonoBehaviour {
	public float timeToDeath = 1.0f;
	
	private float timeBorn = 0.0f;
	
	// Use this for initialization
	void Start () {
		timeBorn = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timeBorn > timeToDeath) {
			Destroy(gameObject);
		}
	}
}
