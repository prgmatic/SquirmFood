using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    public delegate void InputEvent(Direction direction, bool inputValidated);
    public event InputEvent WormMoveInputRecieved;
    public delegate void TileAttributeEvent(int x, int y, BackgroundTileAttribute attribute);
    public event TileAttributeEvent TileAttributeChanged;
    #endregion

    #region Variables
    public bool DoKeyValidation = true;
    public bool ShowLevelSelectionScreenOnStartup = true;
    public int Columns = 8;
    public int Rows = 10;

    private GameTile[,] _tileTable;
    private BackgroundTileAttribute[,] _backgroundTileAttributes;
    private static Gameboard _instance;
    private float _gameDuration = 0f;
    private int _totalMoves = 0;
    private int _movesThisTry = 0;
    private int _retries = 0;
    private Playthrough _playbackData = null;
    private NewBoardLayout _currentLevel;

    [System.NonSerialized]
    public List<GameTile> gameTiles = new List<GameTile>();
    [System.NonSerialized]
    public bool Validated = true;
    [System.NonSerialized]
    public GameStateType GameState = GameStateType.GameOver;
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
    public bool AcceptingInput { get { return GameState == GameStateType.InProgress; } }
    public float GameDuration { get { return _gameDuration; } }
    public int TotalMoves { get { return _totalMoves; } }
    public int MovesThisTry { get { return _movesThisTry; } }
    public int Retries { get { return _retries; } }
    public NewBoardLayout CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }
    #endregion


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Init();
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    private void Init()
    {
        _backgroundTileAttributes = new BackgroundTileAttribute[Columns, Rows];
        _tileTable = new GameTile[Columns, Rows];
        CurrentLevel = LevelSelectionInfo.Level;
    }
    void Start()
    {
        StartGame();
    }
    void Update()
    {
        if (GameState == GameStateType.InProgress || GameState == GameStateType.ViewingPlayback)
        {
            _gameDuration += Time.deltaTime;
        }
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
        if (GameStarted != null)
            GameStarted();
        ResetBoard();
    }
    public void EndGame()
    {
        GameState = GameStateType.GameOver;
        if (GameEnded != null)
            GameEnded();
    }
    private void ResetBoard()
    {
        if (GameboardReset != null)
            GameboardReset();
        if (CurrentLevel != null)
            CurrentLevel.Load();
            //BoardLayoutImporter.ImportBoardLayout(CurrentLevel);
    }
    public void Retry()
    {
        _retries++;
        _movesThisTry = 0;
        ResetBoard();
        if (GameRetry != null)
            GameRetry();
    }
    public void SetBoardSize(int columns, int rows)
    {
        Clear();
        this.Columns = columns;
        this.Rows = rows;

        _backgroundTileAttributes = new BackgroundTileAttribute[Columns, Rows];
        _tileTable = new GameTile[Columns, Rows];
    }
    private void AddTileToTileTable(GameTile tile)
    {
        for (int x = 0; x < tile.Width; x++)
        {
            for (int y = 0; y < tile.Height; y++)
            {
                if (GetTileAt(tile.GridLeft + x, tile.GridTop + y) != null && !GetTileAt(tile.GridLeft + x, tile.GridTop + y).IsWorm && tile.IsWorm) continue;
                _tileTable[tile.GridLeft + x, tile.GridTop + y] = tile;
            }
        }
    }
    private void BlackTileTable()
    {
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
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
                if (thisTileOnly == null || thisTileOnly == GetTileAt(bounds.x + x, bounds.y + y))
                    _tileTable[bounds.x + x, bounds.y + y] = null;
            }
        }
    }

    public void AddGameTile(GameTile tile, int x, int y, bool applyGravity = true)
    {
        Rectangle newTileBounds = new Rectangle(x, y, tile.Width, tile.Height);

        if (!GridBounds.Contains(newTileBounds))
        {
            Debug.Log("Tile out of bounds");
            Destroy(tile.gameObject);
            return;
        }
        if (NumberOfTilesInBounds(newTileBounds) > 0)
        {
            Debug.Log("This tile intersects with another tile");
            Destroy(tile.gameObject);
            return;
        }

        Vector3 worldPosition = GridPositionToWorldPosition(x, y);
        tile.transform.SetParent(this.transform);
        //tile.gameObject.layer = this.gameObject.layer;
        tile.GridPosition = new Point(x, y);
        tile.WorldLeft = worldPosition.x;
        tile.WorldTop = worldPosition.y;
        tile.SettledFromFall += NewTile_SettledFromFall;
        tile.GridPositionMoved += NewTile_GridPositionMoved;
        gameTiles.Add(tile);
        if (GridBounds.Contains(newTileBounds))
            AddTileToTileTable(tile);
        if(applyGravity)
            tile.ApplyGravity();
        if (TileAdded != null)
            TileAdded(tile);
        tile.GridPositionMoved += NewTile_GridPositionMoved1;
    }
    //public GameTile AddTileFromToken(Token token, Point point, bool applyGravity = true, bool ignoreChecks = false)
    //{
    //    return AddTileFromToken(token, point.x, point.y, applyGravity, ignoreChecks);
    //}
    //public GameTile AddTileFromToken(Token token, int x, int y, bool applyGravity = true, bool ignoreChecks = false)
    //{
    //    Rectangle newTileBounds = new Rectangle(x, y, token.Width, token.Height);
    //    if (!ignoreChecks)
    //    {
    //        if (!GridBounds.Contains(newTileBounds))
    //        {
    //            Debug.Log("Tile out of bounds");
    //            return null;
    //        }
    //        if (NumberOfTilesInBounds(newTileBounds) > 0)
    //        {
    //            Debug.Log("This tile intersects with another tile");
    //            return null;
    //        }
    //    }
    //    GameTile newTile = GenerateTileFromColoredToken(token);

    //    Vector3 worldPosition = GridPositionToWorldPosition(x, y);
    //    newTile.gameObject.layer = this.gameObject.layer;
    //    newTile.GridPosition = new Point(x, y);
    //    newTile.WorldLeft = worldPosition.x;
    //    newTile.WorldTop = worldPosition.y;
    //    newTile.SettledFromFall += NewTile_SettledFromFall;
    //    newTile.GridPositionMoved += NewTile_GridPositionMoved;
    //    gameTiles.Add(newTile);
    //    if(GridBounds.Contains(newTileBounds))
    //        AddTileToTileTable(newTile);
    //    if (applyGravity)
    //        newTile.ApplyGravity();
    //    if (TileAdded != null)
    //        TileAdded(newTile);
    //    newTile.GridPositionMoved += NewTile_GridPositionMoved1;
    //    return newTile;
    //}
    //private GameTile GenerateTileFromColoredToken(Token token)
    //{
    //    GameObject go = new GameObject();
    //    GameTile result = go.AddComponent<GameTile>();
    //    go.transform.transform.SetParent(this.transform);
    //    result.TokenProperties = token;
    //    return result;
    //}

    private void NewTile_GridPositionMoved(GameTile sender, Rectangle oldGridBounds)
    {
        RemoveBoundsFromTileTable(oldGridBounds, sender);
        if (GridBounds.Contains(sender.GridBounds))
            AddTileToTileTable(sender);
    }
    private void NewTile_SettledFromFall(GameTile sender)
    {

        if (sender.GridTop > Rows)
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
            return _tileTable[x, y];

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
            return x >= 0 && x < Columns && y >= 0 && y < Rows;
        else
            return x >= 0 && x < Columns && y >= 0 && y < Rows;
    }

    public void DestroyTile(GameTile tile, bool triggerEvent = true, bool applyGravity = true)
    {
        if (GridBounds.Contains(tile.GridBounds))
            RemoveBoundsFromTileTable(tile.GridBounds, tile);

        if (applyGravity)
            ApplyGravity();
        gameTiles.Remove(tile);

        if (triggerEvent && TileDestroyed != null)
            TileDestroyed(tile);
        Destroy(tile.gameObject);
    }
    public void DestroyTileAt(int x, int y, bool triggerEvent = true, bool applyGravity = true)
    {
        if (!IsValidTileCoordinate(x, y, true)) return;
        if (GetTileAt(x, y) != null)
            DestroyTile(GetTileAt(x, y), triggerEvent, applyGravity);
    }
    public void DestroyTilesInBounds(Rectangle bounds, bool triggerEvent = true, bool applyGravity = true)
    {
        for (int y = 0; y < bounds.height; y++)
        {
            for (int x = 0; x < bounds.width; x++)
            {
                DestroyTileAt(bounds.x + x, bounds.y + y, triggerEvent, applyGravity);
            }
        }
    }
    public void Clear()
    {
        while (gameTiles.Count > 0)
        {
            DestroyTile(gameTiles[0], false, false);
        }
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                SetBackgroundTileAttribute(x, y, BackgroundTileAttribute.LimitedMove);
            }
        }
    }

    public void MoveWorm(Direction direction)
    {
        int x = 0;
        int y = 0;
        switch (direction)
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
        bool inputValidated = false;
        for (int i = 0; i < gameTiles.Count; i++)
        {
            GameTile tile = gameTiles[i];
            Worm worm = tile.GetComponent<Worm>();
            if (worm != null)
            {
                if (worm.Move(tile.GridPosition.x + x, tile.GridPosition.y + y))
                {
                    inputValidated = true;
                    _movesThisTry++;
                    _totalMoves++;
                }
            }
        }
        if (WormMoveInputRecieved != null)
            WormMoveInputRecieved(direction, inputValidated);
    }

    public void ApplyGravity()
    {
        for (int y = Rows - 1; y >= 0; y--)
        {
            for (int x = 0; x < Columns; x++)
            {
                GameTile tile = GetTileAt(x, y);
                if (tile != null)
                    tile.ApplyGravity();
            }
        }
    }
    private bool BackgroundTileIsMud(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Columns || y >= Rows) return false;
        return _backgroundTileAttributes[x, y] == BackgroundTileAttribute.FreeMove;
    }

    public void SetBackgroundTileAttribute(int x, int y, BackgroundTileAttribute attribute)
    {
        _backgroundTileAttributes[x, y] = attribute;
        if (TileAttributeChanged != null)
            TileAttributeChanged(x, y, attribute);
    }
    public BackgroundTileAttribute GetBackgroundTileAttribute(int x, int y)
    {
        return _backgroundTileAttributes[x, y];
    }
    public BackgroundTileAttribute[,] ExportBackgroundTileAtributes()
    {
        return _backgroundTileAttributes;
    }

    public void Show()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void Hide()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
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

    public enum BackgroundTileAttribute
    {
        LimitedMove,
        FreeMove
    }
}
