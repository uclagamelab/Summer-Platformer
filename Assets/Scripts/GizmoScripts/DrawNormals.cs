using UnityEngine;
using System.Collections;

public class DrawNormals : MonoBehaviour {
	
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnDrawGizmosSelected () {
		MeshFilter mf = GetComponent("MeshFilter") as MeshFilter;
		Mesh mesh = mf.sharedMesh;
		Vector3[] normals = mesh.normals;
		Vector3[] vertices = mesh.vertices;
		for (int i = 0; i < normals.Length; i++) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position+vertices[i], transform.position+vertices[i] + (normals[i] * 0.25f));
		}
	}
}
