using UnityEngine;
using System.Collections;

public class TextMessageScript : MonoBehaviour {
	public string[] stringArr;
	public int curentTextIndex;
	public UnityEngine.UI.Text text;
	// Use this for initialization
	void Start () {
		//stringArr = new string[messageNumber];
	}

	public void Next()
	{
		curentTextIndex++;
		curentTextIndex = curentTextIndex % stringArr.Length;
		text.text = stringArr [curentTextIndex];

	}

	// Update is called once per frame
	void Update () {
	
	}
}
