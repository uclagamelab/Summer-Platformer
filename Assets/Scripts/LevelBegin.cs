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
	private GameObject playerAndGUI;
	private GameObject player;
	private bool playerReady = false;
	private PhysicsCharacterController physicsController;
	
	// Use this for initialization
	void Start () {
		// find the player and move to this location
		GameObject player = GameObject.Find("Player");
		if (player == null) {
			playerAndGUI = (GameObject)Instantiate(defaultPlayer, Vector3.zero, Quaternion.identity);//AngleAxis(270.0f,Vector3.up) );
			foreach (Transform child in playerAndGUI.transform) {
				if (child.gameObject.name == "Player") {
					player = child.gameObject;
				}
			}
		}
		if (playerStartLocation != null) {
			player.transform.position = playerStartLocation.position;
		}
		else {
			player.transform.position = transform.position;
		}
		
		// stop any movement
		player.rigidbody.velocity = Vector3.zero;
		
		// make sure the camera is enabled
		PlayerAttributes pa = (PlayerAttributes) player.GetComponent("PlayerAttributes");
		pa.ActivateCamera();
		
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
		//Debug.Log("Level begin start");
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
