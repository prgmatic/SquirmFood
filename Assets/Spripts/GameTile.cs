using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class GameTile : MonoBehaviour
{
    #region EventsHandlers
    public delegate void GameTileEvent(GameTile sender);
    public delegate void GameTileGridMovedEvent(GameTile sender, Rectangle oldGridBounds);
    public event GameTileEvent SettledFromFall;
    public event GameTileEvent SettledFromMove;
    public event GameTileGridMovedEvent GridPositionMoved;
    #endregion

    #region Variables
    private static float MoveTime = .2f;

    public bool FreeFall = true;
    public float Acceleration = 9.87f;
    [HideInInspector]
    public string Category = "";

    private SpriteRenderer _renderer;
    private Gameboard gameboard = null;
    private Vector3 _velocity = Vector3.zero;
    private bool _moving = false;
    private Point _size = new Point(1, 1);
    private Point _gridPosition = Point.zero;
    private Rectangle gravityBounds;
    #endregion

    #region Properties
    public Vector3 WorldPosition { get { return this.transform.position; } set { this.transform.position = value; } }

    public int Width { get { return _size.x; } set { _size.x = value; } }
    public int Height { get { return _size.y; } set { _size.y = value; } }
    public Point Size { get { return _size; } set { _size = value; } }

    public int GridLeft { get { return _gridPosition.x; } }
    public int GridRight { get { return _gridPosition.x + Width; } }
    public int GridTop { get { return _gridPosition.y; } }
    public int GridBottom { get { return _gridPosition.y + Height ; } }
    public Point GridPosition { get { return _gridPosition; } set { _gridPosition = value; } }

    public float WorldLeft
    {
        get { return WorldPosition.x - gameboard.TileWidth / 2 * Width; }
        set { WorldPosition = new Vector3(value + gameboard.TileWidth / 2 * Width, WorldPosition.y, WorldPosition.z); }
    }
    public float WorldRight { get { return WorldPosition.x + gameboard.TileWidth / 2 * Width; } }
    public float WorldTop
    {
        get { return WorldPosition.y + gameboard.TileHeight / 2 * Height; }
        set { WorldPosition = new Vector3(WorldPosition.x, value - gameboard.TileHeight / 2 * Height, WorldPosition.z); }
    }
    public float WorldBottom { get { return WorldPosition.y - gameboard.TileHeight / 2 * Height; } }

    public Rectangle GridBounds { get { return new Rectangle(GridLeft, GridTop, Width, Height);} }

    public bool Moving { get { return _moving; } }
    public Sprite Sprite { get { return _renderer.sprite; } set { _renderer.sprite = value; } }
    #endregion

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void ApplyGravity()
    {
        if (GridBottom < gameboard.Rows)
        {
            gravityBounds = new Rectangle(GridLeft, GridTop + 1, Width, Height);
            while (gravityBounds.Bottom <= gameboard.Rows && gameboard.NumberOfTilesInBounds(gravityBounds, this) == 0)
            {
                gravityBounds.y++;
            }

            
            
            

            if (gravityBounds.y - 1 > GridTop)
            {
                Rectangle oldGridBounds = GridBounds;
                _gridPosition.y = gravityBounds.y - 1;
                if (GridPositionMoved != null)
                    GridPositionMoved(this, oldGridBounds);
                StopCoroutine("FallToTarget");
                StartCoroutine("FallToTarget");
            }

            if (GridBottom == gameboard.Rows && (Width > 1 || Height > 1))
            {
                Rectangle oldGridBounds = GridBounds;
                _gridPosition.y = gameboard.Rows + 10;
                if (GridPositionMoved != null)
                    GridPositionMoved(this, oldGridBounds);
                StopCoroutine("FallToTarget");
                StartCoroutine("FallToTarget");
            }
        }
    }
    public void Move(int x, int y)
    {
        Rectangle oldGridBounds = GridBounds;
        _gridPosition.x = x;
        _gridPosition.y = y;
        if (GridPositionMoved != null)
            GridPositionMoved(this, oldGridBounds);
        StopCoroutine("TransitionToNewGridPosition");
        StartCoroutine("TransitionToNewGridPosition");
    }

    // Helper Methods
    public bool IsCardinalNeighbor(GameTile tile)
    {
        bool linedUpHorizontally = (tile.GridTop >= GridTop && tile.GridTop < GridBottom) || (tile.GridBottom < GridTop && tile.GridBottom >= GridBottom);
        bool linedUpVertically = (tile.GridLeft >= GridLeft && tile.GridLeft < GridRight) || (tile.GridRight < GridLeft && tile.GridRight >= GridRight);

        bool touchingHorizontally = linedUpHorizontally && (GridLeft == tile.GridRight || GridRight == tile.GridLeft);
        bool touchingVertically = linedUpVertically && (GridTop == tile.GridBottom || GridBottom == tile.GridTop);

        return touchingHorizontally || touchingVertically;
    }
    public void SetGameboard(Gameboard gameboard)
    {
        this.gameboard = gameboard;
    }

    // Coroutines
    IEnumerator FallToTarget()
    {
        _moving = true;
        float yTarget = gameboard.GridPositionToWorldPosition(GridLeft, GridTop).y;
        bool reachedTarget = false;
        while(!reachedTarget)
        {
            float deltaTime = Time.deltaTime;
            _velocity.y -= Acceleration * Time.deltaTime * 60;
            WorldTop += _velocity.y * Time.deltaTime;
            if(WorldTop <= yTarget)
            {
                _velocity.y = 0;
                WorldTop = yTarget;
                reachedTarget = true;
            }
            yield return 0;
        }
        _moving = false;
        if(SettledFromFall != null)
            SettledFromFall(this);
    }
    IEnumerator TransitionToNewGridPosition()
    {
        _moving = true;
        float timer = 0f;
        Vector3 startPosition = this.transform.position;
        Vector3 endPosition = gameboard.GridPositionToWorldPosition(GridPosition.x, GridPosition.y);
        endPosition.x += gameboard.TileWidth / 2 * Width;
        endPosition.y -= gameboard.TileHeight / 2 * Height;

        while (timer < MoveTime)
        {
            timer += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, timer * (1f / MoveTime));
            yield return null;
        }
        _moving = false;
        if (SettledFromMove != null)
            SettledFromMove(this);
    }
}
