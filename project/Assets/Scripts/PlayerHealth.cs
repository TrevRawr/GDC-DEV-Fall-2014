using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	public float health = 100f;					// The player's health.
	public float maxHealth = 100f;
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
	public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player

	public SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	private float lastHitTime;					// The time at which the player was last hit.
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private PlayerControl playerControl;		// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player


	void Awake ()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerControl>();

		if(healthBar == null)
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();

		if(healthBar == null)
		{
			Debug.LogError("Health bar not found");
			Debug.Break();
		}


		anim = GetComponent<Animator>();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Enemy")
		{
			// ... and if the time exceeds the time of the last hit plus the time between hits...
			if (Time.time > lastHitTime + repeatDamagePeriod) 
			{

					// ... take damage and reset the lastHitTime.
					TakeDamage(col.transform); 
					lastHitTime = Time.time; 

				// ... and if the player still has health...

			}
		}
		if(this.health < maxHealth)
		{
			DisplayHealth();
		}
	}

	void DisplayHealth()
	{
		healthBar.enabled = true;
	}

	void TakeDamage (Transform enemy)
	{
		// Make sure the player can't jump.
		//playerControl.jump = false; // must refer to dino move

		// Create a vector that's from the enemy to the player with an upwards boost.
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
		GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

		// Reduce the player's health by 10.
		health -= damageAmount;

		// Update what the health bar looks like.
		UpdateHealthBar();

		// Play a random clip of the player getting hurt.
		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);

		if(health < 0f)
		{
			// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
			
			// Find all of the colliders on the gameobject and set them all to be triggers.
			Collider2D[] cols = GetComponents<Collider2D>();
			foreach(Collider2D c in cols)
			{
				c.isTrigger = true;
			}
			
			// Move all sprite parts of the player to the front
			SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer s in spr)
			{
				s.sortingLayerName = "UI";
			}
			
			// ... disable user Player Control script
			GetComponent<PlayerControl>().enabled = false;
			
			// ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
			GetComponentInChildren<Gun>().enabled = false;
			
			// ... Trigger the 'Die' animation state
			anim.SetTrigger("Die");
		}
	}

	void KillDino ()
	{
		health = -1;
		anim.SetTrigger("Die");
		Debug.Log ("Dino died aaaa!!");
		Application.LoadLevel(Application.loadedLevel);
	}

	void OnDestroy()
	{
		this.StopAllCoroutines();
	}



	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}
}
