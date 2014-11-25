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
			//see it between every 5s and 4 minutes, this way some people never see it, others can't figure out 
			//what it its
			float t = Random.Range(5f,240.0f);
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
		//2 frames of wtf
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		s.color = revert;

	}
}
