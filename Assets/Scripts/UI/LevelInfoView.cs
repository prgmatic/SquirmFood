using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfoView : MonoBehaviour
{
    public GameObject PlayButton;
    public GameObject ResumeButton;
    public RectTransform OptionalGoalsContainer;
    public GoalView GoalViewPrefab;

    private int _levelNumber;


    public void SetLevel(int levelNumber)
    {
        _levelNumber = levelNumber;

        // Do we show the play or resume button?
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
    public void AddGoal(string description)
    {
        var goalView = Instantiate(GoalViewPrefab);
        goalView.Text.text = description;
        goalView.transform.SetParent(OptionalGoalsContainer.transform, false);
    }
    public void ClearGoals()
    {
        for(int i = 0; i < OptionalGoalsContainer.childCount; i++)
        {
            Destroy(OptionalGoalsContainer.GetChild(i).gameObject);
        }
    }

    private void Awake()
    {
        ClearGoals();
        AddGoal("Procedural Goal 0");
        AddGoal("Procedural Goal 1");
        AddGoal("Procedural Goal 2");
        AddGoal("Procedural Goal 3");
    }
    private NewBoardLayout GetLevel(int levelNumber)
    {
        if (levelNumber >= 0 && levelNumber < GameManager.Instance.LevelSet.Levels.Count)
            return GameManager.Instance.LevelSet.Levels[levelNumber];
        return null;
    }
}
