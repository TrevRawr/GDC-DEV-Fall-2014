using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioPitchWithTimeScale : MonoBehaviour {
	

	// Update is called once per frame
	void Update () {
		this.GetComponent<AudioSource>().pitch=Time.timeScale;
	}
}
