using UnityEngine;
using System.Collections;

public class AgentAI : MonoBehaviour {
	Animator animator;
	Transform agent;
	bool engage = false;
	public EnemyGun gun; // The gun child class of the agent.
	public float speed; // The speed of the agent.
	public float fireInterval;
	public Transform player; // The position of the play character.

	public void Start() {
		animator = GetComponent<Animator> ();
		agent = GetComponent<Transform> ();
		move ();
	}

	public void shoot() {
		gun.Fire ();
	}

	void FixedUpdate() {
		move ();
	}

	void move() {
		if (engage == false) {
						if (player.position.x > agent.position.x) {
								GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
								
						} else if (player.position.x < agent.position.x) {
								GetComponent<Rigidbody2D>().velocity.Set (-speed, 0);
						}
						animator.SetFloat ("speed", 1);
				} else {
						animator.SetFloat ("speed", 0);
				}
	}

	public void EngagePlayer() {
		if (engage == false) {
						InvokeRepeating ("shoot", 0, fireInterval);
						engage = true;
				}
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}

	public void DeengagePlayer() {
		engage = false;
		CancelInvoke ();
	}
}
