using UnityEditor;
using UnityEngine;
using System.Collections;

class DeleteAnimationFrame : ScriptableWizard {
    // default values
	public AnimationClip anim;
	public int deleteFrameBegin;
	public int deleteFrameEnd;
    
    [MenuItem ("Scripts/DeleteAnimationFrame")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard<DeleteAnimationFrame>("Delete Animation Frame", "Delete Frames");
    }
	
	// this corresponds with the Delete Animation Frame button
    void OnWizardCreate () {
		//AnimationClip animCopy = new AnimationClip();
		AnimationClipCurveData[] origCurves = AnimationUtility.GetAllCurves(anim, true);
		float frameRate = anim.frameRate;
		float beginDeleteTime = deleteFrameBegin/frameRate;
		float endDeleteTime = deleteFrameEnd/frameRate;
		float framesDeleted = deleteFrameBegin - deleteFrameEnd +1;
		float deleteTime = endDeleteTime - beginDeleteTime;
		float newTotalTime = anim.length - deleteTime;
		Debug.Log("clear from " + beginDeleteTime + " to " + endDeleteTime);
		anim.ClearCurves();
		//Debug.Log("animation has " + origCurves.Length + " animation curves");
		for (int i = 0; i < origCurves.Length; i++) {
			AnimationClipCurveData accd = origCurves[i];
			//Debug.Log(i + " curve has path " + accd.path + ", property name " + accd.propertyName );//+ ", target " + accd.target.name);
			AnimationCurve ac = accd.curve;
			//Debug.Log(i + " curve has " + ac.length + " keys");
			AnimationCurve newCurve = new AnimationCurve();
			for (int j = 0; j < origCurves[i].curve.length; j++) {
				Keyframe k = origCurves[i].curve.keys[j];
				float time = k.time;
				if (time >= beginDeleteTime && time <= endDeleteTime ) {
					if (origCurves[i].curve.length < 3) {
						// slide this value to the beginning or the end of the animation
						// which way to go?
						if ( (time + deleteTime) < newTotalTime) {
							newCurve.AddKey(new Keyframe(time + deleteTime + (1.0f)/frameRate, k.value));
						}
						else newCurve.AddKey(new Keyframe(time - deleteTime - (1.0f)/frameRate , k.value));
					}
					//else ac.RemoveKey(j);
				}
				else if ( time > endDeleteTime) {
					float newtime = time - deleteTime;
					newCurve.AddKey(new Keyframe(newtime, k.value));
				}
				else { // this should be before
					//k.time = j/frameRate;
					newCurve.AddKey(new Keyframe(time, k.value));
					//Debug.Log("Add key before at " + time);
				}
			}
			anim.SetCurve(accd.path, accd.type, accd.propertyName, newCurve);
			//Debug.Log(i + " curve now has " + ac.length + " keys");
		}
		//AnimationUtility.SetAnimationClips(anim, origCurves);
    }  
    void OnWizardUpdate () {
		
        helpString = "delete frames from animation";
    }   
    // When the user pressed the "New Mesh" button OnWizardOtherButton is called.
    void OnWizardOtherButton () {
        
	}
	
}