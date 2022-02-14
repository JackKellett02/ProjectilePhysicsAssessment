using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the aiming and firing for the interceptor turret
/// and it completes all the calculations needed so that the interceptor
/// projectile can hit it's target.
/// </summary>
public class InterceptorTurretScript : MonoBehaviour {
	#region Variables to assign via the unity inspector
	[SerializeField]
	private GameObject ProjectilePrefab = null;

	[SerializeField]
	private GameObject audioHolderObject = null;
	#endregion

	#region Variable Declarations
	public static Vec3 vectorToFireIn;
	public static Vec3 velocityNeededToIntercept;
	private Vec3 rotationOfTurret;
	private Vec3 displacementOfTarget;
	private Vec3 newPositonOfTarget;
	private Vec3 vectorToTarget;
	private float interceptTime = 1.0f;//Put in the editor how much time it should take the projectile to intercept it's target.
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		rotationOfTurret = new Vec3(0.0f, 0.0f, 0.0f);
	}

	// Update is called once per frame
	void Update() {
		ControlInterceptTime();
		AimAndFireProjectile();
	}

	/// <summary>
	/// Calculates how far the target projectile moves
	/// in the given intercept time.
	/// </summary>
	private void CalculateDisplacementOfTarget() {
		//Declare Component Variables of Displacement.
		float x = 0.0f;
		float y = 0.0f;
		float z = 0.0f;

		//Get the target's starting velocity
		Vec3 projectileVelocity = ProjectileScript.GetTargetProjectileList()[0].GetComponent<ProjectileScript>().GetVelocity();
		Vec3 projectileAcceleration = ProjectileScript.GetTargetProjectileList()[0].GetComponent<ProjectileScript>().GetAcceleration();

		//Calculate Displacement in x Axis
		x = SUVAT.CalculateDisplaceMent1(projectileVelocity.x, interceptTime, projectileAcceleration.x);

		//Calculate Displacement in y Axis
		y = SUVAT.CalculateDisplaceMent1(projectileVelocity.y, interceptTime, projectileAcceleration.y);

		//Calculate Displacement in z Axis
		z = SUVAT.CalculateDisplaceMent1(projectileVelocity.z, interceptTime, projectileAcceleration.z);

		//Set Displacement Vec3 to these values.
		displacementOfTarget = new Vec3(x, y, z);
	}

	/// <summary>
	/// Calculates the position of the target at intercept
	/// by adding the starting position of the projectile
	/// onto the displacement of it after the intercept time
	/// has passed.
	/// </summary>
	private void CalculatePositionOfTargetAtIntercept() {
		//Set Displacement Vec3 after 3 seconds to correct value
		CalculateDisplacementOfTarget();

		//Get the original position of the target
		Vec3 projectilePosition = ProjectileScript.GetTargetProjectileList()[0].GetComponent<ProjectileScript>().GetPositon();
		//Debug.Log(ProjectileScript.targetProjectiles.Count + ", " + projectilePosition.x + ", " + projectilePosition.y + ", " + projectilePosition.z);

		//Set the new position (intercept coords) to the projectile position + the displacement
		newPositonOfTarget = projectilePosition + displacementOfTarget;
	}

	/// <summary>
	/// Calculates the displacement vector from launcher to the intercept
	/// point by taking away the the launchers position from the intercept position.
	/// </summary>
	private void CalculateDisplacementFromLauncherToIntercept() {
		//Calculate the intercept coordinates.
		CalculatePositionOfTargetAtIntercept();

		//Get the launcher position
		Vec3 launcherPos = new Vec3(transform.position);

		//Calculate Displacement From Launcher to intercept
		vectorToTarget = newPositonOfTarget - launcherPos;
	}

	/// <summary>
	/// Calculates the velocity needed to intercept the target from
	/// the projectiles displacement and acceleration and intercept time.
	/// </summary>
	private void CalculateVelocityNeededToIntercept() {
		//Calculate Displacement Needed
		CalculateDisplacementFromLauncherToIntercept();

		//Intialise the velocity Vec 3
		velocityNeededToIntercept = new Vec3();

		//Get the target projectile's acceleration
		Vec3 projectileAcceleration = ProjectileScript.GetTargetProjectileList()[0].GetComponent<ProjectileScript>().GetAcceleration();

		//Calculate x component of velocity
		velocityNeededToIntercept.x = SUVAT.CalculateInitialVelocity5(vectorToTarget.x, interceptTime);

		//Calculate y component of velocity
		velocityNeededToIntercept.y = SUVAT.CalculateInitialVelocity2(vectorToTarget.y, interceptTime, projectileAcceleration.y);

		//Calculate z component of velocity
		velocityNeededToIntercept.z = SUVAT.CalculateInitialVelocity5(vectorToTarget.z, interceptTime);
	}

	/// <summary>
	/// Calculates the unit vector of the vector to the target projectile.
	/// </summary>
	private void GenerateVectorToFireIn() {
		//Generate Values
		float xValue = vectorToTarget.x;
		float yValue = vectorToTarget.y;
		float zValue = vectorToTarget.z;

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
	/// Gets the vector to fire in and converts it
	/// to pitch and yaw using trigonometry.
	/// </summary>
	private void TurnShooterTurret() {
		//Declare the rotation variables.
		float xRotation = 0.0f;
		float yRotation = 0.0f;
		float xzPlane = 0.0f;

		if (vectorToFireIn.x < 0.0f || vectorToFireIn.z < 0.0f) {
			xzPlane = 0.0f - Mathf.Sqrt(velocityNeededToIntercept.x * velocityNeededToIntercept.x + velocityNeededToIntercept.z * velocityNeededToIntercept.z);
		} else {
			xzPlane = Mathf.Sqrt(velocityNeededToIntercept.x * velocityNeededToIntercept.x + velocityNeededToIntercept.z * velocityNeededToIntercept.z);
		}

		//Calculate yaw from the vector to fire in.
		yRotation = (Mathf.Rad2Deg * Mathf.Atan(velocityNeededToIntercept.x / velocityNeededToIntercept.z)) + 180.0f;

		//Calculate pitch from the vector to fire in
		xRotation = Mathf.Rad2Deg * Mathf.Atan(velocityNeededToIntercept.y / xzPlane);

		//Set the rotation vec3 to the new value
		rotationOfTurret = new Vec3(xRotation, yRotation, 0.0f);
	}

	/// <summary>
	/// Turns the turret towards a target
	/// and fires at them if there's a target
	/// inside of the scene.
	/// </summary>
	private void AimAndFireProjectile() {
		//Check if the player projectile has fired.
		if (PlayerTurretScript.GetProjectileFired()) {
			//Check if there is a player projectile in the scene.
			if (ProjectileScript.GetTargetProjectileList().Count > 0 && ProjectileScript.GetTargetProjectileList()[0] != null) {
				//Aim the turret and fire the projectile.
				CalculateVelocityNeededToIntercept();
				GenerateVectorToFireIn();
				TurnShooterTurret();
				UpdateTurretTransforms();

				//Turret Audio
				TurnOnTurretFireAudio();

				//Spawn Projectile
				Instantiate(ProjectilePrefab, this.transform);
			}
		}
	}

	/// <summary>
	/// Changes the turrets transform.rotation to the rotationOfTurret Vec3.
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

	private void ControlInterceptTime() {
		//Check if player wants to increase or decrease intercept time.
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			interceptTime += 0.1f;
		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			interceptTime -= 0.1f;
		}

		//Clamp between min and max values.
		interceptTime = ClampFloat(interceptTime, 0.5f, 5.0f);
	}
	#endregion

	#region Public Access Functions
	/// <summary>
	/// Makes sure the vector to fire in is generated then
	/// it returns it.
	/// </summary>
	/// <returns></returns>
	public Vec3 GetVectorToFireIn() {
		GenerateVectorToFireIn();
		return vectorToFireIn;
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

	public float GetInterceptTime() {
		return interceptTime;
	}
	#endregion
}
