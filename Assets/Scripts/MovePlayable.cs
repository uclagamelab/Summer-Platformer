using UnityEngine;
using System.Collections;

public class MovePlayable : MonoBehaviour {
	public float speed;
	public float maxAngle;
	public float angleSpeed;
	
	private Quaternion uprightOrientation;

	// Use this for initialization
	void Start () {
		uprightOrientation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		float translationUp  = Input.GetAxis ("Vertical") * speed;
		//if ( Input.GetAxis ("Vertical") != 0) Debug.Log(translationUp);
		float translationSide  = Input.GetAxis("Horizontal") * speed;
		
		translationUp = Time.deltaTime * translationUp;
		translationSide = Time.deltaTime * translationSide;
		
		transform.Translate(0, translationUp, 0);
		transform.Translate( translationSide, 0, 0);
		
		if (transform.rotation.z > maxAngle || transform.rotation.z < -maxAngle)
		{
			Debug.Log(transform.rotation.z);
			transform.rotation = Quaternion.Slerp(transform.rotation, uprightOrientation, Time.deltaTime * angleSpeed);
		}
	}
	
	void doCrouch() {
		
	}
	
	void doJump() {
		
	}
}
