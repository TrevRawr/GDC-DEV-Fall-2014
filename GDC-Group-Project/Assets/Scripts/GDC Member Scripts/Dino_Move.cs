using UnityEngine;
using System.Collections;

public class Dino_Move : MonoBehaviour 
{
	Animator dino;  //animation state machine for dino
	float moveX;
	public float maxSpeed;
	public float speedX;
	public bool grounded;
	public float jumpForce;
	bool facingRight;
	bool jump;
	//Ray2D ray;
	RaycastHit2D hit;

	// Use this for initialization
	void Start () 
	{
		dino = gameObject.GetComponent<Animator> ();  //initialize it as the state machine attached to the dino object so we can reference it in this code
	}

	//ground detection control
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "ground")
			grounded = true;
	}

	void OnCollisionExit2D(Collision2D collision)
	{
	 	if (collision.gameObject.tag == "ground")
			grounded = false;
	}

	//flip the character when switching directions from left to right
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
		if (moveX > 0 && !facingRight)
		{
			Flip ();
		}
		else if (moveX < 0 && facingRight)
		{
			Flip ();
		}

		dino.SetFloat ("move", moveX);
		Vector2 movementX = new Vector2 (moveX * speedX, 0);
		rigidbody2D.AddForce (movementX);

		//limit max horizontal speed
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
		{
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}


		if (Input.GetButton("Jump") && grounded)
			jump = true;

		if (jump) 
		{
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}
}
