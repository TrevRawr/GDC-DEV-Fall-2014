using UnityEngine;
using System.Collections;

public class DinoMove : MonoBehaviour 
{
	Animator dino;  		// Animation state machine for dino
	public float maxSpeed;	// The maximum speed the dino can go
	public float speedX;	// The amount of speed
	public float jumpForce;	// The force of each jump

	private float moveX;
	private bool facingRight;
	private bool jump;		// TRUE if dino can jump. FALSE otherwise
	private bool grounded;	// TRUE if dino is touching the ground. FALSE otherwise
	RaycastHit2D hit; 		//Ray2D ray -> what does this do?

	// Use this for initialization
	void Start () 
	{
		dino = gameObject.GetComponent<Animator> ();  //initialize it as the state machine attached to the dino object so we can reference it in this code
	}

	// Gound detection control -- Touched the ground
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "ground")
			grounded = true;
	}

	// Gound detection control -- Left the ground
	void OnCollisionExit2D(Collision2D collision)
	{
	 	if (collision.gameObject.tag == "ground")
			grounded = false;
	}

	// Flip the character when switching directions from left to right
	void Flip()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		facingRight = !facingRight;
	}

	void FixedUpdate()
	{
		moveX = Input.GetAxis ("Horizontal");

		// To the right, to the right
		if (moveX > 0 && !facingRight)
		{
			Flip ();
		}

		// To the left, to the left
		else if (moveX < 0 && facingRight)
		{
			Flip ();
		}

		dino.SetFloat ("move", moveX);
		Vector2 movementX = new Vector2 (moveX * speedX, 0);
		GetComponent<Rigidbody2D>().AddForce (movementX);

		// Limit max horizontal speed
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}

		// When touching the ground and player inputs 'Jump'
		if (Input.GetButton("Jump") && grounded)
			jump = true;

		// Jumping sequence
		if (jump) 
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}
}
