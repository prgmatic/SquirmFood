using UnityEngine;
using System.Collections.Generic;

public class PlaythroughViewer : MonoBehaviour
{
    public delegate void PlaybackEndedEvent();
    public event PlaybackEndedEvent PlaybackEnded;

    public PlaybackType Playback { get { return _playbackType; } }
    private List<PlaythroughAction> _actions { get { return _playthroughActions; } }
    private PlaythroughAction _currentAction { get { return _actions[_currentStep]; } }
    private float _timer { get { return Gameboard.Instance.GameDuration; } }

    private PlaybackType _playbackType = PlaybackType.None;
    private List<PlaythroughAction> _playthroughActions = null;
    private int _currentStep = 0;
    private PlayMakerFSM _fsm;

    void Awake()
    {
        _fsm = Gameboard.Instance.GetComponent<PlayMakerFSM>();
    }

    public void ViewPlaythrough(List<PlaythroughAction> playthroughActions, BoardLayout boardLayout, PlaybackType playbackType)
    {
        //_currentStep = 0;
        //_playthroughActions = playthroughActions;
        //_playbackType = playbackType;
        //Gameboard.Instance.GetComponent<BoardLayoutSet>().enabled = false;
        //Gameboard.Instance.EndGame();
        //Gameboard.Instance.Show();
        //Gameboard.Instance.GameState = Gameboard.GameStateType.ViewingPlayback;
        //Gameboard.Instance.CurrentLevel = boardLayout;
        //Gameboard.Instance.StartGame();
        //_fsm.Fsm.Event("ViewPlaythrough");
    }

    void Update()
    {
        if (Playback == PlaybackType.None || Playback == PlaybackType.Stepped) return;
        if (_currentStep < _actions.Count && _timer >= _actions[_currentStep].Time)
        {
            ExecuteStep(_currentStep);
            _currentStep++;
        }
    }

    public void AdvanceStep()
    {
        while (_actions[_currentStep] is InputAction && !((InputAction)_currentAction).InputValidated && _currentStep < _actions.Count)
        {
            _currentStep++;
        }
        if (_currentStep >= _actions.Count) return;
        ExecuteStep(_currentStep);
        _currentStep++;
    }

    private void ExecuteStep(int index)
    {
        PlaythroughAction action = _actions[index];
        if (action is InputAction)
        {
            Gameboard.Instance.MoveWorm(((InputAction)action).Direction);
        }
        else if (action is RetryAction)
        {
            Gameboard.Instance.Retry();
        }
    }

    public enum PlaybackType
    {
        Stepped,
        Realtime,
        None
    }
}
