using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the movement/destruction and collisions for all projectiles
/// whether target or interceptor.
/// </summary>
public class ProjectileScript : MonoBehaviour {
	#region Variables to assign via the unity inspector
	[SerializeField]
	private bool shooterProjectile = true;

	[SerializeField]
	private float speedOfProjectile = 25.01f;

	[SerializeField]
	private float radius = 1.0f;

	[SerializeField]
	private GameObject audioHitGroundObject = null;

	[SerializeField]
	private GameObject projectileToProjectileAudioObject = null;

	[SerializeField]
	private GameObject explosionEffectPrefab = null;

	[SerializeField]
	private GameObject explosionEffectPrefab2 = null;
	#endregion

	#region Variable Declarations
	private Vec3 aimingVector;
	private Vec3 acceleration = new Vec3(0, -9.8f, 0);
	private Vec3 velocity;
	private Vec3 position;
	private static List<GameObject> allProjectiles = new List<GameObject>();//Used when checking projectile collisions.
	private static List<GameObject> targetProjectiles = new List<GameObject>();//Used when calculating intercept vector.
	private static List<GameObject> interceptorProjectiles = new List<GameObject>();//Used to help get velocity of intercept projectiles for UI.
	private static int missCounter = 0;
	private static int interceptCounter = 0;
	private static Vec3 lastProjectilePositon = null;
	#endregion

	#region Private Functions
	private void Awake() {
		//Add projectile to the lists.
		allProjectiles.Add(gameObject);
		if (shooterProjectile) {
			targetProjectiles.Add(gameObject);
		} else if (!shooterProjectile) {
			interceptorProjectiles.Add(gameObject);
		}

		//Set the starting velocity of the projectile.
		SetStartingVelocity();

		position = new Vec3(transform.position);
	}

	// Update is called once per frame
	void Update() {
		CheckCollisionWithGround();
		AccelerateProjectile();
		UpdateProjectilePosition();
		MoveProjectileToNewPosition();
	}

	/// <summary>
	/// Sets what the starting velocity of the projectile should be
	/// based on where the turret is aiming.
	/// </summary>
	private void SetStartingVelocity() {
		//Initialise velocity vector
		velocity = new Vec3();

		//Check if the projectile is a target or interceptor
		if (shooterProjectile) {
			//As the aiming vector is a unit vector, multiply it by starting speed to get the starting velocity.
			aimingVector = PlayerTurretScript.vectorToFireIn;
			velocity = aimingVector * speedOfProjectile;
		} else if (!shooterProjectile) {
			//Get velocity needed to intercept variable
			velocity = InterceptorTurretScript.velocityNeededToIntercept;
		}
	}

	/// <summary>
	/// Increases the velocity vector by acceleration.
	/// </summary>
	private void AccelerateProjectile() {
		//Increase velocity by acceleration.
		velocity.x += acceleration.x * Time.deltaTime;
		velocity.y += acceleration.y * Time.deltaTime;
		velocity.z += acceleration.z * Time.deltaTime;
	}

	/// <summary>
	/// Increases the position vector by velocity.
	/// </summary>
	private void UpdateProjectilePosition() {
		//Increase position by velocity.
		position.x += velocity.x * Time.deltaTime;
		position.y += velocity.y * Time.deltaTime;
		position.z += velocity.z * Time.deltaTime;
	}

	/// <summary>
	/// Updates the projectile transform to the position vector.
	/// </summary>
	private void MoveProjectileToNewPosition() {
		//Update transform to new position Vec3.
		transform.position = position.ToVector3();
	}

	/// <summary>
	/// Removes the projectileToDestroy from the appropriate lists
	/// and then destroys it.
	/// </summary>
	/// <param name="projectileToDestroy"></param>
	private static void DestroyProjectile(GameObject projectileToDestroy) {
		if (projectileToDestroy.GetComponent<ProjectileScript>().GetShooterVariable()) {
			targetProjectiles.Remove(projectileToDestroy);
		} else if (!projectileToDestroy.GetComponent<ProjectileScript>().GetShooterVariable()) {
			interceptorProjectiles.Remove(projectileToDestroy);
		}

		allProjectiles.Remove(projectileToDestroy);
		Destroy(projectileToDestroy);
	}

	/// <summary>
	/// Checks if a projectile has hit the ground or not.
	/// </summary>
	private void CheckCollisionWithGround() {
		//Check if the projectile has hit the ground.
		if (transform.position.y <= radius) {
			if (shooterProjectile) {
				//Count collision with ground as miss
				missCounter += 1;

				//Play the miss sound audio.
				Instantiate(audioHitGroundObject);
			}

			//Set the last position of the projectile.
			lastProjectilePositon = position;

			//Instantiate projectile explosion prefab.
			Instantiate(explosionEffectPrefab2);


			//Destroy the projectile
			DestroyProjectile(gameObject);
		}
	}

