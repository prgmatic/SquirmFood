using UnityEngine;
using System.Collections;

public class LevelSelectionPanel : MonoBehaviour
{

	public static LevelSelectionPanel Instance { get { return _instance; } }
	private static LevelSelectionPanel _instance;

	public LevelSelectButton LevelButtonPrefab;
	public RectTransform Content;
	public NewLevelSet LevelSet;

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			Init();
		}
		else if (this != _instance)
			Destroy(this.gameObject);
	}

	private void Init()
	{
		FillContent();
	}

	public void Show()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			this.transform.GetChild(i).gameObject.SetActive(true);
		}
	}
	public void Hide()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			this.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	private void ClearContent()
	{
		for (int i = 0; i < Content.childCount; i++)
		{
			Destroy(Content.GetChild(i).gameObject);
		}
	}

	public void OpenMyLevels()
	{
		Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("OpenMyLevelsPanel");
	}

	private void FillContent()
	{
		ClearContent();
		if (LevelSet != null)
		{
			int levelNum = 1;
			for (int i = 0; i < LevelSet.Levels.Count; i++)
			{
				LevelSelectButton button = (LevelSelectButton)Instantiate(LevelButtonPrefab);
				button.ButtonText.text = levelNum.ToString();
				button.transform.SetParent(Content.transform);
				button.Layout = LevelSet.Levels[i];
				button.LevelSelected += Button_LevelSelected;
				button.transform.localScale = new Vector3(1, 1, 1);
				levelNum++;
			}
		}
	}

	private void Button_LevelSelected(NewBoardLayout level)
	{
		LevelSelectionInfo.SelectLevel(level, this.LevelSet);
		Application.LoadLevel("PlayMode");
	}
}
