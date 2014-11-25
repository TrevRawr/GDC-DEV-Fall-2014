var theTarget : Transform;
var lag : float;
var followX : boolean;
var followY : boolean;
var followZ : boolean;
var centered : boolean;

private var startingPosition : Vector3;

function Start()
{
startingPosition = transform.position;
if(centered)
{
startingPosition = Vector3.zero;
}
}

function Update()
{
	currentLag = 1-lag;
	if(followX)
			{
			this.transform.position.x = theTarget.position.x*currentLag + startingPosition.x;
			}
			
			if(followY)
			{
			this.transform.position.y = theTarget.position.y*currentLag + startingPosition.y;
			}

			if(followZ)
			{
			this.transform.position.z = theTarget.position.z*currentLag + startingPosition.z;
			}
}