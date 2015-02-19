using UnityEngine;
using System.Collections.Generic;

public class PlaythroughRecorder : MonoBehaviour
{
    private List<PlaythroughAction> _data = new List<PlaythroughAction>();
    private float _time = 0;
    private bool _recording = false;

    void Awake()
    {
        Gameboard.Instance.GameboardReset += Instance_GameStarted;
    }

    private void Instance_GameStarted()
    {
        StartRecorder();
    }

    public void StartRecorder()
    {
        _recording = true;
        _time = 0f;
        Gameboard.Instance.TileAdded += Instance_TileAdded;
        Gameboard.Instance.TileDestroyed += Instance_TileDestroyed;
        Gameboard.Instance.TileMoved += Instance_TileMoved;
    }

    public void StopRecorder()
    {
        _recording = false;
        Gameboard.Instance.TileAdded -= Instance_TileAdded;
        Gameboard.Instance.TileDestroyed -= Instance_TileDestroyed;
        Gameboard.Instance.TileMoved += Instance_TileMoved;
    }

    private void Instance_TileMoved(GameTile sender, Rectangle oldGridBounds)
    {
        if(sender.GetComponent<Worm>() != null)
        {
            //InputAction tileMovedData = new InputAction();

            //tileMovedData.From = oldGridBounds.Position;
            //tileMovedData.To = sender.GridPosition;
            //_data.Add(tileMovedData);
            Debug.Log(_time);
        }
    }

    void Update()
    {
        if (_recording)
            _time += Time.deltaTime;
    }

    private void Instance_TileDestroyed(GameTile sender)
    {
    }

    private void Instance_TileAdded(GameTile sender)
    {
    }

    
}
