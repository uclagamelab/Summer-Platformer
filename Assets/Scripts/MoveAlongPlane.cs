using UnityEngine;
using System.Collections;

// handle moving an object in the main plane of movement that is parrallel to the camera

public class MoveAlongPlane : MonoBehaviour {
	// Use this for initialization
	void Start () {
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
