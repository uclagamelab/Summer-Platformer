using UnityEngine;
using System.Collections;

public class AnimationAudioMatcher : MonoBehaviour {
	
	private Animation ani;
	
	private AudioSource audioSource;
	public float audioDelay = 0.01f;
	public float lastBeginTime = 0.0f;
	public float minAnimationTime = 0.03f;
	
	// these must match those from animation
	public AnimationClip[] animations;
	
	public AudioClip[] audioClips;
	private AnimationClip lastClip;
	private AnimationClip lastPlayingClip;
	
	// Use this for initialization
	void Start () {
		ani = (Animation) GetComponent("Animation");
		audioSource = (AudioSource) GetComponent("AudioSource");
		
		if (ani == null) {
			Debug.Log(gameObject.name + ": AnimationAudioMatcher: couldn't find animation component");
		}
		
	}
	
	/* // Update is called once per frame
	void Update () {
		if (ani.isPlaying ) { //&& Time.time - lastBeginTime > audioDelay) {
			for (int i = 0; i < animations.Length; i++) {
				if (ani.IsPlaying(animations[i].name)) {
					if (animations[i] != lastClip) {
						lastBeginTime
						if (audioClips[i] != null) {
							if (lastClip != null) Debug.Log("this clip is " + animations[i] + ", last clip was " + lastClip + " " + Time.time);
							//audioSource.Stop();
							audioSource.clip = audioClips[i];
							audioSource.Play();
							Debug.Log("Play");
							lastBeginTime = Time.time;
						}
						lastClip = animations[i];
					}
					else if (!audioSource.isPlaying && audioClips[i] != null) {
						audioSource.Play();
						Debug.Log("Play");
						lastBeginTime = Time.time;
					}
				}
			}
		}
	} */
	
	void Update () {
		if (ani.isPlaying ) { //&& Time.time - lastBeginTime > audioDelay) {
			for (int i = 0; i < animations.Length; i++) {
				if (ani.IsPlaying(animations[i].name)) { // is animations i playing?
					if (animations[i] != lastPlayingClip) { // is it different from the last clip playing
						if (animations[i] == lastClip) {
							if (audioClips[i] != null && Time.time - lastBeginTime > minAnimationTime) {
								audioSource.Stop();
								audioSource.clip = audioClips[i];
								audioSource.Play();
								lastPlayingClip = animations[i];
								lastBeginTime = Time.time;
							}
						}
						else {
							lastClip = animations[i];
							lastBeginTime = Time.time;
						}
					}
					else if (!audioSource.isPlaying && audioClips[i] != null && Time.time - lastBeginTime > audioDelay) {
						audioSource.Play();
						//Debug.Log("Play");
						lastBeginTime = Time.time;
					}
				}
			}
		}
	}
}
