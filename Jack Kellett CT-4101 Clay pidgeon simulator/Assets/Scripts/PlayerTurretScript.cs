using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls all the aiming and firings for the player turret.
/// </summary>
public class PlayerTurretScript : MonoBehaviour {
	#region Variables to assign via the unity inspector
	[SerializeField]
	private GameObject ProjectilePrefab = null;

	[SerializeField]
	private float coolDownTime = 5.0f;//How much time there should be between shots.

	[SerializeField]
	private GameObject audioHolderObject = null;
	#endregion

	#region Variable Declarations
	public static Vec3 vectorToFireIn;
	private Vec3 rotationOfTurret;
	private static bool projectileCooldown = true;
	private static bool projectileFired = false;
	private static int counter = 0;

	//The three floats below are cache variables to help generate the random numbers
	private float xValueCache = 0.0f;
	private float yValueCache = 0.0f;
	private float zValueCache = 0.0f;
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		StartCoroutine("ProjectileCooldown");

		//Initialize the turret rotation variable.
		rotationOfTurret = new Vec3(0.0f, 0.0f, 0.0f);
	}

	// Update is called once per frame
	void Update() {
		AimAndFireProjectile();
	}

	/// <summary>
	/// Gets the vector to fire in and converts it
	/// to pitch and yaw using trigonometry.
	/// </summary>
	private void TurnShooterTurret() {
		//Declare the rotation variables.
		float xRotation = 0.0f;
		float yRotation = 0.0f;
		float xzPlane = 0.0f;

		if (vectorToFireIn.x < 0.0f || vectorToFireIn.z < 0.0f) {
			xzPlane = 0.0f - Mathf.Sqrt(vectorToFireIn.x * vectorToFireIn.x + vectorToFireIn.z * vectorToFireIn.z);
		} else {
			xzPlane = Mathf.Sqrt(vectorToFireIn.x * vectorToFireIn.x + vectorToFireIn.z * vectorToFireIn.z);
		}

		//Calculate yaw from the vector to fire in.
		yRotation = (Mathf.Rad2Deg * Mathf.Atan(vectorToFireIn.x / vectorToFireIn.z));

		//Calculate pitch from the vector to fire in
		xRotation = Mathf.Rad2Deg * Mathf.Atan(vectorToFireIn.y / xzPlane);

		//Set the rotation vec3 to the new value
		rotationOfTurret = new Vec3(xRotation, yRotation, 0.0f);
	}

	/// <summary>
	/// Generate a random vector then convert it to a unit vector
	/// and set the vector to fire in to that.
	/// </summary>
	private void GenerateVectorToFireIn() {
		//Generate Values
		float xValue = GenerateRandomNumber(-100.0f, 100.0f, xValueCache, false) * 12345;
		float yValue = GenerateRandomNumber(50.0f, 200.0f, yValueCache, true) + counter * 12345;
		float zValue = GenerateRandomNumber(2000.0f, 200000.0f, zValueCache, true) + counter * 12345;

		//Cache the vector components and introduce more noise when the iterations get above 45 times.
		if (counter < 45) {
			xValueCache = xValue;
			yValueCache = yValue;
			zValueCache = zValue;
		} else {
			xValueCache = (xValue - counter);
			yValueCache = (yValue - counter);
			zValueCache = (zValue - counter);
		}

		//Increment shot counter
		counter += 1;

		//Calculate the magnitude of the vector
		float vectorMagnitude = Mathf.Sqrt(xValue * xValue + yValue * yValue + zValue * zValue);

		//Calculate the unit vector components
		xValue = (xValue / vectorMagnitude);
		yValue = (yValue / vectorMagnitude);
		zValue = (zValue / vectorMagnitude);

		//Return those values as the vector to fire in
		vectorToFireIn = new Vec3(xValue, yValue, zValue);
	}

	/// <summary>
	/// Every coolDownTime seconds rotate the turret to the direction
	/// to fire in and shoot the projectile in that direction.
	/// </summary>
	private void AimAndFireProjectile() {
		projectileFired = false;
		if (!projectileCooldown) {
			//Set the rotation
			GenerateVectorToFireIn();
			TurnShooterTurret();
			UpdateTurretTransforms();

			//Set Cooldown flag
			projectileCooldown = true;
			StartCoroutine("ProjectileCooldown");

			//Turret Audio
			TurnOnTurretFireAudio();

			//Spawn Projectile
			Instantiate(ProjectilePrefab, this.transform);
			projectileFired = true;
		}
	}

	private IEnumerator ProjectileCooldown() {
		yield return new WaitForSeconds(coolDownTime);
		projectileCooldown = false;
	}

	/// <summary>
	/// Set the unity transform rotation
	/// to the custom Vec3 rotation.
	/// </summary>
	private void UpdateTurretTransforms() {
		transform.rotation = rotationOfTurret.ToQuartenion();
	}
	#endregion

	#region Turret Audio Functions
	private IEnumerator WaitForOneSecondThenTurnOffAudio() {
		yield return new WaitForSeconds(1.0f);

		//Turn off audio object.
		audioHolderObject.SetActive(false);
	}

	private void TurnOnTurretFireAudio() {
		audioHolderObject.SetActive(true);
		StartCoroutine("WaitForOneSecondThenTurnOffAudio");
	}
	#endregion

	#region Public Access Functions
	/// <summary>
	/// Return the vector to fire in.
	/// </summary>
	/// <returns></returns>
	public Vec3 GetVectorToFireIn() {
		GenerateVectorToFireIn();
		return vectorToFireIn;
	}

	/// <summary>
	/// Return the state of the projectile cool down.
	/// </summary>
	/// <returns></returns>
	public static bool GetProjectileCooldown() {
		return projectileCooldown;
	}

	/// <summary>
	/// Return the counter value.
	/// </summary>
	/// <returns></returns>
	public static int GetShotCounter() {
		return counter;
	}

	/// <summary>
	/// Return the state of the projectile fired variable.
	/// </summary>
	/// <returns></returns>
	public static bool GetProjectileFired() {
		return projectileFired;
	}

	/// <summary>
	/// Generate a random number between minValue and maxValue and return it.
	/// </summary>
	/// <param name="minValue"></param>
	/// <param name="maxValue"></param>
	/// <param name="cachedVariable"></param>
	/// <param name="positive"></param>
	/// <returns></returns>
	public static float GenerateRandomNumber(float minValue, float maxValue, float cachedVariable, bool positive) {
		//Declare the temp variable
		float noiseVariable;

		//Initialise it
		noiseVariable = Time.time + cachedVariable;

		//Generate the noise/randomness
		noiseVariable = GenerateNoiseInFloat(noiseVariable, positive);
		noiseVariable = ((noiseVariable % (maxValue - minValue)) + minValue);

		//Clamp it
		if (minValue == 0.0f) {
			noiseVariable = -noiseVariable;
		}

		noiseVariable = ClampFloat(noiseVariable, minValue, maxValue);

		//Return it
		return noiseVariable;
	}

	/// <summary>
	/// Take a number and introduce "noise" into it to help generate a random number.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="positive"></param>
	/// <returns></returns>
	public static float GenerateNoiseInFloat(float x, bool positive) {
		///Convert "x" to int and multiply by 3.
		int randValue = Mathf.RoundToInt(x) + Mathf.CeilToInt(x) + Mathf.FloorToInt(x);
		
		//Make local int variable 1 if it is divisible by 3 to help introduce randomness.
		if (randValue % 3 == 0) {
			randValue = 1;
		}

		//Perform calculations to generate noise.
		x = ((x * 8931) * x * randValue);
		x = (1.0f - ((randValue * (randValue * randValue * 15731 + 789221) + 1378312589) & 0x7ffffff) / 1073741824.0f * x);
		if (positive && x < 0.0f) {
			x = -x;
		}

		//Return result.
		return x;
	}

	/// <summary>
	/// Takes an input float and returns minValue if the input is too small.
	/// Returns maxValue if the input is too large.
	/// </summary>
	/// <param name="valueToClamp"></param>
	/// <param name="minValue"></param>
	/// <param name="maxValue"></param>
	/// <returns></returns>
	public static float ClampFloat(float valueToClamp, float minValue, float maxValue) {
		if (valueToClamp <= minValue) {
			return minValue;
		} else if (valueToClamp >= maxValue) {
			return maxValue;
		} else {
			return valueToClamp;
		}
	}
	#endregion
}