	/// <summary>
	/// Gets the position of the two projectiles and calculates the vector between them.
	/// </summary>
	/// <param name="projectile"></param>
	/// <param name="targetProjectile"></param>
	/// <returns></returns>
	private static Vec3 CalculateVectorBetweenProjectiles(GameObject projectile, GameObject targetProjectile) {
		//Get Projectile Positions
		Vec3 projectilePos = projectile.GetComponent<ProjectileScript>().GetPositon();
		Vec3 targetProjectilePos = targetProjectile.GetComponent<ProjectileScript>().GetPositon();

		//Calculate Vector Between the projectiles.
		return targetProjectilePos - projectilePos;
	}

	/// <summary>
	/// Calculates the magnitude squared of a passed Vec3.
	/// </summary>
	/// <param name="vector"></param>
	/// <returns></returns>
	private static float CalculateVectorMagnitudeSquared(Vec3 vector) {
		return (vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
	}
	#endregion

	#region Public Access Functions
	/// <summary>
	/// Check if any projectile in the scene has collided with any other projectile in the scene.
	/// </summary>
	public static void CheckProjectileToProjectileCollisions() {
		if (allProjectiles.Count > 1) {
			for (int i = 0; i < allProjectiles.Count; i++) {
				for (int n = i + 1; n < allProjectiles.Count; n++) {
					if (allProjectiles[i] != allProjectiles[n]) {
						//Get the distance between the projectiles squared
						Vec3 vectorBetweenProjectiles = CalculateVectorBetweenProjectiles(allProjectiles[i], allProjectiles[n]);
						float vectorMagnitudeSquared = CalculateVectorMagnitudeSquared(vectorBetweenProjectiles);

						//Get the radius of both projectiles add them together then square the answer.
						float projectileRadius = allProjectiles[i].GetComponent<ProjectileScript>().GetRadius() + allProjectiles[n].GetComponent<ProjectileScript>().GetRadius();
						float projectileRadiusSquared = projectileRadius * projectileRadius;

						//Check if the distance between them is smaller than the radius of them added together.
						if (projectileRadiusSquared >= vectorMagnitudeSquared) {
							//Check if one of the projectiles was a target
							if (allProjectiles[i].GetComponent<ProjectileScript>().GetShooterVariable() || allProjectiles[n].GetComponent<ProjectileScript>().GetShooterVariable()) {
								ProjectileScript.IncrementInterceptCounter();
							}

							//Player Projectile to projectile collision audio.
							GameObject audioObject = allProjectiles[i].GetComponent<ProjectileScript>().GetProjectileCollisionAudioObject();
							Instantiate(audioObject);

							//Set the last position of the projectile.
							lastProjectilePositon = allProjectiles[i].GetComponent<ProjectileScript>().GetPositon();

							//Instantiate projectile explosion prefab.
							Instantiate(allProjectiles[i].GetComponent<ProjectileScript>().GetExplosionPrefab());

							//Destroy the projectiles and remove them from the lists
							DestroyProjectile(allProjectiles[i]);
							DestroyProjectile(allProjectiles[n - 1]);
						}
					}
				}
			}
		}
	}

	public GameObject GetProjectileCollisionAudioObject() {
		return projectileToProjectileAudioObject;
	}
	public Vec3 GetAcceleration() {
		return acceleration;
	}

	public Vec3 GetVelocity() {
		return velocity;
	}

	public Vec3 GetPositon() {
		return new Vec3(transform.position);
	}

	public bool GetShooterVariable() {
		return shooterProjectile;
	}

	public float GetRadius() {
		return radius;
	}

	public static Vec3 GetLastProjectilePosition() {
		return lastProjectilePositon;
	}

	public static void IncrementInterceptCounter() {
		interceptCounter += 1;
	}

	public static int GetInterceptCounter() {
		return interceptCounter;
	}

	public static int GetMissCounter() {
		return missCounter;
	}

	public static List<GameObject> GetTargetProjectileList() {
		return targetProjectiles;
	}

	public static List<GameObject> GetInterceptorProjectileList() {
		return interceptorProjectiles;
	}

	public float GetProjectileSpeed() {
		//Calculate magnitude of velocity vector then return it.
		return Mathf.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y + velocity.z * velocity.z);
	}

	public static int GetInterceptProjectileListCount() {
		return interceptorProjectiles.Count;
	}

	public static int GetTargetProjectileListCount() {
		return targetProjectiles.Count;
	}

	public GameObject GetExplosionPrefab() {
		return explosionEffectPrefab;
	}

	public GameObject GetExplosionPrefab2() {
		return explosionEffectPrefab2;
	}
	#endregion
}
