using UnityEngine;
using System.Collections;

public class UIGlobals : MonoBehaviour 
{
    public static UIGlobals Instance { get { return _instance; } }
    private static UIGlobals _instance;

    public GameOverPanel GameOverPanel;
    public LogPlaythroughPanel LogPlayThroughPanel;
    public PlaybackControls PlaybackControls;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (this != _instance)
            Destroy(this.gameObject);
    }

    public void HideAll()
    {
        GameOverPanel.Hide();
        LogPlayThroughPanel.Hide();
        PlaybackControls.Hide();
    }
}
