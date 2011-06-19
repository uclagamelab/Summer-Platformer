using UnityEngine;
using System.Collections;

public class PivotTools : MonoBehaviour {
	public bool drawPivot = false;
	
	
	// Update is called once per frame
	void OnDrawGizmosSelected () {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, 0.25f);
	}
}
