using UnityEngine;
using System.Collections;

public class AnimationAudioMatcher : MonoBehaviour {
	
	private Animation ani;
	
	private AudioSource audioSource;
	public float audioDelay;
	
	// these must match those from animation
	public AnimationClip[] animations;
	
	public AudioClip[] audioClips;
	private int numAnims;
	private AnimationClip lastClip;
	
	// Use this for initialization
	void Start () {
		ani = (Animation) GetComponent("Animation");
		audioSource = (AudioSource) GetComponent("AudioSource");
		
		if (ani != null) {
			numAnims = ani.GetClipCount();
		}
		else {
			Debug.Log(gameObject.name + ": AnimationAudioMatcher: couldn't find animation component");
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (ani.isPlaying) {
			for (int i = 0; i < animations.Length; i++) {
				if (ani.IsPlaying(animations[i].name)) {
					if (animations[i] != lastClip) {
						audioSource.Stop();
						audioSource.clip = audioClips[i];
						lastClip = animations[i];
						audioSource.Play();
					}
					else if (!audioSource.isPlaying) {
						audioSource.Play();
					}
					if (!audioSource.isPlaying) {
						audioSource.Play();
					}
				}
			}
		}
	}
}
