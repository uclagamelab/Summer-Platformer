
@MenuItem("Scripts/Mass Set Materials")
static function MassSetMaterials() {
    Undo.RegisterSceneUndo("Mass Set Materials");

    var mats : Material[] = Selection.activeGameObject.renderer.sharedMaterials;

    for (var obj : GameObject in Selection.gameObjects) {
        obj.renderer.sharedMaterials = mats;
    }
}