using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    public RectTransform MainPanel;
    public RectTransform HelpPanel;
    public RectTransform SettingsPanel;
    public RectTransform LevelSelectionPanel;
    public RectTransform InGameControls;
    public RectTransform GameOverPanel;
    public GameObject Game;

    private LevelSelectionPanel _levelPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void RevealMainMenu()
    {
        HideAll();
        MainPanel.gameObject.SetActive(true);
    }
    public void OpenHelp()
    {
        HideAll();
        HelpPanel.gameObject.SetActive(true);
    }
    public void OpenSettings()
    {
        HideAll();
        SettingsPanel.gameObject.SetActive(true);
    }
    public void OpenLevelSelection()
    {
        HideAll();
        LevelSelectionPanel.gameObject.SetActive(true);
    }
    public void ShowGameboard()
    {
        HideAll();
        Game.SetActive(true);
        InGameControls.gameObject.SetActive(true);
    }
    public void ShowGameOver()
    {
        HideAllPanels();
        GameOverPanel.gameObject.SetActive(true);
    }
    public void QuitGame()
    {
        Gameboard.Instance.Clear();
        RevealMainMenu();
    }

    private void HideAll()
    {
        Game.SetActive(false);
        HideAllPanels();
    }

    private void HideAllPanels()
    {
        MainPanel.gameObject.SetActive(false);
        HelpPanel.gameObject.SetActive(false);
        SettingsPanel.gameObject.SetActive(false);
        LevelSelectionPanel.gameObject.SetActive(false);
        InGameControls.gameObject.SetActive(false);
        GameOverPanel.gameObject.SetActive(false);
    }
}
