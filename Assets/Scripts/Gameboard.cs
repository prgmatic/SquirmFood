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
    #endregion

    #region Variables
    public bool DoKeyValidation = true;
    public bool ShowLevelSelectionScreenOnStartup = true;
    public int Columns = 8;
    public int Rows = 9;
    public bool HopperMask = true;
    public Sprite BackgroundTile1;
    public Sprite BackgroundTile2;

    public Sprite MudTileFill;
    public Sprite MudTileInsideCorner;
    public Sprite MudTileLongEdge;
    public Sprite MudTileOutsideCorner;
    public Sprite MudTileSwoopOut;
    public Sprite MudTileSwoopOut2;
    public Material MultiplyMaterial;

    private GameTile[,] _tileTable;
    private SpriteRenderer[,] _backgroundTiles;
    private BackgroundTileAttribute[,] _backgroundTileAttributes;
    private int _hopperSize = 3;
    private static Gameboard _instance;
    private float _gameDuration = 0f;
    private int _totalMoves = 0;
    private int _movesThisTry = 0;
    private int _retries = 0;
    private Playthrough _playbackData = null;
    private GameObject _bgTileGroup;
    private SpriteRenderer[,] _mudTiles;
    private GameObject _mudTileGroup;
    private BoardLayout _currentLevel;

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
    public Rectangle GridBoundsWithHopper { get { return new Rectangle(0, -HopperSize, Columns, Rows + HopperSize); } }
    public int HopperSize { get { return _hopperSize; } }
    public bool AcceptingInput { get { return GameState == GameStateType.InProgress; } }
    public float GameDuration { get { return _gameDuration; } }
    public int TotalMoves { get { return _totalMoves; } }
    public int MovesThisTry { get { return _movesThisTry; } }
    public int Retries { get { return _retries; } }
    public BoardLayout CurrentLevel {
        get { return _currentLevel; }
        set { _currentLevel = value; } }
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
            return;
        }
    }
    private void Init()
    {
        _backgroundTiles = new SpriteRenderer[Columns, Rows];
        _backgroundTileAttributes = new BackgroundTileAttribute[Columns, Rows];
        CreateBackgroundTiles();
        CreateMudTiles();
        _tileTable = new GameTile[Columns, Rows + _hopperSize];
        if (HopperMask)
            CreateHopperMask();

    }
    void Start()
    {
        /*
        UIGlobals.Instance.HideAll();
        if (DoKeyValidation)
        {
            Hide();
            if (RequestParameters.IsInitialized)
                CheckValidation();
            else
                RequestParameters.Initialized += RequestParameters_Initialized;
        }
        else
        {
            Startup();
        }
        DebugHUD.MessagesCleared += delegate
        {
            DebugHUD.Add("Moves: " + _movesThisTry);
        };
        */
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
        VictoryLossConditions.Instance.Disable();
    }
    private void ResetBoard()
    {
        if (GameboardReset != null)
            GameboardReset();
        if (CurrentLevel != null)
            BoardLayoutImporter.ImportBoardLayout(CurrentLevel);
        
        ResourcePool.Instance.UpdateResourceCount();
        VictoryLossConditions.Instance.Enable();
        UpdateMudTiles();
    }
    public void AdvanceLevel()
    {
        BoardLayoutSet bls = GetComponent<BoardLayoutSet>();
        if(bls != null)
        {
            bls.NextLevel();
        }
        //StartGame();
    }
    public void Retry()
    {
        _retries++;
        _movesThisTry = 0;
        ResetBoard();
        if (GameRetry != null)
            GameRetry();
    }

    /*
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
    */

    public void SetBoardSize(int columns, int rows)
    {
        Clear();
        this.Columns = columns;
        this.Rows = rows;

        _backgroundTiles = new SpriteRenderer[Columns, Rows];
        _backgroundTileAttributes = new BackgroundTileAttribute[Columns, Rows];
        CreateBackgroundTiles();
        _tileTable = new GameTile[Columns, Rows + _hopperSize];
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
        GameTile result = go.AddComponent<GameTile>();
        go.transform.transform.SetParent(this.transform);
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
            DestroyTile(gameTiles[0], false, false);
        }
        for(int y = 0; y < Rows; y++)
        {
            for(int x = 0; x< Columns; x++)
            {
                SetBackgroundTileAttribute(x, y, BackgroundTileAttribute.LimitedMove);
            }
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
        bool inputValidated = false;
        for (int i = 0; i < gameTiles.Count; i++)
        {
            GameTile tile = gameTiles[i];
            Worm worm = tile.GetComponent<Worm>();
            if (worm != null)
            {
                if (worm.Move(worm.Head.GridPosition.x + x, worm.Head.GridPosition.y + y))
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
        if (_bgTileGroup != null)
            Destroy(_bgTileGroup);

        _bgTileGroup = new GameObject();
        _bgTileGroup.name = "BackgroundTiles";
        _bgTileGroup.transform.SetParent(this.transform);
        _bgTileGroup.transform.localPosition = new Vector3(0, 0, 0.1f);

        float scaleF = 1f / BackgroundTile1.bounds.size.x;
        Vector3 scale = new Vector3(scaleF, scaleF, 1f);

        for(int y = 0; y < Rows; y++)
        {
            for(int x = 0; x < Columns; x++)
            {
                GameObject backgroundTile = new GameObject();
                backgroundTile.name = "BackgroundTile";
                backgroundTile.transform.SetParent(_bgTileGroup.transform);
                SpriteRenderer bgRenderer = backgroundTile.AddComponent<SpriteRenderer>();
                bgRenderer.sprite = (x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1) ? BackgroundTile1 : BackgroundTile2;
                //bgRenderer.transform.localScale = new Vector3(tileSet.TileWidth * 100, tileSet.TileHeight * 100, 1);
                bgRenderer.transform.localPosition = new Vector3(
                    Left + x,// - 0.5f,
                    Top - y// - 0.5f
                    );
                bgRenderer.transform.localScale = scale;
                _backgroundTiles[x, y] = bgRenderer;
            }
        }
    }
    private void CreateMudTiles()
    {
        if (_mudTileGroup != null)
            Destroy(_mudTileGroup);

        _mudTileGroup = new GameObject();
        _mudTileGroup.name = "MudTiles";
        _mudTileGroup.transform.SetParent(this.transform);
        _mudTileGroup.transform.localPosition = new Vector3(0, 0, 0.05f);


        float tileWidth = MudTileFill.bounds.size.x;
        float scaleF = 1f / tileWidth * 0.5f;

        Vector3 scale = new Vector3(scaleF, scaleF, 1f);
        _mudTiles = new SpriteRenderer[Columns * 2, Rows * 2];

        for (int y = 0; y < Rows * 2; y++)
        {
            for(int x = 0; x < Columns * 2; x++)
            {
                GameObject mudTile = new GameObject();
                mudTile.name = "MudTile";
                mudTile.transform.SetParent(_mudTileGroup.transform);
                SpriteRenderer mudRenderer = mudTile.AddComponent<SpriteRenderer>();
                mudRenderer.material = MultiplyMaterial;
                //mudRenderer.sprite = MudTileFill;

                mudRenderer.transform.localPosition = new Vector3(
                    Left + x * 0.5f + 0.25f,
                    Top - y * 0.5f - 0.25f
                    );
                mudRenderer.transform.localScale = scale;
                _mudTiles[x, y] = mudRenderer;

                int tx = x % 2;
                int ty = y % 2;

                if (tx == 1 && ty == 0) mudRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
                else if(tx == 0 && ty == 1) mudRenderer.transform.localRotation = Quaternion.Euler(0, 0, 90);
                else if(tx == 1 && ty == 1) mudRenderer.transform.localRotation = Quaternion.Euler(0, 0, 180);
            }
        }

    }
    private void UpdateMudTiles()
    {
        for(int y = 0; y < Rows; y++)
        {
            for(int x = 0; x < Columns; x++)
            {
                UpdateMudTile(x, y);
            }
        }
    }

    private void UpdateMudTile(int x, int y)
    {
        SpriteRenderer topLeft     = _mudTiles[x * 2, y * 2];
        SpriteRenderer topRight    = _mudTiles[x * 2 + 1, y * 2];
        SpriteRenderer bottomLeft  = _mudTiles[x * 2, y * 2 + 1];
        SpriteRenderer bottomRight = _mudTiles[x * 2 + 1, y * 2 + 1];

        bool nAbove      = BackgroundTileIsMud(x, y - 1);
        bool nBelow      = BackgroundTileIsMud(x, y + 1);
        bool nAboveLeft  = BackgroundTileIsMud(x - 1, y - 1);
        bool nLeft       = BackgroundTileIsMud(x - 1, y);
        bool nBelowLeft  = BackgroundTileIsMud(x - 1, y + 1);
        bool nAboveRight = BackgroundTileIsMud(x + 1, y - 1);
        bool nRight      = BackgroundTileIsMud(x + 1, y);
        bool nBelowRight = BackgroundTileIsMud(x + 1, y + 1);

        bool self = _backgroundTileAttributes[x, y] == BackgroundTileAttribute.FreeMove;

        SetMudSprite(topLeft, 0, self, nLeft, nAbove, nAboveLeft);
        SetMudSprite(topRight, -90, self, nRight, nAbove, nAboveRight);
        SetMudSprite(bottomLeft, 90, self, nLeft, nBelow, nBelowLeft);
        SetMudSprite(bottomRight, 180, self, nRight, nBelow, nBelowRight);
    }

    private void SetMudSprite(SpriteRenderer renderer, int rotation, bool self, bool horizontalNeighbor, bool verticalNeightbor, bool cornerNeighbor)
    {
        var scale = renderer.transform.localScale;
        scale.y = Mathf.Abs(scale.y);
        renderer.transform.localScale = scale;
        if (self)
        {
            if (horizontalNeighbor && verticalNeightbor)
            {
                if (cornerNeighbor)
                    renderer.sprite = MudTileFill;
                else renderer.sprite = MudTileSwoopOut2;
            }
            else if((horizontalNeighbor || verticalNeightbor) && cornerNeighbor)
            {
                renderer.sprite = MudTileSwoopOut;
                if (horizontalNeighbor)
                {
                    if (rotation == 90 || rotation == -90)
                    {
                        rotation += 90;
                    }
                    else
                    {
                        rotation += 180;
                        scale.y = -scale.y;
                        renderer.transform.localScale = scale;
                    }
                }
                else
                {
                    if (rotation == 0 || rotation == 180)
                        rotation += 90;
                    else
                    {
                        rotation += 180;
                        scale.y = -scale.y;
                        renderer.transform.localScale = scale;
                    }
                }
            }
            else if (horizontalNeighbor && !verticalNeightbor) // horizontal long edge
            {
                    renderer.sprite = MudTileLongEdge;
                if (rotation == 90f || rotation == -90f)
                {
                    rotation += 90;
                }
            }
            else if (!horizontalNeighbor && verticalNeightbor)
            {
                    renderer.sprite = MudTileLongEdge;
                if (rotation != 90f && rotation != -90f)
                {
                    rotation += 90;
                }
            }
            else renderer.sprite = MudTileOutsideCorner;
        }
        else
        {
            if (horizontalNeighbor && verticalNeightbor && cornerNeighbor)
            {
                renderer.sprite = MudTileInsideCorner;
                rotation += 180;
            }
            else renderer.sprite = null;
        }
        renderer.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        //return null;
    }

    private bool BackgroundTileIsMud(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Columns || y >= Rows) return false;
        return _backgroundTileAttributes[x, y] == BackgroundTileAttribute.FreeMove; 
    }

    public void SetBackgroundTileAttribute(int x, int y, BackgroundTileAttribute attribute)
    {
        _backgroundTileAttributes[x, y] = attribute;
        //_backgroundTiles[x, y].color = GetBackgroundTileColor(x, y);
        
        UpdateMudTiles();
    }
    public BackgroundTileAttribute GetBackgroundTileAttribute(int x, int y)
    {
        return _backgroundTileAttributes[x, y];
    }
    public BackgroundTileAttribute[,] ExportBackgroundTileAtributes()
    {
        return _backgroundTileAttributes;
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

    public void Show()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void Hide()
    {
        for(int i = 0; i < this.transform.childCount; i++)
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
