using UnityEngine;
using System.Collections.Generic;

public class PlaythroughRecorder : MonoBehaviour
{
    public bool AutoLogPlaythrough = false;

    private List<PlaythroughAction> _data = new List<PlaythroughAction>();
    private int _playthroughID = -1;

    void OnEnable()
    {
        StartRecorder();
    }
    void OnDisable()
    {
        StopRecorder();
    }

    public void StartRecorder()
    {
        Debug.Log("Recording Started");
        _data.Clear();
        _playthroughID = -1;
        Gameboard.Instance.WormMoveInputRecieved += Instance_WormMoveInputRecieved;
        Gameboard.Instance.GameRetry += Instance_GameRetry;
        Gameboard.Instance.GameEnded += Instance_GameEnded;
    }
    public void StopRecorder()
    {
        Debug.Log("Recording Stopped");
        Gameboard.Instance.WormMoveInputRecieved -= Instance_WormMoveInputRecieved;
        Gameboard.Instance.GameRetry -= Instance_GameRetry;
        Gameboard.Instance.GameEnded -= Instance_GameEnded;
        //if(AutoLogPlaythrough && _data.Count > 0)
        //{
            //SubmitPlaythrough();
        //}
    }

    //public void SubmitPlaythrough()
    //{
    //    SubmitPlaythrough("", "", 0, 0);
    //}

    //public void SubmitPlaythrough(string name, string notes, int difficulty, int satisfaction)
    //{
    //    if (_playthroughID > 0)
    //    {
    //        WebManager.Instance.AddFeedbackToPlaythrough(_playthroughID, difficulty, satisfaction, notes);
    //    }
    //    else
    //    {
    //        int levelID = Gameboard.Instance.CurrentLevel.ID;
    //        var playthrough = new Playthrough(
    //            levelID,
    //            name,
    //            ValidateKey.Key,
    //            notes,
    //            difficulty,
    //            satisfaction,
    //            Gameboard.Instance.GameDuration,
    //            Gameboard.Instance.TotalMoves,
    //            Gameboard.Instance.MovesThisTry,
    //            Gameboard.Instance.Retries, _data);
    //        WebManager.Instance.PlaythroughSubmited += Insstance_PlaythroughSubmited;
    //        WebManager.Instance.PostPlaythrough(playthrough);
    //    }
    //}

    //public Playthrough ExportPlaythrough()
    //{
    //    return new Playthrough(
    //            Gameboard.Instance.CurrentLevel.ID,
    //            name,
    //            ValidateKey.Key,
    //            "",
    //            -1,
    //            -1,
    //            Gameboard.Instance.GameDuration,
    //            Gameboard.Instance.TotalMoves,
    //            Gameboard.Instance.MovesThisTry,
    //            Gameboard.Instance.Retries, _data);
    //}


    #region Delegates
    private void Instance_WormMoveInputRecieved(Direction direction, bool inputValidated)
    {
        _data.Add(new InputAction(direction, Gameboard.Instance.GameDuration, inputValidated));
    }
    private void Instance_GameRetry()
    {
        _data.Add(new RetryAction(Gameboard.Instance.GameDuration));
    }
    private void Instance_GameEnded()
    {
        Debug.Log("The game has ended");
    }
    #endregion









}
