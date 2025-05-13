using UnityEngine;
using System.Collections.Generic;

public class MoveUndoer : MonoBehaviour
{
    public static int UndoesRemaining { get; private set; }
    private List<byte[]> _boardStates = new List<byte[]>();
    int _undoStep = 0;
    private int _memoryUse = 0;

    private void Awake()
    {
        _undoStep = 0;
        Gameboard gb = Gameboard.Instance;
        gb.GameStarted += Gb_GameStarted;
        gb.WormMoveInputRecieved += Gb_WormMoved;
    }

    private void Gb_WormMoved(Direction direction, bool inputvalidated)
    {
        if (!inputvalidated)
            return;
        RecordBoardState();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            Undo();
    }

    private void RecordBoardState()
    {
        if (_undoStep < _boardStates.Count - 1)
        {
            _boardStates.RemoveRange(_undoStep + 1, _boardStates.Count - _undoStep - 1);
        }

        _undoStep++;
        _boardStates.Add(NewBoardLayout.FromGameboard().ToBinary());
        CalculateMemoryUse();
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
        //if (UndoesRemaining <= 0) return;
        if (_undoStep > 0)
        {
            UndoesRemaining--;
            _undoStep--;
            NewBoardLayout.FromBinary(_boardStates[_undoStep]).Load();
        }
    }

    private void Gb_GameStarted()
    {
        if (Gameboard.Instance.CurrentLevel != null)
            UndoesRemaining = Gameboard.Instance.CurrentLevel.NumberOfUndoes;
        else
            UndoesRemaining = 0;
        _boardStates.Clear();
        CalculateMemoryUse();
        RecordBoardState();
        _undoStep = 0;
    }
}