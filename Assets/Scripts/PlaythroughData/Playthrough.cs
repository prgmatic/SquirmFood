using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Playthrough
{
    public int ID = -1;
    public int LevelID = -1;
    public string TesterName = "Anonymous";
    public string TesterKey = "";
    public float DurationInSeconds = 0f;
    public int DifficultyRating = 0;
    public int SatisfactionRating = 0;
    public string Notes = "";
    public double AODate = 0;
    public int TotalMoves = 0;
    public int MovesOnWin = 0;
    public int Retries = 0;
    public System.DateTime DateTime { get { return System.DateTime.FromOADate(AODate); } set { AODate = value.ToOADate(); } }
    public System.TimeSpan Duration { get { return System.TimeSpan.FromSeconds(DurationInSeconds); } set { DurationInSeconds = (float)value.TotalSeconds; } }

    private int _playIndex = 0;
    private bool _steppedPlayback = false;
    private PlaythroughAction currentAction { get { return Actions[_playIndex]; } }

    public List<PlaythroughAction> Actions;
    public Playthrough()
    {

    }
    public Playthrough(int levelID, string testerName, string testerKey, string notes, int difficulty, int satisfaction, float duration, int totalMoves, int movesOnWin, int reties, List<PlaythroughAction> actions)
    {
        this.LevelID = levelID;
        this.TesterName = testerName;
        this.TesterKey = testerKey;
        this.Notes = notes;
        this.DifficultyRating = difficulty;
        this.SatisfactionRating = satisfaction;
        this.DateTime = System.DateTime.Now;
        this.DurationInSeconds = duration;
        this.TotalMoves = totalMoves;
        this.MovesOnWin = movesOnWin;
        this.Retries = reties;

        Actions = actions;
    }

    public void ResetPlayback(bool steppedPlayback)
    {
        _steppedPlayback = steppedPlayback;
        _playIndex = 0;
    }

    public void AdvanceStep()
    {
        while (Actions[_playIndex] is InputAction && !((InputAction)currentAction).InputValidated && _playIndex < Actions.Count)
        {
            _playIndex++;
        }
        if (_playIndex >= Actions.Count) return;
        ExecuteStep(_playIndex);
        _playIndex++;
    }
    public void Playback(float time)
    {
        if (_steppedPlayback) return;
        if(_playIndex < Actions.Count && time >= Actions[_playIndex].Time)
        {
            ExecuteStep(_playIndex);
            _playIndex++;
        }
    }

    private void ExecuteStep(int index)
    {
        PlaythroughAction action = Actions[index];
        if (action is InputAction)
        {
            Gameboard.Instance.MoveWorm(((InputAction)action).Direction);
        }
        else if (action is RetryAction)
        {
            Gameboard.Instance.Retry();
        }
    }
}
