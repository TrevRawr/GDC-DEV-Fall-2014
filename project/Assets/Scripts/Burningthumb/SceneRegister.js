
function Start()
{
if(!SceneManager.isInitialized())
{
Debug.Log("Not first Scene, will load init");
//Debug.Break();
	SceneManager.LoadInitial();
}else
{
SceneManager.RegisterScene(gameObject.name,gameObject);
}

}

function OnDestroy()
{
SceneManager.Unregister(gameObject.name);
}