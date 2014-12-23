using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour {

	public float bulletVelocity = 100;
	public GameObject bullet;
	public Transform bulletSpawn;

	public void Fire() {
		GameObject g = (GameObject) GameObject.Instantiate (bullet, bulletSpawn.position, bulletSpawn.rotation);
		ConstantForce2D c = g.GetComponent<ConstantForce2D> ();
		c.force.x = bulletVelocity;
	}
}
