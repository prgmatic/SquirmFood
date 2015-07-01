using UnityEngine;
using System.Collections.Generic;

public class BuildRecorder : MonoBehaviour
{
    private List<byte[]> boardStates = new List<byte[]>();
    int undoStep = 0;
    private bool recordState = false;
    private bool listenToEvents = true;
    private int _memoryUse = 0;

    void Awake()
    {
		undoStep = 0;
        Gameboard gb = Gameboard.Instance;
        gb.GameStarted += Gb_GameStarted;
        gb.TileAdded += Gb_TileAdded;
        gb.TileDestroyed += Gb_TileDestroyed;
        gb.TileMoved += Gb_TileMoved;
        gb.GameboardReset += Gb_GameboardReset;
        gb.GameEnded += Gb_GameEnded;
		gb.TileAttributeChanged += Gb_TileAttributeChanged;
        DebugHUD.MessagesCleared += DebugHUD_MessagesCleared;
    }

	

	private void Gb_GameEnded()
    {
        boardStates.Clear();
        CalculateMemoryUse();
    }

    private void DebugHUD_MessagesCleared(object sender, System.EventArgs e)
    {
        DebugHUD.Add(string.Format("Undo Memory: {0}kb", System.Math.Round((double)_memoryUse / 1000, 1)));
    }

    void LateUpdate()
    {
        if(recordState == true)
        {
            if(undoStep < boardStates.Count -1)
            {
                boardStates.RemoveRange(undoStep + 1, boardStates.Count - undoStep - 1);
            }
            undoStep++;
            boardStates.Add(NewBoardLayout.FromGameboard().ToBinary());
            recordState = false;
            CalculateMemoryUse();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Undo();
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Redo();
        }
    }


    public void Undo()
    {
        if (undoStep > 0)
        {
            listenToEvents = false;
            undoStep--;
            NewBoardLayout.FromBinary(boardStates[undoStep]).Load();
            listenToEvents = true;
        }
    }

    public void Redo()
    {
        if (undoStep < boardStates.Count - 1)
        {
            listenToEvents = false;
            undoStep++;
            NewBoardLayout.FromBinary(boardStates[undoStep]).Load();
            listenToEvents = true;
        }
    }

    private void MarkToRecord()
    {
        if (listenToEvents)
            recordState = true;
    }

    private void Gb_GameboardReset()
    {
        MarkToRecord();
    }

    private void Gb_TileMoved(GameTile sender, Rectangle oldGridBounds)
    {
        MarkToRecord();
    }

    private void Gb_TileDestroyed(GameTile sender)
    {
        MarkToRecord();
    }

    private void Gb_TileAdded(GameTile sender)
    {
        MarkToRecord();
    }
	private void Gb_TileAttributeChanged(int x, int y, Gameboard.BackgroundTileAttribute attribute)
	{
		MarkToRecord();
	}

	private void Gb_GameStarted()
    {
        MarkToRecord();
        boardStates.Clear();
        CalculateMemoryUse();
        undoStep = -1;
    }

    private void CalculateMemoryUse()
    {
        _memoryUse = 0;
        foreach(var state in boardStates)
        {
            _memoryUse += state.Length;
        }
    }
}
