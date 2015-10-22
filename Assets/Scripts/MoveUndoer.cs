using UnityEngine;
using System.Collections.Generic;

public class MoveUndoer : MonoBehaviour
{

    public static int UndoesRemaining { get; private set; }
    private List<byte[]> _boardStates = new List<byte[]>();
    int _undoStep = 0;
    private bool _recordState = false;
    private bool _listenToEvents = true;
    private int _memoryUse = 0;

    private void Awake()
    {
		_undoStep = 0;
        Gameboard gb = Gameboard.Instance;
        gb.GameStarted += Gb_GameStarted;
        gb.TileMoved += Gb_TileMoved;
    }
    private void LateUpdate()
    {
        if (_recordState == true)
        {
            if (_undoStep < _boardStates.Count - 1)
            {
                _boardStates.RemoveRange(_undoStep + 1, _boardStates.Count - _undoStep - 1);
            }
            _undoStep++;
            _boardStates.Add(NewBoardLayout.FromGameboard().ToBinary());
            _recordState = false;
            CalculateMemoryUse();
        }
        if (Input.GetKeyDown(KeyCode.Z))
            Undo();
    }
    private void MarkToRecord()
    {
        if (_listenToEvents)
            _recordState = true;
    }
    private void CalculateMemoryUse()
    {
        _memoryUse = 0;
        foreach (var state in _boardStates)
        {
            _memoryUse += state.Length;
        }
    }
    public void Undo()
    {
        if (UndoesRemaining <= 0) return;
        if (_undoStep > 0)
        {
            UndoesRemaining--;
            _listenToEvents = false;
            _undoStep--;
            NewBoardLayout.FromBinary(_boardStates[_undoStep]).Load();
            _listenToEvents = true;
        }
    }

    private void Gb_GameStarted()
    {
        if (Gameboard.Instance.CurrentLevel != null)
            UndoesRemaining = Gameboard.Instance.CurrentLevel.NumberOfUndoes;
        else
            UndoesRemaining = 0;
        MarkToRecord();
        _boardStates.Clear();
        CalculateMemoryUse();
        _undoStep = -1;
    }
    private void Gb_TileMoved(GameTile sender, Rectangle oldGridBounds)
    {
        MarkToRecord();
    }


}
