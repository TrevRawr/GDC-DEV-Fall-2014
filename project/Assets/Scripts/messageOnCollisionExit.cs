using UnityEngine;
using System.Collections;

public class MessageOnCollisionExit : MonoBehaviour {

	public bool onTrigger = false;
	public string message = "Activate";
	public GameObject[] targets;
	public SendMessageOptions options = SendMessageOptions.DontRequireReceiver;
	
	void OnCollisionExit2D(Collision2D other)
	{
		foreach(GameObject g in targets)
		{
			g.SendMessage(message,options);
		}
	}
	
	void OnCollisionExit(Collision other)
	{
		foreach(GameObject g in targets)
		{
			g.SendMessage(message,options);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		foreach(GameObject g in targets)
		{
			g.SendMessage(message,options);
		}
	}
}
