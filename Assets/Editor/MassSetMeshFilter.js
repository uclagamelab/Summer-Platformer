
@MenuItem("Scripts/Mass Set Mesh Filter")
static function MassSetMeshFilter() {
    Undo.RegisterSceneUndo("Mass Set Mesh Filter");

    //var mats : Material[] = Selection.activeGameObject.renderer.sharedMaterials;
	
	var mesh : Mesh = Selection.activeGameObject.GetComponent("MeshFilter").mesh;

    for (var obj : GameObject in Selection.gameObjects) {
        obj.GetComponent("MeshFilter").sharedMesh = mesh;
    }
}