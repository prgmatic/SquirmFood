using UnityEngine;
using System.Collections.Generic;

public class VictoryLossConditions : MonoBehaviour 
{
    public static VictoryLossConditions Instance { get { return _instance; } }
    private static VictoryLossConditions _instance;

    private bool CheckEnabled = false;
    private PlayMakerFSM gameboardFsm;

    public List<GameCondition> VictoryConditions = new List<GameCondition>();
    public List<GameCondition> LossConditions = new List<GameCondition>();

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (this != _instance)
            Destroy(this.gameObject);
        this.CheckEnabled = false;
        gameboardFsm = Gameboard.Instance.GetComponent<PlayMakerFSM>();
    }

    void Update()
    {
        if (CheckEnabled && (Gameboard.Instance.GameState == Gameboard.GameStateType.InProgress || Gameboard.Instance.GameState == Gameboard.GameStateType.ViewingPlayback))
        {
            CheckForVictory();
            CheckForLoss();
        }
    }

    private void CheckForVictory()
    {
        foreach(var condition in VictoryConditions)
        {
            if(condition.ConditionMet())
            {
                if (Gameboard.Instance.GameState == Gameboard.GameStateType.InProgress)
                    gameboardFsm.Fsm.Event("Victory");
                    
                /*
                if(Gameboard.Instance.GameState == Gameboard.GameStateType.InProgress)
                    Gameboard.Instance.GameOver("You Win");
                if (Gameboard.Instance.GameState == Gameboard.GameStateType.ViewingPlayback)
                    Gameboard.Instance.StartGame();
                    */
            }
        }
    }

    private void CheckForLoss()
    {
        foreach (var condition in LossConditions)
        {
            if (condition.ConditionMet())
            {
                gameboardFsm.Fsm.Event("Loss");
                //Gameboard.Instance.GameOver("Game Over");
            }
        }
    }

    public void Enable()
    {
        CheckEnabled = true;
    }

    public void Disable()
    {
        CheckEnabled = false;
    }
}
