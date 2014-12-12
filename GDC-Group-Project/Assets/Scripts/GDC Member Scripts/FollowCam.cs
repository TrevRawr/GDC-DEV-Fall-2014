using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	GameObject dino;
	// Use this for initialization
	void Start () {
		dino = GameObject.Find ("Dino");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dinoPos = dino.transform.position;
		dinoPos.z = -10;
		dinoPos.y += 2;
		transform.position = dinoPos;
	}
}
