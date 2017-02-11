using UnityEngine;
using System.Collections;

public class Pauser : MonoBehaviour {
	private bool paused = false;
	private float oldScale =1;
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.P))
		{
			paused = !paused;
		}
		oldScale = Time.timeScale;
		if(paused)
		{

			Time.timeScale = 0;
		}
		else
			Time.timeScale = oldScale;
	}
}
