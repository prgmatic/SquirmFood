using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverPanel : MonoBehaviour 
{
    public Text GameOverText;
    public Button NextLevelButton;
    public Button LogPlaythroughButton;
    private PlayMakerFSM fsm;
    private HutongGames.PlayMaker.FsmState gameOverState;

    void Awake()
    {
        fsm = Gameboard.Instance.GetComponent<PlayMakerFSM>();
        gameOverState = fsm.Fsm.GetState("GameOverScreen");
    }

    void Update()
    {
        if(fsm.Fsm.ActiveState != gameOverState)
        {
            NextLevelButton.gameObject.SetActive(false);
            LogPlaythroughButton.gameObject.SetActive(false);
        }
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
