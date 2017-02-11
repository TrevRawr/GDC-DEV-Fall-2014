using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BulletTime : MonoBehaviour {

	 const float BULLET_TIME_SCALE =0.5f;
	const float REGEN_INTERVAL =0.5f;
	const float CONSUME_INTERVAL =0.2f;
	  bool on = false;
	static bool havePowerup=false;
	static int maxPoints=10;
	int currentPoints=0;
	int exhaustPenalty = 5;
	public Transform powerupBar;

	float oldTimeScale =1;
	// Use this for initialization
	void Start () {
		currentPoints =maxPoints;
		this.InvokeRepeating("Consume",0,CONSUME_INTERVAL);
		this.InvokeRepeating("Regen",0,REGEN_INTERVAL);
	}

	void Consume()
	{
		if(on)
		{
			currentPoints--;
		}
		if(currentPoints <=0)
		{
			Deactivate();
		}
	}

	void UpdateBar()
	{
		if(powerupBar !=null)
		{
			powerupBar.localScale = new Vector3(currentPoints/(float)maxPoints,1,1);
			
		}
	}

	void Regen()
	{
		if(!on)
		{
			if(currentPoints != maxPoints)
			{
				currentPoints++;


				
			}
		}
	}

	void Deactivate()
	{
		if(!on) return;
		Time.timeScale = oldTimeScale;
		on =false;

		if(currentPoints <=0)
		{
		currentPoints -= exhaustPenalty;
		}

	}

	void Activate()
	{
		if(on == false)
		{
			on = true;
			oldTimeScale = Time.timeScale;
		}
	}

	// Update is called once per frame
	void Update () 
	{
#if UNITY_EDITOR
		havePowerup=true;
#endif
		float bulletTimeIntensity = Input.GetAxis("Bullet Time");

		if(currentPoints>0)
		UpdateBar();

		if(bulletTimeIntensity <=0)
		{
			Deactivate();
			return;
		}
		else if (currentPoints > 0)
		{
			Activate();
			Time.timeScale = oldTimeScale - (bulletTimeIntensity*BULLET_TIME_SCALE);
		}


	}

	void OnDestroy()
	{
		CancelInvoke();
	}
}
