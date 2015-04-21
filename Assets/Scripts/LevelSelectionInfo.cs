using UnityEngine;
using System.Collections.Generic;

public class LevelSelectionInfo : MonoBehaviour 
{
	public BoardLayout SelectedLevel;
	public LevelSet LevelSet;

	public static BoardLayout Level
	{
		get
		{
			if (Instance != null)
				return Instance.SelectedLevel;
			return null;
		}
	}

	public static LevelSelectionInfo Instance { get { return _instance; } }
	private static LevelSelectionInfo _instance;

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else if (this != _instance)
			Destroy(this.gameObject);
	}

	public static void SelectLevel(BoardLayout level, LevelSet levelSet)
	{
		if(Instance == null)
		{
			GameObject go = new GameObject();
			go.name = "LevelSelectionInfo";
			go.AddComponent<LevelSelectionInfo>();
		}
		Instance.SelectedLevel = level;
		Instance.LevelSet = levelSet;
	}

	public void AdvanceLevel()
	{
		if(LevelSet == null)
		{
			Debug.LogError("LevelSet is null, cannot advance level");
			return;
		}
		int index = LevelSet.Levels.IndexOf(SelectedLevel);
		index++;
		if (index >= LevelSet.Levels.Count) index = 0;
		SelectedLevel = LevelSet.Levels[index];
		Gameboard.Instance.CurrentLevel = SelectedLevel;
	}
}
