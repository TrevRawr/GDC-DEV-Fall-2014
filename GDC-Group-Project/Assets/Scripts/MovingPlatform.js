var waypointCurrent : int =0;
var waypointNext : int =1;
var waypoints : Transform[];
var waitTime : float;
var interval : float;
var lerpStep : float;
var activateOnAwake : boolean = true;
var activated : boolean = false;

function Awake()
{
if(!enabled)
return;

	transform.position=waypoints[0].position;
	
	if(activateOnAwake)
	Activate();
}

function Activate()
{
	if (activated)
		return;
	else
	{
		activated = true;
		MovePlatform();
	}
}

function OnCollisionEnter( other : Collision)
{
//Debug.Log("Transfered");
	other.transform.parent=transform.parent;
}
function OnCollisionExit( other : Collision)
{
	other.transform.parent=null;
	Debug.LogWarning("Original parent restoration not implemented");
}
function MovePlatform()
{
	var lerpIndex : float;
	
	while(true)
	{
		if((waypointNext)>(waypoints.length-1))
		{
			waypointNext=0;
		}
		
		while(1 >=lerpIndex)
		{
			lerpIndex+=lerpStep;
			transform.position=Vector3.Lerp(waypoints[waypointCurrent].position, waypoints[waypointNext].position,lerpIndex);
			yield WaitForSeconds(interval);
		}	
		
		if(0==waypointNext)
		{
			waypointCurrent=0;
			waypointNext=1;
		}
		else
		{
			waypointCurrent++;
			waypointNext++;
		}
		
		lerpIndex=0;
		
		yield WaitForSeconds(waitTime);
		
	}
}

