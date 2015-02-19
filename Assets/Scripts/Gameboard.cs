using UnityEngine;
using System.Collections.Generic;

public class Gameboard : MonoBehaviour
{
    #region Events
    public delegate void GridEvent(int x, int y);
    public event GameTile.GameTileEvent TileSettled;
    public event GameTile.GameTileEvent TileAdded;
    public event GameTile.GameTileEvent TileDestroyed;
    public event GameTile.GameTileGridMovedEvent TileMoved;
    public delegate void GameStateEvent();
    public event GameStateEvent GameboardReset;
    public event GameStateEvent GameStarted;
    public event GameStateEvent GameEnded;
    public event GameStateEvent GameRetry;
    public delegate void DirectionEvent(Direction direction);
    public event DirectionEvent WormMoveInputRecieved;
    public GameStateType GameState = GameStateType.GameOver;
    #endregion

    #region Variables
    public int Columns = 8;
    public int Rows = 9;
    public bool HopperMask = true;
    public Color BackGroundColor1 = new Color(32f / 255, 30f / 255, 24f / 255);
    public Color BackGroundColor2 = new Color(63f / 255, 51f / 255, 47f / 255);

    private GameTile[,] _tileTable;
    private int _hopperSize = 3;
    private static Gameboard _instance;
    private float _gameDuration = 0f;
    private float _prevGameDuration = 0f;
    private int _totalMoves = 0;
    private int _movesThisTry = 0;
    private int _retries = 0;
    private bool _steppedPlayback = false;
    private static Playthrough _staticPlaybackData = null;
    private Playthrough _playbackData = null;

    [HideInInspector]
    public List<GameTile> gameTiles = new List<GameTile>();
    #endregion

