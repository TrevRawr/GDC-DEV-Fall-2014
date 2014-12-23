using UnityEngine;
using System.Collections;

public class AgentAI : MonoBehaviour {
	Animator animator;
	Transform agent;
	bool engage = false;
	public EnemyGun gun;
	public float speed;
	public float fireInterval = 0.3f;
	public Transform player;

	public void Start() {
		animator = GetComponent<Animator> ();
		agent = GetComponent<Transform> ();
		move ();
	}

	public void shoot() {
		gun.Fire ();
	}

	void FixedUpdate() {
		if (rigidbody2D.velocity.magnitude > 0) {
						animator.SetFloat ("speed", 1);
				} else {
			move ();
						animator.SetFloat ("speed", 0);
				}
	}

	public void move() {
		if (engage == false) {
						if (player.position.x > agent.position.x) {
								rigidbody2D.velocity.Set (speed, 0);
						} else {
								rigidbody2D.velocity.Set (-speed, 0);
						}
				}
	}

	public void EngagePlayer() {
		//this.InvokeRepeating ("shoot", 0, fireInterval);
		engage = true;
		shoot ();
		rigidbody2D.velocity.Set (0, 0);
	}
}
