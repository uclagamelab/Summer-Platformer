using UnityEngine;
using System.Collections;

public class TextBoxControl : MonoBehaviour {
	
	public GameObject nextScreen;
	private Vector3 originalPosition;
	private bool windowIsActive = false;
	
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (windowIsActive) {
			if ( Input.GetAxis("Fire1") > 0.0f || Input.GetAxis("Jump") > 0.0f ) {
				if (nextScreen != null) {
					nextScreen.transform.position = transform.position;
					transform.position = originalPosition;
					windowIsActive= false;
					TextBoxControl tbc = (TextBoxControl) nextScreen.GetComponent("TextBoxControl");
					tbc.ActivateWindow();
				}
				else {
					Time.timeScale = 1.0f;
					transform.position = originalPosition;
					windowIsActive= false;
				}
			}
		}
	}
	
	public void ActivateWindow() {
		windowIsActive = true;
	}
}
