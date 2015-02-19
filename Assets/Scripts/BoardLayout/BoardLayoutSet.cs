using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class BoardLayoutSet : MonoBehaviour 
{

    public List<ToggleableBoardLayout> BoardLayouts = new List<ToggleableBoardLayout>();
    private List<PlaythroughAction> _playthroughActions = new List<PlaythroughAction>();
    public string PlayTesterName = "Anonymous";
    public bool AutoLogPlaytest = true;


    private int _playthroughID = -1;
    private int _currentBoardIndex = -1;
    private ToggleableBoardLayout _currentBoardLayout { get { return BoardLayouts[_currentBoardIndex]; } }

    void Awake()
    {
        Gameboard.Instance.GameboardReset += Instance_GameboardReset;
        Gameboard.Instance.WormMoveInputRecieved += Instance_WormMoveInputRecieved;
        Gameboard.Instance.GameRetry += Instance_GameRetry;
        Gameboard.Instance.GameStarted += Instance_GameStarted;
        Gameboard.Instance.GameEnded += Instance_GameEnded;

        for(int i = 0; i < BoardLayouts.Count; i++)
        {
            if (BoardLayouts[i].Enabled)
            {
                _currentBoardIndex = i;
                break;
            }
        }
    }
    void Start() { }

    public void NextLevel()
    {
        var enabledLayouts = (from layout in BoardLayouts
                   where layout.Enabled
                   select layout).ToList();
        if (enabledLayouts.Count > 0)
        {
            int enabledIndex = enabledLayouts.IndexOf(BoardLayouts[_currentBoardIndex]);
            enabledIndex++;
            if (enabledIndex >= enabledLayouts.Count) enabledIndex = 0;
            _currentBoardIndex = BoardLayouts.IndexOf(enabledLayouts[enabledIndex]);
        }
        else
        {
            _currentBoardIndex = 0;
        }
    }
    public void SetLayout(BoardLayout layout)
    {
        int index = -1;
        for(int i = 0; i < BoardLayouts.Count; i++)
        {
            if(layout == BoardLayouts[i].Layout)
            {
                index = i;
                break;
            }
        }
        if (index >= 0) _currentBoardIndex = index;
    }
    public void AddPlaythrough(string name, string notes, int difficulty, int satisfaction)
    {
        if (_playthroughID > 0)
        {
            PlaythroughDatabase.Insstance.AddFeedbackToPlaythrough(_playthroughID, difficulty, satisfaction, notes);
        }
        else
        {
            string sceneGUID = SceneGUID.Instance.GUID;
            string layoutGUID = _currentBoardLayout.Layout.GUID;
            var playthrough = new Playthrough(name,
                sceneGUID,
                layoutGUID,
                notes,
                difficulty,
                satisfaction,
                Gameboard.Instance.GameDuration,
                Gameboard.Instance.TotalMoves,
                Gameboard.Instance.MovesThisTry,
                Gameboard.Instance.Retries, _playthroughActions);
            PlaythroughDatabase.Insstance.PlaythroughSubmited += Insstance_PlaythroughSubmited;
            PlaythroughDatabase.Insstance.PostPlaythrough(playthrough);
        }
    }

    

    #region Delegates
    private void Instance_GameEnded()
    {
        if(AutoLogPlaytest)
        {
            AddPlaythrough(PlayTesterName, "", 0, 0);
        }
    }
    private void Instance_GameboardReset()
    {
        if (!this.enabled || _currentBoardIndex < 0) return;
        if (BoardLayouts.Count > 0)
        {
            BoardLayoutImporter.ImportBoardLayout(BoardLayouts[_currentBoardIndex].Layout.Tokens);
        }
    }
    private void Instance_GameStarted()
    {
        _playthroughID = -1;
        _playthroughActions.Clear();
    }
    private void Instance_GameRetry()
    {
        _playthroughActions.Add(new RetryAction(Gameboard.Instance.GameDuration));
    }
    private void Instance_WormMoveInputRecieved(Direction direction)
    {
        _playthroughActions.Add(new InputAction(direction, Gameboard.Instance.GameDuration));
    }
    private void Insstance_PlaythroughSubmited(int playthroughID)
    {
        PlaythroughDatabase.Insstance.PlaythroughSubmited -= Insstance_PlaythroughSubmited;
        this._playthroughID = playthroughID;
    }
    #endregion

    [System.Serializable]
    public class ToggleableBoardLayout
    {
        public BoardLayout Layout;
        public bool Enabled = true;
    }
}
