using UnityEngine;
using System.Collections;

public class DrawTransformLocation : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnDrawGizmosSelected () {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, 0.25f);
	}
}
