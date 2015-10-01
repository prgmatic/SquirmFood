using UnityEngine;
using System.Collections;
using System.Threading;

public class GameboardLoader : MonoBehaviour 
{
	public GameObject ObjectToLoad;

	private Thread _thread;
	private AsyncOperation _async;
	private bool _loadingLevel = false;
    private float _awakeTime;

	void Awake()
	{
        _awakeTime = Time.time;
		_loadingLevel = true;
		_async = Application.LoadLevelAdditiveAsync ("PlayMode");
	}

	private void Update()
	{
		if (_loadingLevel && _async.isDone) 
		{
			_loadingLevel = false;
            Debug.Log(string.Format("Load Time {0}", Time.time - _awakeTime));
			GameObject.Find("Main Camera").SetActive(false);
            //GameObject.Find("Game").SetActive(false);
            this.enabled = false;
		}
	}

	private void LoadGameboard()
	{
		//Debug.Log ("Executing on thread");
		//var go = Instantiate (ObjectToLoad);
		//DontDestroyOnLoad (go);
	}
}
