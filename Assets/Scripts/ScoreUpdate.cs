using UnityEngine;
using System.Collections;

public class ScoreUpdate : MonoBehaviour {

	public int score = 0;
	
	private TextMesh text;
	// Use this for initialization
	void Start () {
		text = (TextMesh) GetComponent("TextMesh");
	}
	
	// Update is called once per frame
	void Update () {
		text.text = score.ToString();
	}
	
	public void UpdateScore(int newScore) {
		score = newScore;
	}
}