    #region Properties
    public static Gameboard Instance { get { return _instance; } }
    public float Left { get { return transform.position.x - (float)Columns / 2f; } }
    public float Right { get { return transform.position.x + (float)Columns / 2f; } }
    public float Top { get { return transform.position.y + (float)Rows / 2f; } }
    public float Bottom { get { return transform.position.y - (float)Rows / 2f; } }
    public float Width { get { return Right - Left; } }
    public float Height { get { return Top - Bottom; } }
    public Rectangle GridBounds { get { return new Rectangle(0, 0, Columns, Rows); } }
    public Rectangle GridBoundsWithHopper { get { return new Rectangle(0, -HopperSize, Columns, Rows + HopperSize); } }
    public int HopperSize { get { return _hopperSize; } }
    public bool AcceptingInput { get { return GameState == GameStateType.InProgress; } }
    public float GameDuration { get { return _gameDuration; } }
    public int TotalMoves { get { return _totalMoves; } }
    public int MovesThisTry { get { return _movesThisTry; } }
    public int Retries { get { return _retries; } }
    #endregion
    

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            Init();
        }
        else if(this != Instance)
        {
            Destroy(this.gameObject);
        }
    }
    private void Init()
    {
        CreateBackgroundTiles();
        _tileTable = new GameTile[Columns, Rows + _hopperSize];
        if (HopperMask)
            CreateHopperMask();

    }
    void Start()
    {
        DebugHUD.MessagesCleared += delegate
        {
            DebugHUD.Add("Moves: " + _movesThisTry);
        };
        StartGame();
    }
    void Update()
    {
        if (GameState == GameStateType.InProgress || GameState == GameStateType.ViewingPlayback)
        {
            _prevGameDuration = _gameDuration;
            _gameDuration += Time.deltaTime;
        }

        if(GameState == GameStateType.ViewingPlayback)
        {
            _playbackData.Playback(_gameDuration);
        }
    }

    public void GameOver(string GameOverMessage)
    {
        GameState = GameStateType.GameOver;
        UIGlobals.Instance.GameOverPanel.Show(GameOverMessage);
        if (GameEnded != null)
            GameEnded();
        VictoryLossConditions.Instance.Disable();
    }
    public void StartGame()
    {
        _retries = 0;
        _gameDuration = 0f;
        _totalMoves = 0;
        _movesThisTry = 0;
        _retries = 0;
        Clear();
        GameState = GameStateType.InProgress;
        UIGlobals.Instance.HideAll();
        if (GameStarted != null)
            GameStarted();
        ResetBoard();
    }
    private void ResetBoard()
    {
        if (GameboardReset != null)
            GameboardReset();
        ResourcePool.Instance.UpdateResourceCount();
        VictoryLossConditions.Instance.Enable();
    }
    public void ContinueGame()
    {
        GameState = GameStateType.InProgress;
        UIGlobals.Instance.GameOverPanel.Hide();
        VictoryLossConditions.Instance.Disable();
    }
    public void NextLevel()
    {
        BoardLayoutSet bls = GetComponent<BoardLayoutSet>();
        if(bls != null)
        {
            bls.NextLevel();
        }
        StartGame();
    }
    public void Retry()
    {
        _retries++;
        _movesThisTry = 0;
        ResetBoard();
        if (GameRetry != null)
            GameRetry();
    }
    public void ViewReplay(Playthrough playthrough, bool steppedPlayback)
    {
        _playbackData = playthrough;
        _steppedPlayback = steppedPlayback;
        _playbackData.ResetPlayback(steppedPlayback);
        _totalMoves = 0;
        _gameDuration = 0f;
        UIGlobals.Instance.GameOverPanel.Hide();
        UIGlobals.Instance.LogPlayThroughPanel.Hide();
        UIGlobals.Instance.PlaybackControls.Show(steppedPlayback);
        GameState = GameStateType.ViewingPlayback;
        if (GameboardReset != null)
            GameboardReset();
        ResourcePool.Instance.UpdateResourceCount();
        VictoryLossConditions.Instance.Enable();
    }
    public void RestartPlayback()
    {
        if(GameState == GameStateType.ViewingPlayback)
        {
            ViewReplay(_playbackData, _steppedPlayback);
        }
    }
    public void AdvanceStepInPlayback()
    {
        if (GameState == GameStateType.ViewingPlayback)
        {
            _playbackData.AdvanceStep();
        }
    }

    private void AddTileToTileTable(GameTile tile)
    {
        for (int x = 0; x < tile.Width; x++)
        {
            for (int y = 0; y < tile.Height; y++)
            {
                if (GetTileAt(tile.GridLeft + x, tile.GridTop + y) != null && !GetTileAt(tile.GridLeft + x, tile.GridTop + y).IsWorm && tile.IsWorm) continue;
                _tileTable[tile.GridLeft + x, tile.GridTop + y + _hopperSize] = tile;
            }
        }
    }
    private void BlackTileTable()
    {
        for(int x = 0; x< Columns; x++)
        {
            for(int y = 0; y < Rows; y++)
            {
                _tileTable[x, y] = null;
            }
        }
    }
    private void RemoveBoundsFromTileTable(Rectangle bounds, GameTile thisTileOnly = null)
    {
        for (int x = 0; x < bounds.width; x++)
        {
            for (int y = 0; y < bounds.height; y++)
            {
                if (!IsValidTileCoordinate(bounds.x + x, bounds.y + y, true)) continue;
                if(thisTileOnly == null || thisTileOnly == GetTileAt(bounds.x + x, bounds.y + y))
                    _tileTable[bounds.x + x, bounds.y + y + _hopperSize] = null;
            }
        }
    }

    public GameTile AddTileFromToken(Token token, Point point, bool applyGravity = true, bool ignoreChecks = false)
    {
        return AddTileFromToken(token, point.x, point.y, applyGravity, ignoreChecks);
    }
    public GameTile AddTileFromToken(Token token, int x, int y, bool applyGravity = true, bool ignoreChecks = false)
    {
        Rectangle newTileBounds = new Rectangle(x, y, token.Width, token.Height);
        if (!ignoreChecks)
        {
            if (!GridBoundsWithHopper.Contains(newTileBounds))
            {
                Debug.Log("Tile out of bounds");
                return null;
            }
            if (NumberOfTilesInBounds(newTileBounds) > 0)
            {
                Debug.Log("This tile intersects with another tile");
                return null;
            }
        }
        GameTile newTile = GenerateTileFromColoredToken(token);

        Vector3 worldPosition = GridPositionToWorldPosition(x, y);
        newTile.GridPosition = new Point(x, y);
        newTile.WorldLeft = worldPosition.x;
        newTile.WorldTop = worldPosition.y;
        newTile.SettledFromFall += NewTile_SettledFromFall;
        newTile.GridPositionMoved += NewTile_GridPositionMoved;
        gameTiles.Add(newTile);
        if(GridBoundsWithHopper.Contains(newTileBounds))
            AddTileToTileTable(newTile);
        if (applyGravity)
            newTile.ApplyGravity();
        if (TileAdded != null)
            TileAdded(newTile);
        newTile.GridPositionMoved += NewTile_GridPositionMoved1;
        return newTile;
    }
    private GameTile GenerateTileFromColoredToken(Token token)
    {
        GameObject go = new GameObject();
        go.transform.transform.SetParent(this.transform);
        GameTile result = go.AddComponent<GameTile>();
        result.TokenProperties = token;
        return result;
    }

    private void NewTile_GridPositionMoved(GameTile sender, Rectangle oldGridBounds)
    {
        RemoveBoundsFromTileTable(oldGridBounds, sender);
        if(GridBoundsWithHopper.Contains(sender.GridBounds))
            AddTileToTileTable(sender);
    }
    private void NewTile_SettledFromFall(GameTile sender)
    {
        
        if(sender.GridTop > Rows)
        {
            DestroyTile(sender);
            return;
        }
        
        if (TileSettled != null)
            TileSettled(sender);
    }

    public GameTile GetTileAt(int x, int y)
    {
        try
        {
            return _tileTable[x, y + _hopperSize];

        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            return null;
        }
    }
    public GameTile[] GetTilesInBounds(Rectangle bounds, GameTile exclusion = null)
    {
        List<GameTile> result = new List<GameTile>();
        for (int x = 0; x < bounds.width; x++)
        {
            for (int y = 0; y < bounds.height; y++)
            {
                if (IsValidTileCoordinate(bounds.x + x, bounds.y + y))
                {
                    GameTile tile = GetTileAt(bounds.x + x, bounds.y + y);
                    if (tile != null && tile != exclusion)
                        result.Add(tile);
                }
            }
        }
        return result.ToArray();
    }
    public short NumberOfTilesInBounds(Rectangle bounds, GameTile exclusion = null)
    {
        short result = 0;
        for (int x = 0; x < bounds.width; x++)
        {
            for (int y = 0; y < bounds.height; y++)
            {
                GameTile tile = GetTileAt(bounds.x + x, bounds.y + y); // tileTable[bounds.x + x, bounds.y + y + hopperSize];
                if (tile != null && tile != exclusion)
                    result++;
            }
        }
        return result;
    }
    public GameTile[] GetCardinalNeighbors(GameTile tile)
    {
        List<GameTile> result = new List<GameTile>();
        Rectangle bounds = new Rectangle(tile.GridRight, tile.GridTop, 1, tile.Height);
        result.AddRange(GetTilesInBounds(bounds));
        bounds.x = tile.GridLeft - 1;
        result.AddRange(GetTilesInBounds(bounds));
        bounds = new Rectangle(tile.GridLeft, tile.GridTop - 1, tile.Width, 1);
        result.AddRange(GetTilesInBounds(bounds));
        bounds.y = tile.GridBottom;
        result.AddRange(GetTilesInBounds(bounds));
        return result.ToArray();
    }

    public Point WorldPositionToGridPosition(Vector3 worldPosition)
    {
        float fx = worldPosition.x - Left;
        float fy = worldPosition.y - Top;

        if (fx < 0) fx -= 1;
        if (fy > 0) fy += 1;

        int x = (int)fx;
        int y = -(int)fy;
        
        return new Point(x, y);
    }
    public Vector3 GridPositionToWorldPosition(int x, int y)
    {
        return new Vector3(
            Left + x,
            Top - y, 0);
    }
    public bool IsValidTileCoordinate(Point point, bool includeHopper = false)
    {
        return IsValidTileCoordinate(point.x, point.y, includeHopper);
    }
    public bool IsValidTileCoordinate(int x, int y, bool includeHopper = false)
    {
        if (includeHopper)
            return x >= 0 && x < Columns && y >= -_hopperSize && y < Rows;
        else
            return x >= 0 && x < Columns && y >= 0 && y < Rows;
    }

    public void DestroyTile(GameTile tile, bool triggerEvent = true, bool applyGravity = true)
    {
        if(GridBoundsWithHopper.Contains(tile.GridBounds))
            RemoveBoundsFromTileTable(tile.GridBounds, tile);

        if(applyGravity)
            ApplyGravity();
        gameTiles.Remove(tile);

        if (triggerEvent && TileDestroyed != null)
            TileDestroyed(tile);
        Destroy(tile.gameObject);
    }
    public void DestroyTileAt(int x, int y, bool triggerEvent = true, bool applyGravity = true)
    {
        if(!IsValidTileCoordinate(x, y, true)) return;
        if(GetTileAt(x, y) != null)
            DestroyTile(GetTileAt(x, y), triggerEvent, applyGravity);
    }
    public void DestroyTilesInBounds(Rectangle bounds, bool triggerEvent = true, bool applyGravity = true)
    {
        for(int y = 0; y < bounds.height; y++)
        {
            for(int x = 0; x < bounds.width; x++)
            {
                DestroyTileAt(bounds.x + x, bounds.y + y, triggerEvent, applyGravity);
            }
        }
    }
    public void Clear()
    {
        while(gameTiles.Count > 0)
        {
            DestroyTile(gameTiles[0]);
        }
    }

    public void MoveWorm(Direction direction)
    {
        int x = 0;
        int y = 0;
        switch(direction)
        {
            case Direction.Left:
                x = -1;
                break;
            case Direction.Right:
                x = 1;
                break;
            case Direction.Up:
                y = -1;
                break;
            case Direction.Down:
                y = 1;
                break;
        }
        for (int i = 0; i < gameTiles.Count; i++)
        {
            GameTile tile = gameTiles[i];
            Worm worm = tile.GetComponent<Worm>();
            if (worm != null)
            {
                if (worm.Move(worm.Head.GridPosition.x + x, worm.Head.GridPosition.y + y))
                {
                    _movesThisTry++;
                    _totalMoves++;
                }
            }
        }
        if (WormMoveInputRecieved != null)
            WormMoveInputRecieved(direction);
    }

    public void ApplyGravity()
    {
        for (int y = Rows - 1; y >= -HopperSize; y--)
        {
            for (int x = 0; x < Columns; x++)
            {
                GameTile tile = GetTileAt(x, y);
                if (tile != null)
                    tile.ApplyGravity();
            }
        }
    }

    private void CreateBackgroundTiles()
    {
        GameObject backgroundTiles = new GameObject();
        backgroundTiles.name = "BackgroundTiles";
        backgroundTiles.transform.SetParent(this.transform);
        backgroundTiles.transform.localPosition = new Vector3(0, 0, 0.1f);

        for(int y = 0; y < Rows; y++)
        {
            for(int x = 0; x < Columns; x++)
            {
                GameObject backgroundTile = new GameObject();
                backgroundTile.name = "BackgroundTile";
                backgroundTile.transform.SetParent(backgroundTiles.transform);
                SpriteRenderer bgRenderer = backgroundTile.AddComponent<SpriteRenderer>();
                bgRenderer.sprite = Utils.DummySprite;
                //bgRenderer.transform.localScale = new Vector3(tileSet.TileWidth * 100, tileSet.TileHeight * 100, 1);
                bgRenderer.transform.localPosition = new Vector3(
                    Left + x + 0.5f,
                    Top - y - 0.5f
                    );
                bgRenderer.color = (x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1) ? BackGroundColor1 : BackGroundColor2;
            }
        }
    }
    private void CreateHopperMask()
    {
        GameObject hopperMask = new GameObject();
        hopperMask.name = "HopperMask";
        hopperMask.transform.SetParent(this.transform);
        SpriteRenderer sr = hopperMask.AddComponent<SpriteRenderer>();
        sr.sprite = Utils.DummySprite;
        sr.transform.localScale = new Vector3(Width * 100, Height * 100, 1);
        sr.transform.position = new Vector3(this.transform.position.x, Top + sr.transform.localScale.y / 2 / 100, -0.1f);
        sr.color = Camera.main.backgroundColor;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b);
    }

    public static void SetPlaybackData(Playthrough playthrough)
    {
        _staticPlaybackData = playthrough;
    }

    #region Delegates
    private void NewTile_GridPositionMoved1(GameTile sender, Rectangle oldGridBounds)
    {
        if (TileMoved != null)
            TileMoved(sender, oldGridBounds);
    }
    #endregion

    public enum GameStateType
    {
        InProgress,
        GameOver,
        ViewingPlayback
    }
}
