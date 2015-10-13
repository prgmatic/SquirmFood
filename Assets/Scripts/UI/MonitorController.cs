using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonitorController : MonoBehaviour
{
    public static MonitorController Instance { get; private set; }

    public GameObject MainMenuControls;
    public GameObject LevelSelectControls;
    public GameObject LevelSelectContent;
    public Text PlayButtonText;

    public void OpenMainMenu()
    {
        MainMenuControls.SetActive(true);
        LevelSelectControls.SetActive(false);
        UpdatePlayButtonText();
    }

    public void OpenLevelSelect()
    {
        LevelSelectControls.SetActive(true);
        MainMenuControls.SetActive(false);
        UpdateLevelSelectButtons();
    }

    public void SelectLevel(int levelNumber)
    {
        CameraPanner.Instance.PanToGameboard(levelNumber);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            OpenMainMenu();
        }
        else if (this != Instance)
            Destroy(this.gameObject);
    }

    private void UpdatePlayButtonText()
    {
        PlayButtonText.text = SaveData.CurrentLevel >= 0 ? "Continue" : "New Game";
    }

    private void UpdateLevelSelectButtons()
    {
        var buttons = LevelSelectContent.GetComponentsInChildren<LevelSelectButton2>();
        foreach(var button in buttons)
        {
            button.ButtonText.text = "Level " + (button.LevelNumber + 1);
            if (SaveData.LevelsCompleted[button.LevelNumber])
                button.ButtonText.text += " (Complete)";
        }
    }

    
}
