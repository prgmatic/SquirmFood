using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfoView : MonoBehaviour
{
    public Text LevelName;
    public GameObject PlayButton;
    public GameObject ResumeButton;

    private int _levelNumber;


    public void SetLevel(int levelNumber)
    {
        _levelNumber = levelNumber;
        var level = GetLevel(levelNumber);
        if (level != null)
            LevelName.text = level.name;
        else
            LevelName.text = "Invalid Level";
        if(GameManager.Instance.State == GameManager.GameState.GamePaused 
            && levelNumber == GameManager.Instance.CurrentLevel)
        {
            PlayButton.Hide();
            ResumeButton.Show();
        }
        else
        {
            PlayButton.Show();
            ResumeButton.Hide();
        }

    }

    public void PlayLevel()
    {
        GameManager.Instance.PlayLevel(_levelNumber);
    }

    private NewBoardLayout GetLevel(int levelNumber)
    {
        if (levelNumber >= 0 && levelNumber < GameManager.Instance.LevelSet.Levels.Count)
            return GameManager.Instance.LevelSet.Levels[levelNumber];
        return null;
    }
}
