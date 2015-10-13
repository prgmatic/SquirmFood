using UnityEngine;
using System.Collections;

public class MonitorController : MonoBehaviour
{
    public static MonitorController Instance { get; private set; }

    public GameObject MainMenuControls;
    public GameObject LevelSelectControls;

    public void OpenMainMenu()
    {
        MainMenuControls.SetActive(true);
        LevelSelectControls.SetActive(false);
    }

    public void OpenLevelSelect()
    {
        LevelSelectControls.SetActive(true);
        MainMenuControls.SetActive(false);
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

    
}
