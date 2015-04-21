using UnityEngine;
using System.Collections.Generic;

public class SceneManagerHelper : MonoBehaviour 
{
	public void OpenLevelSelection()
	{
		SceneManager.OpenLevelSelection();
	}
}

public static class SceneManager
{
	public static void OpenLevelSelection()
	{
		Application.LoadLevel("LevelSelection");
	}
}
