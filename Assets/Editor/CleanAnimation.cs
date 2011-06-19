using UnityEditor;
using UnityEngine;
using System.Collections;

class CleanAnimation : ScriptableWizard {
    // default values
	public AnimationClip anim;
	public bool deleteScaleFrames;
	public bool matchBeginEndKeys;
	public bool deleteRotationX;
	public bool deleteRotationY;
	public bool deleteRotationZ;
    
    [MenuItem ("Scripts/Clean Animation")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard<CleanAnimation>("Clean Animation", "Clean");
    }
	
	// this corresponds with the Delete Animation Frame button
    void OnWizardCreate () {
		//AnimationClip animCopy = new AnimationClip();
		AnimationClipCurveData[] origCurves = AnimationUtility.GetAllCurves(anim, true);
		anim.ClearCurves();
		//Debug.Log("animation has " + origCurves.Length + " animation curves");
		for (int i = 0; i < origCurves.Length; i++) {
			AnimationClipCurveData accd = origCurves[i];
			if (deleteScaleFrames ){
				if (accd.propertyName == "m_LocalScale.x" || accd.propertyName == "m_LocalScale.y" || accd.propertyName == "m_LocalScale.z") continue;
			}
			if (deleteRotationX && accd.propertyName == "m_LocalRotation.x") continue;
			if (deleteRotationY && accd.propertyName == "m_LocalRotation.y") continue;
			if (deleteRotationZ && accd.propertyName == "m_LocalRotation.z") continue;
			
			//Debug.Log(i + " curve has path " + accd.path + ", property name " + accd.propertyName );//+ ", target " + accd.target.name);
			AnimationCurve ac = accd.curve;
			//Debug.Log(i + " curve has " + ac.length + " keys");
			// find first keys
			Keyframe[] keys = ac.keys;
			if (ac.length < 2) continue;
			Keyframe firstKey = keys[0];
			if (firstKey.time != 0) Debug.LogWarning("Clean Animation Warning: found 0 keyframe with time" + firstKey.time);
			
			Keyframe secondKey = keys[origCurves[i].curve.length-1];
			float newValue = (firstKey.value + secondKey.value)/2.0f;
			
			//Debug.Log("Clean Animation : old values " + ac.keys[0].time + ":" + ac.keys[0].value + "," + ac.keys[ac.length-1].time + "," + ac.keys[ac.length-1].value);
			
			firstKey.value = newValue;
			secondKey.value = newValue;
			
			ac.RemoveKey(0);
			ac.RemoveKey(ac.length-1);
			ac.AddKey(firstKey);
			ac.AddKey(secondKey);
			
			Debug.Log("Clean Animation : new values " + ac.keys[0].time + ":" + ac.keys[0].value + "," + ac.keys[ac.length-1].time + "," + ac.keys[ac.length-1].value);

			if (matchBeginEndKeys) anim.SetCurve(accd.path, accd.type, accd.propertyName, ac);
			else anim.SetCurve(accd.path, accd.type, accd.propertyName, accd.curve);
			
			//Debug.Log(i + " curve now has " + ac.length + " keys");
		}
		//AnimationUtility.SetAnimationClips(anim, origCurves);
    }  
    void OnWizardUpdate () {
		
        helpString = "clean frames in animation clip";
    }   
    // When the user pressed the "New Mesh" button OnWizardOtherButton is called.
    void OnWizardOtherButton () {
        
	}
	
}