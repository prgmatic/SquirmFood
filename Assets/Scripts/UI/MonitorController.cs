using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonitorController : MonoBehaviour
{
    public static MonitorController Instance { get; private set; }

    public GameObject MainMenuControls;
    public GameObject OptionsMenuControls;
    public AudioVolumeDisplay MusicDisplay;
    public AudioVolumeDisplay SFXDisplay;

    public void OpenMainMenu()
    {
        MainMenuControls.SetActive(true);
        OptionsMenuControls.SetActive(false);
    }

    public void OpenOptionsMenu()
    {
        MusicDisplay.SetSprite(SaveData.MusicVolume);
        SFXDisplay.SetSprite(SaveData.SFXVolume);

        OptionsMenuControls.SetActive(true);
        MainMenuControls.SetActive(false);
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
