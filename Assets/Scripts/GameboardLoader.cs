using UnityEngine;
using System.Collections;
using System.Threading;

public class GameboardLoader : MonoBehaviour 
{
	public GameObject ObjectToLoad;

	private Thread _thread;
	private AsyncOperation _async;
	private bool _loadingLevel = false;


	void Awake()
	{
		_loadingLevel = true;
		_async = Application.LoadLevelAdditiveAsync ("PlayMode");
	}

	private void Update()
	{
		if (_loadingLevel && _async.isDone) 
		{
			_loadingLevel = false;
			Debug.Log("Load complete");
			GameObject.Find("Main Camera").SetActive(false);
			//GameObject.Find("Game").SetActive(false);
		}
	}

	private void LoadGameboard()
	{
		//Debug.Log ("Executing on thread");
		//var go = Instantiate (ObjectToLoad);
		//DontDestroyOnLoad (go);
	}
}
