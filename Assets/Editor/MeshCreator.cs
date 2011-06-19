using UnityEditor;
using UnityEngine;
using System.Collections;

class MeshCreator : ScriptableWizard {
    // default values
	public Texture2D outlineTexture; 
	public Material meshMaterial;  
	public float meshHeight; 
	public float meshWidth; 
	public float heightOffset; 
	public float widthOffset; 
	public float meshDepth; 
	public bool generateCollider;
	
	bool setInitialValues = false;
    
    [MenuItem ("GameObject/Mesh creator")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard<MeshCreator>("Create Mesh", "Update Mesh", "New Mesh");
    }
	
	// this corresponds with the Update Mesh button
    void OnWizardCreate () {
		GameObject activeObject = Selection.activeGameObject;
		if (activeObject != null) { // check if an object is selected
			
			// get the main texture image from the selected object
			// this should probably look for a special script component that stores import settings
			MeshCreatorData mcd = (MeshCreatorData) activeObject.GetComponent("MeshCreatorData");
			if (mcd == null) {
				Debug.LogError("MeshCreator Error: selected object does not have a MeshCreatorData component. Select an object with a MeshCreatorData component to update."); // TODO: add instructions on how to fix
				return;
			}
			
			// add a TextureImporter object here to check whether texture is readable
			// set it to readable if necessary
			
			if (outlineTexture == null) {
				Debug.LogError("MeshCreator Error: no texture found. Make sure to have a texture selected before updating mesh.");
				return;
			}
			
			Mesh msh = GetMesh();

        
			// Set up game object with mesh;
		
			// TODO: add only if not already there, should be there
			MeshRenderer mr = (MeshRenderer) activeObject.GetComponent("MeshRenderer");
			if (mr == null) {
				Debug.LogWarning("MeshCreator Warning: no mesh renderer found on update object, adding one.");
				Selection.activeGameObject.AddComponent(typeof(MeshRenderer));
			}
			
			// update the front material via renderer
			activeObject.renderer.sharedMaterial = meshMaterial;
			
			MeshFilter mf = (MeshFilter) activeObject.GetComponent("MeshFilter");
			if (mf == null) {
				Debug.LogWarning("MeshCreator Warning: no mesh filter found on update object, adding one.");
				mf= Selection.activeGameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
			}
			mf.mesh = msh;
			
			// generate a mesh collider
			if (generateCollider) {
				Collider col = activeObject.collider;
				if (col == null) {
					activeObject.AddComponent(typeof(MeshCollider));
				}
				MeshCollider mcol = activeObject.GetComponent("MeshCollider") as MeshCollider;
				if (mcol == null) {
					Debug.LogWarning("MeshCreator Warning: found a non-Mesh collider on object to update. If you really want a new collider generated, remove the old one and update the object with MeshCreator again.");
				}
				else {
					mcol.sharedMesh = msh;
				}
			}
			
			// update the MeshCreatorData component
			mcd.outlineTexture = outlineTexture;
			mcd.frontMaterial = meshMaterial; 
			mcd.meshHeight = meshHeight;
			mcd.meshWidth = meshWidth;
			mcd.heightOffset = heightOffset;
			mcd.widthOffset = widthOffset;
			mcd.meshDepth = meshDepth;
			mcd.generateCollider = generateCollider;
		}
		else { // here if no object is selected
			Debug.LogError("MeshCreator Error: no object selected to update. Select an object with a MeshCreatorData component to update, or select an empty game object to create a new mesh."); 
		}
		setInitialValues = false;
    }  
    void OnWizardUpdate () {
		GameObject activeObject = Selection.activeGameObject;
		string messageString = helpString;
		if (activeObject != null) {
			if (setInitialValues == false) {
				MeshCreatorData mcd = (MeshCreatorData) activeObject.GetComponent("MeshCreatorData");
				if (mcd == null) { // no MeshCreatorData, so set some default values
					messageString = "The selected object does not have mesh creator data, the following parameters will be used to create a new mesh."; 
					outlineTexture = (Texture2D) Resources.LoadAssetAtPath("Assets/Textures/defaultMeshCreatorTexture.png", typeof(Texture2D));
					meshMaterial = (Material) Resources.LoadAssetAtPath("Assets/Materials/defaultMeshCreatorMaterial.mat", typeof(Material)) ; 
					meshHeight = 1.0f;
					meshWidth = 1.0f;
					heightOffset = 0.5f;
					widthOffset = 0.0f;
					meshDepth = 0.1f;
					generateCollider = true;
				} else { // set the wizard values to MeshCreatorData value
					messageString = "The selected object will be updated with the following values.";
					outlineTexture = mcd.outlineTexture;
					meshMaterial = mcd.frontMaterial; 
					meshHeight = mcd.meshHeight;
					meshWidth = mcd.meshWidth;
					heightOffset = mcd.heightOffset;
					widthOffset = mcd.widthOffset;
					meshDepth = mcd.meshDepth;
					generateCollider = mcd.generateCollider;
				}
				setInitialValues = true;
			}
		}
		else {
			messageString = "no object selected!!!!!!";
		}
		//Debug.Log("Wizard update");
        helpString = messageString;
    }   
    // When the user pressed the "New Mesh" button OnWizardOtherButton is called.
    void OnWizardOtherButton () {
        GameObject activeObject = Selection.activeGameObject;
		if (activeObject != null) { // check if an object is selected
			
			// get the main texture image from the selected object
			// this should probably look for a special script component that stores import settings
			MeshCreatorData mcd = (MeshCreatorData) activeObject.GetComponent("MeshCreatorData");
			if (mcd == null) { // probably good, if a empty game object was selected
				mcd = activeObject.AddComponent(typeof(MeshCreatorData)) as MeshCreatorData;
			}
			else {
				Debug.LogError("MeshCreator Error: selected object already has MeshCreatorData component. Use 'update mesh' button to update the mesh instead of create new mesh."); 
				return;
			}
			
			// add a TextureImporter object here to check whether texture is readable
			// set it to readable if necessary
			
			if (outlineTexture == null) {
				Debug.LogError("MeshCreator Error: no texture found. Make sure to have a texture selected before creating mesh.");
				return;
			}
			
			Mesh msh = GetMesh();
        
			// Set up game object with mesh;
			MeshRenderer mr = (MeshRenderer) activeObject.GetComponent("MeshRenderer");
			if (mr == null) {
				Selection.activeGameObject.AddComponent(typeof(MeshRenderer));
			}
			else {
				Debug.LogWarning("MeshCreator Warning: previous mesh renderer found on object while creating new mesh. Won't create new mesh renderer, but will change material.");
			}
			
			// update the front material via renderer
			activeObject.renderer.sharedMaterial = meshMaterial;
			
			MeshFilter mf = (MeshFilter) activeObject.GetComponent("MeshFilter");
			if (mf == null) {
				mf= Selection.activeGameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
			}
			else {
				Debug.LogWarning("MeshCreator Warning: previous mesh filter found on object while creating new mesh. Old mesh is being destroyed.");
			}
			mf.mesh = msh;
			
			// generate a mesh collider
			if (generateCollider) {
				Collider col = activeObject.collider;
				if (col == null) {
					activeObject.AddComponent(typeof(MeshCollider));
					MeshCollider mcol = activeObject.GetComponent("MeshCollider") as MeshCollider;
					mcol.sharedMesh = msh;
				}
				else {
					Debug.LogWarning("MeshCreator Warning: existing collider on selected object and generateCollider was specified. Update mesh to replace old collider with new one based on mesh.");
				}
				
			}
			
			// update the MeshCreatorData component
			mcd.outlineTexture = outlineTexture;
			mcd.frontMaterial = meshMaterial; 
			mcd.meshHeight = meshHeight;
			mcd.meshWidth = meshWidth;
			mcd.heightOffset = heightOffset;
			mcd.widthOffset = widthOffset;
			mcd.meshDepth = meshDepth;
		}
		else { // here if no object is selected
			Debug.LogError("MeshCreator Error: no object selected to generate mesh onto. Select an empty game object to create a new mesh."); 
		}
		setInitialValues = false;
	}
	/*
	*	GetMesh() does calculation of mesh from the raster image.
	*/ 
	public Mesh GetMesh() {
			string path = AssetDatabase.GetAssetPath(outlineTexture);
			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
			textureImporter.isReadable = true;
			AssetDatabase.ImportAsset(path);
			
			//Debug.Log("found texture " + outlineTexture.width + "," + outlineTexture.height);
			Color[] pixels = outlineTexture.GetPixels();	// get the pixels to build the mesh from
			//Debug.Log("total pixel count " + pixels.Length);
			
			// possibly do some size checking
			// TODO: check for a square power of two
			int imageHeight = outlineTexture.height;
			int imageWidth = outlineTexture.width;
			if ( ((float)imageWidth)/((float)imageHeight) != meshWidth/meshHeight) {
				Debug.LogWarning("Mesh Creator Warning: selected meshWidth and meshHeight is not the same proportion as source image width and height. Results may be distorted.");
			}
			
			// make a surface object to create and store data from image
			MC_SimpleSurfaceEdge mcs = new MC_SimpleSurfaceEdge(pixels,  imageWidth, imageHeight);
			
			// need a list of ordered 2d points
			Vector2 [] vertices2D = mcs.GetOutsideEdgeVertices();
        
			// Use the triangulator to get indices for creating triangles
			Triangulator tr = new Triangulator(vertices2D);
			int[] indices = tr.Triangulate(); // these will be reversed for the back side
			Vector2[] uvs = new Vector2[vertices2D.Length * 4];
			// Create the Vector3 vertices
			Vector3[] vertices = new Vector3[vertices2D.Length * 4];
			//Vector3[] verticesBack = new Vector3[vertices2D.Length];
			
			float halfDepth = meshDepth/2.0f;
			Debug.Log("mesh depth set to " + halfDepth);
			for (int i=0; i<vertices2D.Length; i++) {
				float vertX = vertices2D[i].x/imageWidth ; // get X point and normalize
				float vertY = vertices2D[i].y/imageHeight ; // get Y point and normalize
				vertX = (vertX * meshWidth) - (meshWidth / 2.0f);  // scale X and position centered
				vertY = (vertY * meshHeight) - (meshHeight / 2.0f);
				vertX = vertX + widthOffset;
				vertY = vertY + heightOffset;
				vertices[i] = new Vector3(vertX, vertY, -halfDepth);
				vertices[i + vertices2D.Length] = new Vector3(vertX, vertY, halfDepth);
				vertices[i+(vertices2D.Length*2)] = new Vector3(vertX, vertY, -halfDepth); // vertex for side
				vertices[i +(vertices2D.Length*3)] = new Vector3(vertX, vertY, halfDepth);
				uvs[i] = mcs.GetUVForIndex(i);
				uvs[i+vertices2D.Length] = uvs[i];
				uvs[i+(vertices2D.Length*2)] = uvs[i];
				uvs[i+(vertices2D.Length*3)] = uvs[i];
				//Debug.Log("make vertex at " + vertX + "," + vertY + " with uv " + uvs[i].x + "," + uvs[i].y); 
			}
			
			// make the back side triangle indices
			// double the indices for front and back, 6 times the number of edges on front
			int[] allIndices = new int[(indices.Length*2) + ( (vertices2D.Length ) * 6)];
			
			// copy over the front and back index data
			for (int i = 0; i < indices.Length; i++) {
				allIndices[i] = indices[i]; // front side uses normal indices returned from the algorithm
				allIndices[(indices.Length*2) - i -1] = indices[i] + vertices2D.Length; // backside reverses the order
			}
			
			// create the side triangle indices
			// for each edge, create a new set of two triangles
			// edges are just two points from the original set
			for (int i = 0; i < vertices2D.Length - 1; i++) {
				allIndices[(indices.Length*2) + (6 * i)] = (vertices2D.Length *2) + i + 1;
				allIndices[(indices.Length*2) + (6 * i) + 1] = (vertices2D.Length *2) +i ;
				allIndices[(indices.Length*2) + (6 * i) + 2] = (vertices2D.Length *2) + i + 1 + vertices2D.Length;
				allIndices[(indices.Length*2) + (6 * i) + 3] = (vertices2D.Length *2) + i + 1 + vertices2D.Length;
				allIndices[(indices.Length*2) + (6 * i) + 4] = (vertices2D.Length *2) + i ;
				allIndices[(indices.Length*2) + (6 * i) + 5] = (vertices2D.Length *2) + i + vertices2D.Length;
			}
			
			// wrap around for the last face
			allIndices[allIndices.Length-6] = (vertices2D.Length *2) + 0;
			allIndices[allIndices.Length-5] = (vertices2D.Length *2) +vertices2D.Length-1;
			allIndices[allIndices.Length-4] = (vertices2D.Length *2) +vertices2D.Length;
			allIndices[allIndices.Length-3] = (vertices2D.Length *2) +vertices2D.Length;
			allIndices[allIndices.Length-2] = (vertices2D.Length *2) +vertices2D.Length-1;
			allIndices[allIndices.Length-1] = (vertices2D.Length *2) + (vertices2D.Length*2) - 1;
		
			// Create the mesh
			Mesh msh = new Mesh();
			msh.vertices = vertices;
			msh.triangles = allIndices;
			msh.uv = uvs;
			msh.RecalculateNormals();
			msh.RecalculateBounds();
			msh.name = outlineTexture.name + ".mesh";
			return msh;
	}
}