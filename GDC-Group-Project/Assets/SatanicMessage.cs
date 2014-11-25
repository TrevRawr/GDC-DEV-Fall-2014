using UnityEngine;
using System.Collections;

public class SatanicMessage : MonoBehaviour {
	public Color satanColor = Color.red;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(Repeat());
	}

	IEnumerator Repeat()
	{
		Debug.Log("Summoning Satan");
		while(true)
		{
			//see it between every 15s and 10 minutes
			float t = Random.Range(5f,600.0f);
			Debug.Log("Summoning Satan in "+t);
			yield return new WaitForSeconds(t);
			Debug.Log("Summoning Satan");
			StartCoroutine(SatanSpeaks());
		}
	}
	
	IEnumerator SatanSpeaks()
	{
		Debug.Log("Your soul is mine");
		SpriteRenderer s = this.GetComponent<SpriteRenderer>();
		Color revert = s.color;
		s.color = satanColor;
		//Debug.Break();
		//5 frames of wtf
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		s.color = revert;

	}
}
