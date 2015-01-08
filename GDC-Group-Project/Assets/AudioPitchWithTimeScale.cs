using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioPitchWithTimeScale : MonoBehaviour {
	

	// Update is called once per frame
	void Update () {
		this.audio.pitch=Time.timeScale;
	}
}
