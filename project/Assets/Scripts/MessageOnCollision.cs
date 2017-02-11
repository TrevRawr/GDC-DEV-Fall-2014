using UnityEngine;
using System.Collections;

public class MessageOnCollision : MonoBehaviour {

	public bool onTrigger = false;
	public string message = "Activate";
	public GameObject[] targets;
	public SendMessageOptions options = SendMessageOptions.DontRequireReceiver;

	void OnCollisionEnter2D(Collision2D other)
	{
		foreach(GameObject g in targets)
		{
			g.SendMessage(message,options);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		foreach(GameObject g in targets)
		{
			g.SendMessage(message,options);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		foreach(GameObject g in targets)
		{
			g.SendMessage(message,options);
		}
	}
}
