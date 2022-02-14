using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to control what happens when the hit ground audio object is activated.
/// </summary>
public class HitGroundAudioControllerScript : MonoBehaviour {
	private void Awake() {
		StartCoroutine("DestroyAudioHolderAfterOneSecond");
	}

	private IEnumerator DestroyAudioHolderAfterOneSecond() {
		yield return new WaitForSeconds(1.0f);
		Destroy(gameObject);
	}
}
