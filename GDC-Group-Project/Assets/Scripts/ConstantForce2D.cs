using UnityEngine;
using System.Collections;


public class ConstantForce2D : MonoBehaviour 
{
	public Vector2 force;

	void FixedUpdate()
	{
		Rigidbody2D r = this.GetComponent<Rigidbody2D>();
		r.AddRelativeForce (force * Time.deltaTime);
	}
}