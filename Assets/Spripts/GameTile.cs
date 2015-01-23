using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class GameTile : MonoBehaviour
{
    public delegate void GameTileEvent(GameTile sender);
    public event GameTileEvent SettledFromFall;
    public event GameTileEvent SettledFromMove;
    
    [HideInInspector]
    public int Width = 1;
    [HideInInspector]
    public int Height = 1;
    public bool FreeFall = true;
    public float Acceleration = 9.87f;

    private SpriteRenderer _renderer;
    private Gameboard gameboard = null;
    private Vector3 _velocity = Vector3.zero;
    private bool _moving = false;

    private static float MoveTime = .2f;
    [HideInInspector]
    public string Category = "";

    private int x = 0;
    private int y = 0;

    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }
    public bool Moving { get { return _moving; } }
    public Color Color
    {
        get { return _renderer.color; }
        set { _renderer.color = value; }
    }

    public void SetPosition(int x, int y)
    {
        this.X = x;
        this.Y = y;
        Vector2 pos = gameboard.GridPositionToWorldPosition(x, y);
        pos.x += (Width - 1) * gameboard.TileSet.TileWidth / 2;
        pos.y -= (Height - 1) * gameboard.TileSet.TileHeight / 2;
        this.transform.localPosition = pos;
    }

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float yPos = gameboard.TilesPerComlumn;
        for(int column = 0; column < Width; column++)
        {
            
            int y = Y + Height - 1;
            while (y + 1 < gameboard.TilesPerComlumn && gameboard.GetTileAt(X + column, y + 1) == null)
            {
                y++;
            }
            if (y < yPos) yPos = y;
        }
        if(yPos > Y + Height - 1)
        {
            gameboard.MoveTile(this, GridSpacesOccupied(), GridSpacesAfterApplyingGravity());
            Fall();
        }
    }

    
    public Point[] GridSpacesOccupied()
    {
        Point[] result = new Point[Width * Height];
        for(int i = 0; i < result.Length; i++)
        {
            result[i] = new Point(X + i % Width, Y + i / Width);
        }
        return result;
    }
    
    public Point[] GridSpacesAfterApplyingGravity()
    {
        int highestYPos = gameboard.TilesPerComlumn;

        for(int column = 0; column < Width; column ++)
        {
            int y = Y + Height - 1;
            while(y + 1 < gameboard.TilesPerComlumn && gameboard.GetTileAt(X + column, y + 1) == null )
            {
                y++;
            }
            if (y < highestYPos) highestYPos = y;
        }

        Point[] result = new Point[Width * Height];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Point(X + i % Width, highestYPos - Height + 1 + i / Width);
        }
        return result;
    }







    public void MoveTo(Vector3 endPosition, bool animate = true)
    {
        StartCoroutine("Move", endPosition);
    }

    

    public void SetGameboard(Gameboard gameboard)
    {
        this.gameboard = gameboard;
    }
    public void SetSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }
    public void Fall()
    {
        StopCoroutine("FallToTarget");
        StartCoroutine("FallToTarget");
    }
    public bool IsCardinalNeighbor(GameTile tile)
    {
        if( (tile.X == X - 1 && tile.Y == Y) ||
            (tile.X == X + 1 && tile.Y == Y) ||
            (tile.X == X && tile.Y == Y - 1) ||
            (tile.X == X && tile.Y == Y + 1))
        {
            return true;
        }
        return false;
    }
    public void UpdatePosition(bool animate = true)
    {
        StopCoroutine("Move");
        Vector3 targetPosition = gameboard.GridPositionToWorldPosition(X, Y);
        if (animate)
        {
            StartCoroutine("Move", targetPosition);
        }
        else
        {
            this.transform.position = targetPosition;
        }
    }

    IEnumerator FallToTarget()
    {
        _moving = true;
        float yTarget = gameboard.Top - gameboard.TileSet.TileHeight * Y - gameboard.TileSet.TileHeight / 2 * Height;
        bool reachedTarget = false;
        while(!reachedTarget)
        {
            _velocity.y -= Acceleration * Time.deltaTime;
            this.transform.position += _velocity;
            if(this.transform.position.y <= yTarget)
            {
                _velocity.y = 0;
                Vector3 pos = this.transform.position;
                pos.y = yTarget;
                this.transform.position = pos;
                reachedTarget = true;
            }
            yield return null;
        }
        _moving = false;
        if(SettledFromFall != null)
            SettledFromFall(this);
    }
    IEnumerator Move(Vector3 endPosition)
    {
        _moving = true;
        float timer = 0f;
        Vector3 startPosition = this.transform.position;

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
