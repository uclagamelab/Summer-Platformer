using UnityEngine;
using System.Collections;

public class Counter : MonoBehaviour {
	public int counterValue;
	public GameObject[] imagePlanes;
	
	private int maxValue;
	
	// Use this for initialization
	void Start () {
		maxValue = imagePlanes.Length;
	}
	
	// Update is called once per frame
	void Update () {
		// draw the counter
		
	}
	
	public void UpdateCounter(int val) {
		counterValue = val;
		print(val);
		for (int i = 0; i < maxValue; ++i) {
			GameObject imagePlane = imagePlanes[i];
			if (i < counterValue) {
				imagePlane.active = true;
			}
			else imagePlane.active = false;
		}
	}
}
