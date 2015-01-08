using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpritePulser : MonoBehaviour {

	public Color[] colors;
	public int nextColor;
	public bool shuffle;
	public float maxLerpTime =1.0f;
	public float minLerpTime =0.7f;
	public float minDelay = 0.0f;
	public float maxDelay =0.0f;
	public int steps = 100;

	// Use this for initialization
	void Start () 
	{
		if(nextColor > colors.Length)
		{
			Debug.LogError("SpritePulser:Color exceeds available ranges",this.gameObject);
			return;
		}
		if(colors.Length < 2)
		{
			Debug.LogError("SpritePulser:Please specify 2 or more colors.",this.gameObject);
			return;
		}
		StartCoroutine(Repeat());
	}
	
	IEnumerator Repeat()
	{
		SpriteRenderer s = this.GetComponent<SpriteRenderer>();
		//Debug.Log("Summoning Satan");
		while(true)
		{
			float t = Random.Range(minLerpTime,maxLerpTime);
			float stepTime= t/(float)steps;
			Color old = s.color;
			if(shuffle)
			{
				int i =0;
				do
				{
					i = (int)Random.value*(colors.Length+1);
					// in case all the color happen to be the same or something, don't tie up the game
					yield return new WaitForEndOfFrame();				
				}while(colors[i].Equals(s.color));
				nextColor =i;
			}else
			{
				nextColor++;
				nextColor = nextColor%colors.Length;
			}

			//actually animate
			for(int i=0; i<steps;i++)
			{
				s.color = Color.Lerp(old,colors[nextColor],i/(float)steps);
				yield return new WaitForSeconds(stepTime);
			}

			yield return new WaitForSeconds(Random.Range(this.minDelay,this.maxDelay));
		}
	}

}
