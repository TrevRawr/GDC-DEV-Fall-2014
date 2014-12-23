using UnityEngine;
using System.Collections;

public class destroyOnCollide : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D coll) {
		Debug.Log ("Collide");
		Destroy (gameObject);
	}
}
