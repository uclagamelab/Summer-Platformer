using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {
	
	public string levelName;
	public GameObject playerPrefab;
	
	public bool killPlayerOnThisScreen = true;
	
	private Object thePlayer;
	
	public void Start() {
		// kill the player on this screen
		if (killPlayerOnThisScreen == true) {
			GameObject player = GameObject.Find("PlayerAndGUI(Clone)");
			if (player != null) {
				Destroy(player);
			}
			else {
				Debug.LogWarning(gameObject.name + ": LevelLoader: told to destroy player, but no player found.");
			}
		}
	}
	
	public void OnLevelWasLoaded()  {
		// kill the player on this screen
		//~ if (killPlayerOnThisScreen == true) {
			//~ GameObject player = GameObject.Find("PlayerAndGUI(Clone)");
			//~ if (player != null) {
				//~ Destroy(player);
			//~ }
			//~ else {
				//~ Debug.LogWarning(gameObject.name + ": LevelLoader: told to destroy player, but no player found.");
			//~ }
		//~ }
	}
	
	public void StartLevel() {
		// instantiate the player for a fresh new game
		if (playerPrefab != null) thePlayer = (Object) Instantiate(playerPrefab, new Vector3(100,-10,100), Quaternion.identity);
		Application.LoadLevel(levelName);
	}
}
