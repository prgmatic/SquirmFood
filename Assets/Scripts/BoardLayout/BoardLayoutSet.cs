using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class BoardLayoutSet : MonoBehaviour 
{

    public List<ToggleableBoardLayout> BoardLayouts = new List<ToggleableBoardLayout>();

    private int _currentBoardIndex = -1;
    private ToggleableBoardLayout _currentBoardLayout { get { return BoardLayouts[_currentBoardIndex]; } }

    void Awake()
    {
        Gameboard.Instance.GameboardReset += Instance_GameboardReset;
        Gameboard.Instance.GameStarted += Instance_GameStarted;

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



    #region Delegates
    private void Instance_GameStarted()
    {
        if (!this.enabled || _currentBoardIndex < 0) return;
        if (BoardLayouts.Count > 0)
        {
            Gameboard.Instance.CurrentLevel = BoardLayouts[_currentBoardIndex].Layout;
        }
    }

    private void Instance_GameboardReset()
    {
        /*
        if (!this.enabled || _currentBoardIndex < 0) return;
        if (BoardLayouts.Count > 0)
        {
            BoardLayoutImporter.ImportBoardLayout(BoardLayouts[_currentBoardIndex].Layout);
        }
        */
    }
    #endregion

    [System.Serializable]
    public class ToggleableBoardLayout
    {
        public BoardLayout Layout;
        public bool Enabled = true;
    }
}
