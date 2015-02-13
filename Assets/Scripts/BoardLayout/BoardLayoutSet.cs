using UnityEngine;
using System.Collections.Generic;

public class BoardLayoutSet : MonoBehaviour 
{
    public List<BoardLayout> BoardLayouts = new List<BoardLayout>();
    private int _currentBoardIndex = 0;

    void Awake()
    {
        Gameboard.Instance.GameStarted += Instance_GameStarted;
    }

    public void NextLevel()
    {
        _currentBoardIndex++;
        if (_currentBoardIndex == BoardLayouts.Count) _currentBoardIndex = 0;
    }

    private void Instance_GameStarted()
    {
        if (!this.enabled) return;
        if(BoardLayouts.Count > 0)
        {
            BoardLayoutImporter.ImportBoardLayout(BoardLayouts[_currentBoardIndex].Tokens);
        }
    }

    void Start()
    {
    }
}
