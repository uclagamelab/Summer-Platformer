using UnityEditor;
using UnityEngine;
using System.Collections;

class FillBonesWithCubes : ScriptableWizard {
    // default values
	public GameObject root;
	public float minWidth = 0.05f;
	public float maxWidth = 0.5f;
	public Mesh boneMesh;
	public Material boneMaterial;
    
	Transform lastTransform;
	
    [MenuItem ("Scripts/Fill Bones with Cubes")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard<FillBonesWithCubes>("Fill Bones with Cubes", "Fill");
    }
	
	// this corresponds with the Fill button
    void OnWizardCreate () {
		AttachedCubeToChildren(root);
    }  
	
	// this does the recursive attaching
	void AttachedCubeToChildren(GameObject parentGameObject) {
		Transform rootTransform = parentGameObject.transform;
		//~ foreach( Transform child in rootTransform) {
			//~ AttachedCubeToChildren(child.gameObject);
		//~ }
		
		Transform[] allChildren = new Transform[rootTransform.childCount];
		int count = 0;
		foreach (Transform child in rootTransform) {
			allChildren[count] = child;
			count++;
		}
		
		foreach( Transform child in allChildren) {
			//if (child.gameObject.name == "bone") continue;
			Transform otherTransform = child;
			Vector3 center = rootTransform.position - otherTransform.position;
			center /= -2.0f;
			GameObject newCube = new GameObject("bone");
			newCube.transform.parent = rootTransform;
			newCube.transform.position = rootTransform.position;
			
			newCube.transform.Translate(center);
			newCube.transform.rotation = rootTransform.rotation;
			MeshRenderer mr = newCube.AddComponent("MeshRenderer") as MeshRenderer;
			MeshFilter mf = newCube.AddComponent("MeshFilter") as MeshFilter;
			mf.sharedMesh = boneMesh;
			mr.material = boneMaterial;
			float distance = Vector3.Distance(otherTransform.position, rootTransform.position);
			newCube.transform.localScale = new Vector3(  distance, 0.2f, 0.2f);
			AttachedCubeToChildren(otherTransform.gameObject);
			//newCube.transform.parent = rootTransform;

		}
	}
	
    void OnWizardUpdate () {
		
        helpString = "fill bones between nodes in selected game object";
    }
	
    // When the user pressed the "New Mesh" button OnWizardOtherButton is called.
    void OnWizardOtherButton () {
        
	}
	
}