using UnityEngine;
using System.Collections;

public class ExplodeOnCollision : MonoBehaviour {

	public float explosionRadius = 1;
	public float explosiveForce = 1;
	public float damage = 10;
	public Transform explosionEffect;
	public LayerMask layerMask;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		Explode();
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		Explode();
	}

	void Explode()
	{
		GameObject.Destroy(this.gameObject);

		//Rigidbody2D r = other.gameObject.GetComponent<Rigidbody2D>();
		Collider2D[] hitColliders =Physics2D.OverlapCircleAll(this.transform.position,explosionRadius,layerMask);
		
		int i = 0;
		while (i < hitColliders.Length) {
			hitColliders[i].SendMessage("TakeDamage",this.transform,SendMessageOptions.DontRequireReceiver);
			Rigidbody2D r = hitColliders[i].gameObject.GetComponent<Rigidbody2D>();
			if(r != null)
			{
				r.AddForceAtPosition(new Vector2(explosiveForce,explosiveForce),this.transform.position);
			}
			i++;
		}
		if(explosionEffect != null)
		{
			GameObject.Instantiate(explosionEffect,this.transform.position,Quaternion.identity);
		}

	}
}
