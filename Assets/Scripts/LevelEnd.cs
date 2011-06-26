using UnityEngine;
using System.Collections;

public class LevelEnd : MonoBehaviour {
	public AnimationClip clipForEnd;
	public bool playAnimationOnLevelEnd = false;
	public string nextLevel;
	
	private bool reachedEnd = false;
	private Animation anim;
	
	// Use this for initialization
	void Start () {
		if (playAnimationOnLevelEnd) {
			anim = (Animation) GetComponent("Animation");
			anim.playAutomatically = false;
		}
		if (nextLevel == "") Debug.LogWarning("Level End leads to nowhere. You may want to set a level name in the inspector");
	}
	
	// Update is called once per frame
	void Update () {
		if (reachedEnd) {
			if ( (playAnimationOnLevelEnd && !animation.IsPlaying(clipForEnd.name) ) || !playAnimationOnLevelEnd ) {
				PhysicsCharacterController pa = (PhysicsCharacterController)GameObject.Find("Player").GetComponent("PhysicsCharacterController");
				pa.isControllable = false;
				Application.LoadLevel(nextLevel);
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		// did  player activate win??
		if (other.gameObject.tag == "Player") {
			reachedEnd = true;
			if (playAnimationOnLevelEnd) animation.Play(clipForEnd.name);
		}
	}
}
