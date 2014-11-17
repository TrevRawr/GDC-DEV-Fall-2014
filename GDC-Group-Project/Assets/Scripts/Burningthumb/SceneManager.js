
//#pragma strict
/**
The scene manager replaces the Application class for loading scenes. The manager
tracks what scenes have been loaded so they may easily be unloaded and may be
serialized so the gamestate may be preserved accros launches.

In order to use a scene with the SceneManager scenes should contain only one
item in the root of the hierachy, which is named "wrapperPrefix"+sceneFileName.
By default this means that scene file names should match the name of the root object.

<b>Note:</b> The SceneManager does not guard against duplicate loads.
@include SaveManager
@bug Destroy not working. Do slow poll until scene is loaded therefore object created. 
*/
static class SceneManager
{

	enum e_SceneLoadMode
	{
	Load,LoadAdditive,LoadAsync,LoadAdditiveAsync,Unload
	};
	/**
	Table of loaded scenes indexed by name.
	*/
	var loadedScenes : Hashtable = new Hashtable();
	var loadingScenes : ArrayList = new ArrayList();
	/**
	To prevent accidental deletion of other objects when invoking Unload() a
	prefix may be specified. e.g. prefix=_scene_ assetName=_scene_sceneName.
	*/
	final var wrapperPrefix : String ="";
	var initialized :boolean = false;
	
	function Load(sceneName : String, mode : e_SceneLoadMode )
	{
		if(loadingScenes.Count>1)
		{
			Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
		
			if(loadingScenes.Count>2)
			{
				Application.backgroundLoadingPriority = ThreadPriority.Normal;
			}
		
		}
		
		switch (mode)
		{
		case e_SceneLoadMode.Load:
		print(1);
			Load(sceneName);
			break;
		case e_SceneLoadMode.LoadAdditive:
		print(2);
			LoadAdditive(sceneName);
			break;
		case e_SceneLoadMode.LoadAsync:
		print(3);
			LoadAsync(sceneName);
			break;
		case e_SceneLoadMode.LoadAdditiveAsync:
		print(4);
			LoadAdditiveAsync(sceneName);
			break;
		case e_SceneLoadMode.Unload:
			Unload(sceneName);
			Debug.Log("Unloaded:Scene:"+sceneName);
		
		}
		
	}
	
	function RegisterScene(sceneName : String, object : GameObject)
	{
		
		loadedScenes.Add(sceneName,object);
		loadingScenes.Remove(sceneName);
		Debug.Log("Finished Loading:Scene:"+sceneName);

	}
	
	
	function Load(sceneName : String)
	{	
		if( !isLoaded(sceneName) && !isLoading(sceneName) )
		{
		Debug.Log("Begin Loading:Scene:"+sceneName);
		loadingScenes.Add(sceneName);
		Application.LoadLevel(sceneName);	
		}	
	}
	
	function LoadAsync(sceneName : String)
	{
		if( !isLoaded(sceneName) && !isLoading(sceneName) )
		{
		Debug.Log("Begin Loading:Scene:"+sceneName);
		loadingScenes.Add(sceneName);
		var async : AsyncOperation = Application.LoadLevelAsync(sceneName);
		yield async;
		}
	}
	
	/**
	@todo duplication checks
	*/
	function LoadAdditiveAsync(sceneName : String)
	{
		if( !isLoaded(sceneName) && !isLoading(sceneName) )
		{
		Debug.Log("Begin Loading:Scene:"+sceneName);
		loadingScenes.Add(sceneName);
		var async : AsyncOperation = Application.LoadLevelAdditiveAsync(sceneName);
		
		while(!async.isDone)
		yield async;
		}

	}
	
	function LoadAdditive(sceneName : String)
	{
	print(isLoaded(sceneName));
	print(isLoading(sceneName));
	
		if( !isLoaded(sceneName) && !isLoading(sceneName) )
		{
		Debug.Log("Begin Loading:Scene:"+sceneName);
		loadingScenes.Add(sceneName);
		Application.LoadLevelAdditive(sceneName);
		}
	}
	
	function isLoaded(sceneName : String) : boolean
	{
		if(loadedScenes.ContainsKey(sceneName))
		{
			return true;
		}
		return false;
	}
	
	function isLoading(sceneName : String) : boolean
	{
		if(loadingScenes.Contains(sceneName))
		{
			return true;
		}
		return false;
	}
	
	function Unload(sceneName : String)
	{
	print(isLoaded(sceneName));
	print(isLoading(sceneName));
		if(SceneManager.isLoaded(sceneName))
		{
			Debug.Log(loadedScenes[sceneName]);
			var toDestroy : GameObject = (loadedScenes[sceneName]) as GameObject;
			GameObject.Destroy(toDestroy);
			Unregister(sceneName);
		}
	}
	
	function Unregister(sceneName : String)
	{
		loadedScenes.Remove(sceneName);
	}
	
	function UnloadAll ()
	{
		for (var each : DictionaryEntry in loadedScenes as Hashtable)
		{
		Unload(each.Key);
		}
	}
	
	function Initialize()
	{
	 initialized = true;
	}
	
	function isInitialized()
	{
	return initialized;
	}
	
	function LoadInitial()
	{
	Initialize();
	Application.LoadLevel("init");
	}
	
	/**
	This additivly loads a scene for cutscene playback then asynchronously additivly loads the
	next scene, then destroys the cuscene scene.
	@todo not implemented.
	*/
	#if !(UNITY_IOS || UNITY_ANDROID)
	function CutsceneLoad(sceneName : String, cutsceneName : String, unloadOthers : boolean)
	{
		if(unloadOthers)
		{
			UnloadAll();
		}
		LoadAdditiveAsync("_cutscene");
	}
	#endif 
}