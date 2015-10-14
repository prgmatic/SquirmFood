using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public NewLevelSet LevelSet;
    public Transform Game;
    public GameObject InGameControls;
    public GameObject PostLevelMenu;

    [System.NonSerialized]
    public GameState State;

    public void WarpToStart()
    {
        ClearMenus();
        State = GameState.StartScreen;
        CameraPanner.Instance.WarpCameraToStart();
    }

    public void GoToStart()
    {
        ClearMenus();
        State = GameState.StartScreen;
        CameraPanner.Instance.PanToStart();
    }
    public void GoToMainMenu()
    {
        ClearMenus();
        if (State != GameState.MainMenu && State != GameState.LevelSelect)
            CameraPanner.Instance.PanToMonitor();
        MonitorController.Instance.OpenMainMenu();
        State = GameState.MainMenu;
    }
    public void GoToLevelSelect()
    {
        ClearMenus();
        if (State != GameState.MainMenu && State != GameState.LevelSelect)
            CameraPanner.Instance.PanToMonitor();
        MonitorController.Instance.OpenLevelSelect();
        State = GameState.LevelSelect;
    }
    public void PlayLevel(int levelNumber)
    {
        ClearMenus();
        ShowInGameControls();
        State = GameState.PlayingGame;
        Gameboard.Instance.Clear();
        Game.transform.position = Game.transform.position.SetY(-7.2f - 24f * levelNumber);
        LevelSet.Levels[levelNumber].Load();
        Gameboard.Instance.StartGame();
        CameraPanner.Instance.PanToGameboard(levelNumber);
        SaveData.CurrentLevel = levelNumber;
    }
    public void ContinueGame()
    {
        if(SaveData.CurrentLevel < 0)
        {
            SaveData.CurrentLevel = 0;
        }
        PlayLevel(SaveData.CurrentLevel);
    }
    public void LevelComplete()
    {
        Debug.Log("Show complete menu");
        State = GameState.PostLevel;
        SaveData.LevelsCompleted[SaveData.CurrentLevel] = true;
        SaveData.CurrentLevel++;
        SaveData.Save();
        ClearMenus();
        ShowPostLevelMenu();
    }
    public void ReplayLevel()
    {
        SaveData.CurrentLevel--;
        ContinueGame();
    }
    public void GoBack()
    {
        switch(State)
        {
            case GameState.MainMenu:
                GoToStart();
                break;
            case GameState.LevelSelect:
                GoToMainMenu();
                break;
            case GameState.PlayingGame:
                GoToMainMenu();
                break;
        }
    }
    public void DeleteSave()
    {
        SaveData.Delete();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
        }
        else if (this != Instance)
            Destroy(this.gameObject);
    }
    private void Init()
    {
        SaveData.Load();
        WarpToStart();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GoBack();
    }
    private void ShowInGameControls()
    {
        InGameControls.SetActive(true);
    }
    private void HideInGameControls()
    {
        InGameControls.SetActive(false);
    }
    private void ShowPostLevelMenu()
    {
        PostLevelMenu.SetActive(true);
    }
    private void HidePostLevelMenu()
    {
        PostLevelMenu.SetActive(false);
    }
    private void ClearMenus()
    {
        HideInGameControls();
        HidePostLevelMenu();
    }

    public enum GameState
    {
        StartScreen,
        MainMenu,
        LevelSelect,
        PlayingGame,
        PostLevel
    }
}
