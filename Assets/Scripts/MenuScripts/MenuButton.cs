/* 

MenuButton keeps track of a series of states for a button

TODO: 
	-add audio effects.
	-make button select time less than button move time.

*/

using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	public Material normalMaterial;
	public Material highlightMaterial;
	private Material activeMaterial;
	//public bool setActiveOnMenuStart;
	
	private Renderer buttonRenderer;
	
	public GameObject upPressButton;
	public GameObject downPressButton;
	public GameObject leftPressButton;
	public GameObject rightPressButton;
	
	private MenuButton upPressMenuButton;
	private MenuButton downPressMenuButton;
	private MenuButton leftPressMenuButton;
	private MenuButton rightPressMenuButton;
	
	private bool buttonIsHighlighted = false;
	private float horizontalValue = 0.0f;
	private float verticalValue = 0.0f;
	private bool activateButton = false;
	
	public GameObject activateButtonObject;
	
	private MenuController activateMenuController;
	private LevelLoader startNewGame;
	
	private bool buttonReleased = false;
	
	public float buttonReleaseIncrement = 0.3f;
	private float buttonReleaseTime = 0.0f;
	
	// awake is used because materials need to be set before the menu controller takes over
	void Awake () {
		// check values
		if (highlightMaterial == null) Debug.LogWarning("MenuButton: missing texture for highlight button state. Highlight will not be visible.");
		if (normalMaterial == null) Debug.LogWarning("MenuButton: missing texture for normal button state. Button will not be visible.");
		
		// store the renderer for later
		buttonRenderer = (Renderer) gameObject.GetComponent("Renderer");
		if (buttonRenderer == null) {
			Debug.LogWarning("MenuButton: renderer not found on object, button will not be visible.");
		}
		
		// set the current state
		activeMaterial = normalMaterial;
		buttonRenderer.material = normalMaterial;
		
		// get components for menu buttons
		if (upPressButton != null) {
			upPressMenuButton = (MenuButton) upPressButton.gameObject.GetComponent("MenuButton");
			if (upPressMenuButton == null) {
				Debug.LogWarning("MenuButton: object attached to upPressButton doens't have a MenuButton component. The world may end, so add one in the inspector.");
			}
		}
		
		if (downPressButton != null) {
			downPressMenuButton = (MenuButton) downPressButton.gameObject.GetComponent("MenuButton");
			if (downPressMenuButton == null) {
				Debug.LogWarning("MenuButton: object attached to downPressButton doens't have a MenuButton component. The world may end, so add one in the inspector.");
			}
		}
		
		if (rightPressButton != null) {
			rightPressMenuButton = (MenuButton) rightPressButton.gameObject.GetComponent("MenuButton");
			if (rightPressMenuButton == null) {
				Debug.LogWarning("MenuButton: object attached to rightPressButton doens't have a MenuButton component. The world may end, so add one in the inspector.");
			}
		}
		
		if (leftPressButton != null) {
			leftPressMenuButton = (MenuButton) leftPressButton.gameObject.GetComponent("MenuButton");
			if (leftPressMenuButton == null) {
				Debug.LogWarning("MenuButton: object attached to leftPressButton doens't have a MenuButton component. The world may end, so add one in the inspector.");
			}
		}
		
		// determine how the button activation reponds
		if (activateButtonObject != null) {
			activateMenuController = (MenuController) activateButtonObject.GetComponent("MenuController");
			if (activateMenuController == null) {
				startNewGame = (LevelLoader) activateButtonObject.GetComponent("LevelLoader");
				if (startNewGame == null) {
					Debug.LogWarning(gameObject.name + ": MenuButton: couldn't find a 'LevelLoader' or 'MenuController' component for the activateButtonObject " + activateButtonObject.gameObject.name);
				}
			}
		}
		else {
			Debug.LogWarning(gameObject.name + "MenuButton: activateButtonObject not set. Button will not do anything when activated.");
		}
		buttonReleaseTime = 0.0f;
		buttonReleased = true;
	}
	
	void Update() {
		horizontalValue = 0.0f;
		verticalValue = 0.0f;
		if (buttonIsHighlighted) {
			horizontalValue = Input.GetAxis("Horizontal");
			verticalValue = Input.GetAxis("Vertical");
			if (Input.GetAxis("Jump") > 0.0f) {
				if (buttonReleased) activateButton = true;
			}
			else if (Input.GetKey(KeyCode.Return)) {
				if (buttonReleased) activateButton = true;
			}

		}
		if (!buttonReleased && Time.time - buttonReleaseTime > buttonReleaseIncrement) {
			buttonReleased = true;
		}
	}
	
	// do switching of the buttons on LateUpdate to avoid multiple click throughs
	void LateUpdate() {
		if (buttonIsHighlighted && (horizontalValue != 0.0f || verticalValue != 0.0f) && buttonReleased) {
			//float maxValue = 0.0f;
			if (Mathf.Abs(verticalValue) < Mathf.Abs(horizontalValue) ) {
				if (horizontalValue < 0.0f && leftPressMenuButton != null) {
					buttonIsHighlighted = false;
					activateButton = false;
					SwitchTexture(); // deactivate state
					leftPressMenuButton.HighlightButton();
					//Debug.Log("activate left");
				}
				else if (horizontalValue > 0.0f && rightPressMenuButton != null) {
					buttonIsHighlighted = false;
					activateButton = false;
					SwitchTexture();
					rightPressMenuButton.HighlightButton();
					//Debug.Log("activate right");
				}
			}
			else {
			if (verticalValue > 0.0f && upPressMenuButton != null) {
					buttonIsHighlighted = false;
					activateButton = false;
					SwitchTexture(); // deactivate state
					upPressMenuButton.HighlightButton();
					//Debug.Log("activate up");
				}
				else if (verticalValue < 0.0f && downPressMenuButton != null) {
					buttonIsHighlighted = false;
					activateButton = false;
					SwitchTexture();
					downPressMenuButton.HighlightButton();
					//Debug.Log("activate down");
				}
			}
		}
		else if (buttonIsHighlighted && activateButton) {
			Debug.Log(gameObject.name + ": MenuButton: activate button");
			if (activateMenuController != null) {
				activateButton = false;
				buttonIsHighlighted = false;
				SwitchTexture();
				activateMenuController.ActivateMenu();
				Debug.Log(gameObject.name + ": MenuButton: activate menu controller " + activateMenuController.gameObject.name);
			}
			else if (startNewGame != null) {
				activateButton = false;
				buttonIsHighlighted = false;
				startNewGame.StartLevel();
				Debug.Log(gameObject.name + ": MenuButton: start a new game.");
			}
			else {
				activateButton = false;
				Debug.LogWarning(gameObject.name + ": MenuButton: no button activation behavior defined");
			}
		}
	}
	
	public void HighlightButton() {
		buttonIsHighlighted = true;
		SwitchTexture();
		buttonReleased = false;
		buttonReleaseTime = Time.time;
		//Debug.Log("highlight button " + gameObject.name);
	}
	
	public void SwitchTexture() {
		if (activeMaterial == normalMaterial) {
			buttonRenderer.material = highlightMaterial;
			activeMaterial = highlightMaterial;
			//Debug.Log("MeshButton: switch to highlight material " + gameObject.name);
		}
		else {
			buttonRenderer.material = normalMaterial;
			activeMaterial = normalMaterial;
		}
	}
}
