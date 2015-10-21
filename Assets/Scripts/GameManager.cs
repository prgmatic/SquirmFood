using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void StateEvent(GameState state);
    public event StateEvent StateChanged;

    public static GameManager Instance { get; private set; }
    public NewLevelSet LevelSet;
    public GameObject Game;
    public GameObject InGameControls;
    public Slider FXVolumeSlider;
    public Slider MusicVolumeSlider;
    public GameObject LevelPreviews;
    public GameObject LevelInfos;

    public int CurrentLevel
    {
        get { return SaveData.CurrentLevel; }
    }

    public GameState State
    {
        get { return _state; }
        private set
        {
            if(value != _state)
            {
                if (StateChanged != null)
                    StateChanged(value);
            }
            _state = value;
        }
    }

    private GameState _state;

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
        SaveData.Save();
        ClearMenus();
        if (State != GameState.MainMenu && State != GameState.OptionsMenu)
            CameraPanner.Instance.PanToMonitor();
        MonitorController.Instance.OpenMainMenu();
        State = GameState.MainMenu;
    }

    public void OpenOptionsMenu()
    {
        ClearMenus();
        if (State != GameState.MainMenu)
            CameraPanner.Instance.PanToMonitor();
        MonitorController.Instance.OpenOptionsMenu();
        State = GameState.OptionsMenu;
    }
    public void GoToLevelSelect()
    {
        ClearMenus();
        CameraPanner.Instance.PanToGameboard(0);
        State = GameState.LevelSelect;
    }
    public void PlayLevel(int levelNumber)
    {
        Game.Show();
        ClearMenus();
        InGameControls.Show();
        State = GameState.PlayingGame;
        Gameboard.Instance.Clear();
        Game.transform.position = Game.transform.position.SetY(-7.2f - 22f * levelNumber);
        LevelSet.Levels[levelNumber].Load();
        Gameboard.Instance.StartGame();
        CameraPanner.Instance.PanToGameboard(levelNumber);
        SaveData.CurrentLevel = levelNumber;
    }
    public void ContinueGame()
    {
        if (SaveData.CurrentLevel < 0)
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
        Game.Hide();
        LevelInfos.Show();
        LevelPreviews.Show();
        UpdateLevelViews();
        //PostLevelMenu.Show();
    }

    public void ReplayLevel()
    {
        SaveData.CurrentLevel--;
        ContinueGame();
    }
    public void PauseGame()
    {
        if (State != GameState.PlayingGame) return;
        State = GameState.GamePaused;
        ClearMenus();
        LevelInfos.Show();
        LevelPreviews.Show();
        UpdateLevelViews();
    }
    public void ResumeGame()
    {
        SaveData.Save();
        if (State != GameState.GamePaused) return;
        ClearMenus();
        InGameControls.Show();
        State = GameState.PlayingGame;
    }
    public void GoBack()
    {
        switch (State)
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
    public void SetSliderValues()
    {
        FXVolumeSlider.value = SaveData.SFXVolume;
        MusicVolumeSlider.value = SaveData.MusicVolume;
    }
    public void MusicVolumeUp()
    {
        SaveData.MusicVolume = (byte)Mathf.Min(5, SaveData.MusicVolume + 1);
        MonitorController.Instance.MusicDisplay.SetSprite(SaveData.MusicVolume);
    }
    public void MusicVolumeDown()
    {
        SaveData.MusicVolume = (byte)Mathf.Max(0, SaveData.MusicVolume - 1);
        MonitorController.Instance.MusicDisplay.SetSprite(SaveData.MusicVolume);
    }
    public void SFXVolumeUp()
    {
        SaveData.SFXVolume = (byte)Mathf.Min(5, SaveData.SFXVolume + 1);
        MonitorController.Instance.SFXDisplay.SetSprite(SaveData.SFXVolume);
    }
    public void SFXVolumeDown()
    {
        SaveData.SFXVolume = (byte)Mathf.Max(0, SaveData.SFXVolume - 1);
        MonitorController.Instance.SFXDisplay.SetSprite(SaveData.SFXVolume);
    }
    public NewBoardLayout GetLevel(int levelNumber)
    {
        if (levelNumber >= 0 && levelNumber < LevelSet.Levels.Count)
            return LevelSet.Levels[levelNumber];
        return null;
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
        Game.Hide();
        LevelInfos.Show();
        LevelPreviews.Show();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GoBack();
    }
    private void ClearMenus()
    {
        InGameControls.Hide();
        LevelInfos.Hide();
        LevelPreviews.Hide();
    }
    private void UpdateLevelViews()
    {
        LevelPreviews.GetComponent<LevelPreviewPanner>().UpdatePreview();
        LevelInfos.GetComponent<LevelInfoPanner>().UpdateInfo();
    }

    public enum GameState
    {
        StartScreen,
        MainMenu,
        OptionsMenu,
        LevelSelect,
        PlayingGame,
        PostLevel,
        GamePaused
    }
}
