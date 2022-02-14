using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys the particle effect object after 1 second.
/// </summary>
public class ExplosionEffectScript : MonoBehaviour {
	private void Awake() {
		//Set the position of the explosion effect to the position of the projectile
		gameObject.transform.position = ProjectileScript.GetLastProjectilePosition().ToVector3();

		//Start destruction delay.
		StartCoroutine("DestructionDelay");
	}

	private IEnumerator DestructionDelay() {
		yield return new WaitForSeconds(1.0f);
		Destroy(gameObject);
	}
}
