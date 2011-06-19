/*

MenuController moves camera to properly look at a menu
and assigns a default highlighted button on activation.

TODO: add option for animation between each menu.

*/


using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
	
	public GameObject defaultButton;
	private MenuButton defaultButtonComponent;

	public bool activateMenuOnStart;
	
	private Transform cameraPostion;
		
	// Use this for initialization
	void Start () {
		if (defaultButton != null) {
			defaultButtonComponent = (MenuButton) defaultButton.GetComponent("MenuButton");
		}
		else {
			Debug.LogWarning(gameObject.name + ": MenuController: defaultButton not set, menu will get confused unless set.");
		}
		
		// store the camera position
		foreach (Transform child in transform) {
			if (child.gameObject.name == "CameraPosition") {
				cameraPostion = child;
			}
		}
		
		if (cameraPostion == null) {
			Debug.LogWarning(gameObject.name + ": MenuController: no cameraPostion object set, please name a game object to 'CameraPosition' and the correct transform will be set");
		}
		
		// activate this menu if necessary
		if (activateMenuOnStart) {
			ActivateMenu();
		}
	}
	
	public void ActivateMenu() {
		// set the state of the default button
		if (defaultButtonComponent != null) {
			defaultButtonComponent.HighlightButton();
			SetCameraToMenu();
		}
		else {
			Debug.LogError(gameObject.name + ": MenuController: defaultButtonComponent not set, camera won't where to go when menu is activated.");
		}
	}
	
	void SetCameraToMenu() {
		GameObject cam = GameObject.Find("Main Camera");
		cam.transform.position = cameraPostion.position;
	}
	
}
