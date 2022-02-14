using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the player movement script for my project. Used in conjunction with the "MouseLook.cs" Script to form the character controller.
/// It is using the brackey's character controller as a base and my additions to it are as follows:
/// </summary>
public class PlayerMovement : MonoBehaviour {
	[SerializeField]
	private CharacterController controller = null;//Used to manipulate the character controller.

	[SerializeField]
	private float speed = 12.0f;//Used to determine how fast the user will move.

	//variable declarations.
	//The three public variables below are public because I tested the game and when each one of them was private the game broke.
	//My theory is that each of these variables are being used in "isGrounded" and being passed to that function
	//and unity does not like that because they were private and shouldn't be accessed by functions.
	//I am puzzled though because it was my belief it was ok to pass private variable as parameters.

	private Vector3 velocity;
	private bool isGrounded;
	public static float x;
	public static float z;

	// Update is called once per frame
	void Update() {
		x = Input.GetAxis("Horizontal");
		z = Input.GetAxis("Vertical");

		//The two lines of code below take the x and z variables and place them into a vector called move than then is passed to the controller.move function to move the player.
		//The value passed in is multiplied by the speed variable and is then multiplied by Time.deltaTime to make sure movement speed is independent from the framerate.
		Vector3 move = transform.right * x + transform.forward * z;
		controller.Move(move * speed * Time.deltaTime);
	}
}
