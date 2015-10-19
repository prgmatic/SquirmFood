using UnityEngine;
using System.Collections.Generic;

public class LevelPreview : MonoBehaviour
{

    public float Left { get { return transform.position.x - (float)_columns / 2f; } }
    public float Right { get { return transform.position.x + (float)_columns / 2f; } }
    public float Top { get { return transform.position.y + (float)_rows / 2f; } }
    public float Bottom { get { return transform.position.y - (float)_rows / 2f; } }

    private byte _rows;
    private byte _columns;

    private GameObject _tiles;
    private int _levelNumber;

    public void Clear()
    {
        for (int i = 0; i < _tiles.transform.childCount; i++)
        {
            Destroy(_tiles.transform.GetChild(i).gameObject);
        }
    }

    public void SetBoardSize(byte columns, byte rows)
    {
        _rows = rows;
        _columns = columns;
    }

    public void AddTile(GameObject go, int x, int y, int tileWidth, int tileHeight)
    {
        go.transform.SetParent(_tiles.transform);
        go.transform.position = GridPositionToWorldPosition(x, y);

        var spriteRenderer = go.GetComponent<SpriteRenderer>();
        var size = spriteRenderer.bounds.size;
        go.transform.position += new Vector3((float)tileWidth / 2, (float)-tileHeight / 2);
    }

    public Vector3 GridPositionToWorldPosition(int x, int y)
    {
        return new Vector3(
            Left + x,
            Top - y, this.transform.position.z);
    }

    public void LoadLevel(int levelNumber)
    {
        Clear();
        _levelNumber = levelNumber;
        var level = GetLevel(levelNumber);
        if (level == null) return;
        var prefabs = Resources.LoadAll<GameTile>("BoardPieces");
        var dict = new Dictionary<int, GameTile>();
        foreach (var prefab in prefabs)
        {
            if (!dict.ContainsKey(prefab.ID))
            {
                dict.Add(prefab.ID, prefab);
            }
        }

        if (level.Columns > 0 && level.Rows > 0)
        {
            SetBoardSize(level.Columns, level.Rows);
        }

        foreach (var tile in level.Tiles)
        {
            if (dict.ContainsKey(tile.ID))
            {
                var go = Instantiate(dict[tile.ID]).gameObject;
                var tileComponent = go.GetComponent<GameTile>();
                var tileWidth = tileComponent.Width;
                var tileHeight = tileComponent.Height;
                var wormComponenet = go.GetComponent<Worm>();
                if (wormComponenet != null)
                {
                    Destroy(wormComponenet);
                    Destroy(go.GetComponent<WormAnimationSettings>());
                    Destroy(go.GetComponent<WormAudioController>());
                }
                Destroy(tileComponent);
                AddTile(go, tile.X, tile.Y, tileWidth, tileHeight);
            }
        }
        foreach (var mudTile in level.MudTiles)
        {
            AddMudTile(mudTile % _columns, mudTile / _columns);
            //Gameboard.Instance.SetBackgroundTileAttribute(mudTile % Columns, mudTile / Columns, Gameboard.BackgroundTileAttribute.FreeMove);
        }
    }

    private void AddMudTile(int x, int y)
    {
        var go = new GameObject();
        go.name = "MudTile";
        go.layer = this.gameObject.layer;
        go.transform.SetParent(_tiles.transform);
        go.transform.position = GridPositionToWorldPosition(x, y) + new Vector3(0.5f, -0.5f);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = Utils.DummySprite;
        sr.sortingOrder = -50;
        sr.color = Color.gray;
    }

    private void Awake()
    {
        _tiles = new GameObject();
        _tiles.name = "Tiles";
        _tiles.transform.SetParent(this.transform);
    }

    private NewBoardLayout GetLevel(int levelNumber)
    {
        if (GameManager.Instance.State == GameManager.GameState.GamePaused)
        {
            var levels = GameManager.Instance.LevelSet.Levels;
            if (GameManager.Instance.CurrentLevel == _levelNumber)
            {
                return null;
            }
        }

        if (levelNumber >= 0 && levelNumber < GameManager.Instance.LevelSet.Levels.Count)
            return GameManager.Instance.LevelSet.Levels[levelNumber];
        return null;
    }
}
