using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Deals with player controls and what is shown to the player.
/// </summary>
public class UiControllerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector
	[SerializeField]
	private Text missesTextObject = null;

	[SerializeField]
	private Text interceptTextObject = null;

	[SerializeField]
	private Text interceptProjectileSpeedTextObject = null;

	[SerializeField]
	private Text targetProjectileSpeedTextObject = null;

	[SerializeField]
	private Text interceptTimeTextObject = null;
	#endregion

	#region Variable Declarations
	private string missesText = "Misses: ";
	private string interceptText = "Intercepts: ";
	private string interceptProjectileSpeedText = "Interceptor Speed: ";
	private string targetProjectileSpeedText = "Target Speed: ";
	private string interceptTimeText = "Intercept Time(s): ";
	private float targetProjectileSpeed = 0.0f;
	private float interceptProjectileSpeed = 0.0f;
	private InterceptorTurretScript turretScript = null;
	private List<GameObject> targetProjectileList;
	private List<GameObject> interceptorProjectileList;
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		//Initialize text objects text with base values.
		turretScript = GameObject.FindGameObjectsWithTag("Interceptor")[0].GetComponent<InterceptorTurretScript>();
		missesTextObject.text = missesText + ProjectileScript.GetMissCounter();
		interceptTextObject.text = interceptText + ProjectileScript.GetInterceptCounter();
		interceptProjectileSpeedTextObject.text = interceptProjectileSpeedText + 0.0f + " m/s";
		targetProjectileSpeedTextObject.text = targetProjectileSpeedText + 0.0f + " m/s";
		interceptTimeTextObject.text = interceptTimeText + turretScript.GetInterceptTime();
	}

	// Update is called once per frame
	void Update() {
		GetProjectileLists();
		UpdateProjectileSpeeds();
		DisplayText();
		ExitGame();
	}

	/// <summary>
	/// Get the speed of the current projectiles and update the text objects based on what are in the scene.
	/// </summary>
	private void UpdateProjectileSpeeds() {
		if (ProjectileScript.GetTargetProjectileListCount() > 0 && ProjectileScript.GetInterceptProjectileListCount() > 0) {
			targetProjectileSpeed = targetProjectileList[0].GetComponent<ProjectileScript>().GetProjectileSpeed();
			interceptProjectileSpeed = interceptorProjectileList[0].GetComponent<ProjectileScript>().GetProjectileSpeed();
		} else {
			targetProjectileSpeed = 0.0f;
			interceptProjectileSpeed = 0.0f;
		}

	}

	/// <summary>
	/// Define the projectile list variables to what the current projectile lists are.
	/// </summary>
	private void GetProjectileLists() {
		targetProjectileList = ProjectileScript.GetTargetProjectileList();
		interceptorProjectileList = ProjectileScript.GetInterceptorProjectileList();
	}

	private void DisplayText() {
		//Check if the misses text object has changed.
		if (missesTextObject.text != (missesText + ProjectileScript.GetMissCounter())) {
			//Update it with the new value.
			missesTextObject.text = missesText + ProjectileScript.GetMissCounter();
		}

		//Check if the intercepts text object has changed
		if (interceptTextObject.text != (interceptText + ProjectileScript.GetInterceptCounter())) {
			//Update it with the new value.
			interceptTextObject.text = interceptText + ProjectileScript.GetInterceptCounter();
		}

		//Check if the intercepts speed text object has changed
		if (interceptProjectileSpeedTextObject.text != (interceptProjectileSpeedText + interceptProjectileSpeed)) {
			//Update it with the new value.
			interceptProjectileSpeedTextObject.text = interceptProjectileSpeedText + interceptProjectileSpeed + " m/s";
		}

		//Check if the targets speed text object has changed
		if (targetProjectileSpeedTextObject.text != (targetProjectileSpeedText + targetProjectileSpeed)) {
			//Update it with the new value.
			targetProjectileSpeedTextObject.text = targetProjectileSpeedText + targetProjectileSpeed + " m/s";
		}

		//Check if the intercept time text object has changed
		if(interceptTimeTextObject.text != (interceptTimeText + turretScript.GetInterceptTime())) {
			//Update it with the new value.
			interceptTimeTextObject.text = interceptTimeText + turretScript.GetInterceptTime();
		}
	}

	/// <summary>
	/// Exit the game if the player presses 'escape'.
	/// </summary>
	private void ExitGame() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
			Debug.Log("Quit game button pressed.");
		}
	}
	#endregion
}
