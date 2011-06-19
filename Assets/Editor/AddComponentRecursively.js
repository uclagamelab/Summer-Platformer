class AddComponentRecursively extends ScriptableWizard {

   
    var componentName : String = "";
   
    @MenuItem ("GameObject/Add Component Recursively...")
   
    static function AddComponentsRecursivelyItem() {
        ScriptableWizard.DisplayWizard("Add Component Recursively", AddComponentRecursively, "Add", "");
    }
   
    //Main function
    function OnWizardCreate() {
        var total : int = 0;
        for (var currentTransform : Transform in Selection.transforms) {
          total += RecurseAndAdd(currentTransform, componentName);
        }
        if (total == 0)
            Debug.Log("No components added.");
        else
            Debug.Log(total + " components of type \"" + componentName + "\" created.");
    }
   
    function RecurseAndAdd(parent : Transform, componentToAdd : String) : int {
        //keep count
        var total : int = 0;
        //add components to children
        for (var child : Transform in parent) {
            total += RecurseAndAdd(child, componentToAdd);
        }
        //add component to parent
        var existingComponent : Component = parent.GetComponent(componentToAdd);
        if (!existingComponent) {
            parent.gameObject.AddComponent(componentToAdd);
            total++;
        }
       
        return total;
    }
    //Set the help string
    function OnWizardUpdate () { 
        helpString = "Specify the exact name of the component you wish to add:";
    }
   
    // The menu item will be disabled if no transform is selected.
    @MenuItem ("GameObject/Add Component Recursively...", true)

    static function ValidateMenuItem() : boolean {
       return Selection.activeTransform;
    }
}