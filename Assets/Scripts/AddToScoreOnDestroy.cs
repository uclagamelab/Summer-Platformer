using UnityEngine;
using System.Collections;

public class AddToScoreOnDestroy : MonoBehaviour {
	
	public int scoreValue = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDestroy() {
		GameObject thePlayer = GameObject.Find("Player");
		thePlayer.BroadcastMessage("UpdateScore", scoreValue);
	}
}
