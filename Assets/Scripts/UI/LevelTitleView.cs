using UnityEngine;
using UnityEngine.UI;

public class LevelTitleView : MonoBehaviour
{

    private Color[] _difficultyColors =
    {
        new Color32(180, 255, 0, 255),
        new Color32(255, 206, 0, 255),
        new Color32(255, 90, 0, 255)
    };

    public Text LevelName;
    public Text Difficulty;

    public void SetDifficulty(NewBoardLayout.LevelDifficulty difficulty)
    {
        Difficulty.color = _difficultyColors[(int)difficulty];
        Difficulty.text = difficulty.ToString().ToUpper();
    }
    public void SetAlpha(float alpha)
    {
        LevelName.color = new Color(LevelName.color.a, LevelName.color.g, LevelName.color.b, alpha);
        Difficulty.color = new Color(Difficulty.color.a, Difficulty.color.g, Difficulty.color.b, alpha);
    }
    public void SetLevel(NewBoardLayout level)
    {
        if(level != null)
        {
            LevelName.text = level.name;
            SetDifficulty(level.Difficulty);
        }
    }
}
