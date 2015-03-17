using UnityEngine;
using System.Collections.Generic;

public class BuildRecorder : MonoBehaviour
{
    private List<byte[]> boardStates = new List<byte[]>();
    int undoStep = 0;
    private bool recordState = false;
    private bool listenToEvents = true;
    PlayMakerFSM fsm;
    HutongGames.PlayMaker.FsmState buildingState;
    private int _memoryUse = 0;

    void Awake()
    {
        Gameboard gb = Gameboard.Instance;
        fsm = Gameboard.Instance.GetComponent<PlayMakerFSM>();
        buildingState = fsm.Fsm.GetState("Building");
        gb.GameStarted += Gb_GameStarted;
        gb.TileAdded += Gb_TileAdded;
        gb.TileDestroyed += Gb_TileDestroyed;
        gb.TileMoved += Gb_TileMoved;
        gb.GameboardReset += Gb_GameboardReset;
        gb.GameEnded += Gb_GameEnded;
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
        if(fsm.Fsm.ActiveState != buildingState)
            return;
        if(recordState == true)
        {
            if(undoStep < boardStates.Count -1)
            {
                boardStates.RemoveRange(undoStep + 1, boardStates.Count - undoStep - 1);
            }
            undoStep++;
            boardStates.Add(BoardLayoutExporter.ExportBinary());
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
            BoardLayoutImporter.ImportBinary(boardStates[undoStep]);
            listenToEvents = true;
        }
    }

    public void Redo()
    {
        if (undoStep < boardStates.Count - 1)
        {
            listenToEvents = false;
            undoStep++;
            BoardLayoutImporter.ImportBinary(boardStates[undoStep]);
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
