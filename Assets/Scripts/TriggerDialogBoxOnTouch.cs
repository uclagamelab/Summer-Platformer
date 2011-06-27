using UnityEngine;
using System.Collections;

public class TriggerDialogBoxOnTouch : MonoBehaviour {
	
	public GameObject dialogBox;
	public bool destroyTriggerAfterReading = true;
	public string tagToTriggerDialog = "Player";
	
	
	// Use this for initialization
	void Start () {
		// find the player
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if (other.name == tagToTriggerDialog) {
			
			
			// bring up the window
			GameObject thePlayer = GameObject.Find("Player");
			Debug.Log("player at " + thePlayer.transform.position);
			PlayerAttributes pa = (PlayerAttributes) thePlayer.GetComponent("PlayerAttributes");
			Debug.Log("dialogBox location " + pa.textBoxLocation.transform.position);
			Debug.Log("dialogBox location " + pa.textBoxLocation.transform.localPosition);
			dialogBox.transform.position = pa.textBoxLocation.transform.position;
			//dialogBox.transform.position = pa.textBoxLocation.transform.position;
			TextBoxControl tbc = (TextBoxControl) dialogBox.GetComponent("TextBoxControl");
			tbc.ActivateWindow();
			
			// pause the game
			Time.timeScale = 0.0001f;
			
			// destroy if necessary
			if (destroyTriggerAfterReading) {
				Destroy(gameObject);
			}
		}
	}
}
