using UnityEngine;
using System.Collections;

public class LevelBegin : MonoBehaviour {
	// set the default values for the level
	public GameObject defaultPlayer;
	public GameObject firstSpawnPoint;
	public bool playAnimationOnLevelStart = false;
	public AnimationClip clipForStart;
	public Transform playerStartLocation;
	
	private Animation animation;
	private GameObject player;
	private bool playerReady = false;
	private PhysicsCharacterController physicsController;
	
	// Use this for initialization
	void Start () {
		// find the player and move to this location
		GameObject player = GameObject.Find("Player");
		if (player == null) {
			player = (GameObject)Instantiate(defaultPlayer, Vector3.zero, Quaternion.identity);
		}
		if (playerStartLocation != null) {
			player.transform.position = playerStartLocation.position;
		}
		else {
			player.transform.position = transform.position;
		}
		
		// make player unmoveable if animation needs playing
		if (playAnimationOnLevelStart) {
			physicsController = (PhysicsCharacterController) player.GetComponent("PhysicsCharacterController");
			physicsController.isControllable = false;
			animation = (Animation) GetComponent("Animation");
			animation.playAutomatically = true;
		}
		else {
			playerReady = true;
		}
		Debug.Log("Level begin start");
	}
	
	void OnLevelWasLoaded() {
		Debug.Log("Level begin level loaded");
	}
	
	void Update() {
		if (!playerReady) {
			if (!animation.IsPlaying(clipForStart.name) ) {
				playerReady = true;
				physicsController.isControllable = true;
			}
		}
	}

}
