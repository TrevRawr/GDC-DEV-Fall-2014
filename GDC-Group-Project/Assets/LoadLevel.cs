using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {


	public string level;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		Application.LoadLevel (level);
	}
}
