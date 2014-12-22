using UnityEngine;
using System.Collections;

public class MessageOnCollision : MonoBehaviour {

	public string message = "Activate";
	public GameObject[] targets;
	public SendMessageOptions options = SendMessageOptions.DontRequireReceiver;

	void OnCollisionEnter2D(Collider2D other)
	{
		foreach(GameObject g in targets)
		{
			g.SendMessage(message,options);
		}
	}

	void OnCollisionEnter(Collider other)
	{
		foreach(GameObject g in targets)
		{
			g.SendMessage(message,options);
		}
	}
}
