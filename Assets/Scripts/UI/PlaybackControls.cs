using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaybackControls : MonoBehaviour
{
    public Button NextStepButton;
    public Button ReplayButton;
	
    public void Show(bool steppedPlayback)
    {
        this.gameObject.SetActive(true);
        NextStepButton.gameObject.SetActive(steppedPlayback);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Replay()
    {
        //Gameboard.Instance.RestartPlayback();
    }

    public void NextStep()
    {
        //Gameboard.Instance.AdvanceStepInPlayback();
    }
}
