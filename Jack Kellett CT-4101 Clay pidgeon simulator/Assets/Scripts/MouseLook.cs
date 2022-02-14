using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the MouseLook script that allows the player's mouse to move.
/// The basic script allowing the user to look around themselves was made using brackey's first person controller tutorial.
/// The additions I have made are as follows:
/// 1. Commenting.
/// 2. Changing mouseSensitivity and playerBody to private and using the serializefield on them to preserve encapsulation.
/// </summary>
public class MouseLook : MonoBehaviour {

	[SerializeField]
	private float mouseSensitivity = 100.0f;

	[SerializeField]
	private Transform playerBody = null;

	private float xRotation = 0.0f;
	public static float mouseX = 0.0f;
	public static float mouseY = 0.0f;

	// Start is called before the first frame update
	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update() {
		//The variable mouseX and mouseY get the mouse input, times it by the mouse sensitivity and time.deltatime to get how much the camera should turn by.
		mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		//xRotation limits the amounnt the player can look up or down to 90 degrees in both directions.
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

		//This actually moves the player characters camera and movement.
		transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
		playerBody.Rotate(Vector3.up * mouseX);
	}
}