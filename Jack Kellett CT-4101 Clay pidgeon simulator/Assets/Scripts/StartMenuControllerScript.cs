using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Allows player to load the simulation or close the application.
/// </summary>
public class StartMenuControllerScript : MonoBehaviour {
	private void Start() {
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	// Update is called once per frame
	void Update() {
		//Load next scene if player presses space.
		if (Input.GetKeyDown(KeyCode.Space)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		//Exit Game if player presses escape.
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
