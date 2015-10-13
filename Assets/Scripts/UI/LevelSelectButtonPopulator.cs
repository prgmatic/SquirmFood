using UnityEngine;
using System.Collections;

public class LevelSelectButtonPopulator : MonoBehaviour
{
    public LevelSelectButton2 ButtonPrefab;

    private void Awake()
    {
        PopulateButtons();
    }

    private void ClearContent()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    private void PopulateButtons()
    {
        var levelSet = GameManager.Instance.LevelSet;
        ClearContent();
        if (levelSet != null)
        {
            int levelNum = 1;
            for (int i = 0; i < levelSet.Levels.Count; i++)
            {
                var button = (LevelSelectButton2)Instantiate(ButtonPrefab);
                button.ButtonText.text = "Level " + levelNum.ToString();
                button.transform.SetParent(this.transform, false);
                button.LevelNumber = i;
                button.LevelSelected += Button_LevelSelected;
                button.transform.localScale = new Vector3(1, 1, 1);
                levelNum++;
            }
        }
    }

    private void Button_LevelSelected(int levelNumber)
    {
        GameManager.Instance.PlayLevel(levelNumber);
    }
}
