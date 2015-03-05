using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverPanel : MonoBehaviour 
{
    public Text GameOverText;
    public Button NextLevelButton;
    public PlayMakerFSM fsm;

    void Awake()
    {
        fsm = Gameboard.Instance.GetComponent<PlayMakerFSM>();
    }

    void OnEnable()
    {
        BoardLayoutSet bls = Gameboard.Instance.GetComponent<BoardLayoutSet>();
        if (bls != null && bls.enabled)
        {
            NextLevelButton.gameObject.SetActive(true);
        }
        else NextLevelButton.gameObject.SetActive(false);
    }

    public void SetGameOverMessage(string msg)
    {
        this.GameOverText.text = msg;
    }

    public void GoToLevelSelection()
    {
        fsm.Fsm.Event("GoToLevelSelection");
    }
    public void RestartLevel()
    {
        fsm.Fsm.Event("RestartLevel");
    }

    public void GoToNextLevel()
    {
        fsm.Fsm.Event("GoToNextLevel");
    }
    public void LogPlaythrough()
    {
        fsm.Fsm.Event("OpenPlaythroughSubmission");
    }
    
}
