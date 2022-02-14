using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is attached in the unity world and calls the function that checks the collisions of all projectiles in the scene.
/// </summary>
public class ObjectCollisionScript : MonoBehaviour {
	// Update is called once per frame
	void Update() {
		ProjectileScript.CheckProjectileToProjectileCollisions();
	}
}
