using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class GameTile : MonoBehaviour
{
    public bool FreeFall = true;
    public float Acceleration = 9.87f;

    private int targetX = 0;
    private int targetY = 0;
    private SpriteRenderer _renderer;
    private Gameboard gameboard = null;
    private Vector3 _velocity = Vector3.zero;
    

    private static float MoveTime = .2f;
    [HideInInspector]
    public string Category = "";

    public int X { get { return (int)((transform.position.x - gameboard.Left) / gameboard.TileSet.TileWidth); } }
    public int Y { get { return -(int)((transform.position.y - gameboard.Top) / gameboard.TileSet.TileWidth); } }
    public float Top
    {
        get { return transform.position.y + gameboard.TileSet.TileHeight / 2; }
        set
        {
            Vector3 pos = transform.position;
            pos.y = value - gameboard.TileSet.TileHeight / 2;
            transform.position = pos;
        }
    }
    public float Bottom
    {
        get { return transform.position.y - gameboard.TileSet.TileHeight / 2; }
        set
        {
            Vector3 pos = transform.position;
            pos.y = value + gameboard.TileSet.TileHeight / 2;
            transform.position = pos;
        }
    }

    public GameTile TileBelow
    {
        get
        {
            int x = X;
            int y = Y + 1;
            while(y < gameboard.TilesPerComlumn)
            {
                GameTile tile = gameboard.GetTileAt(x, y);
                if (tile != null)
                    return tile;
                y++;
            }
            return null;
        }
    }
    public bool AtBottom { get { return Y == gameboard.TilesPerComlumn - 1; } }
    public Color Color
    {
        get { return _renderer.color; }
        set { _renderer.color = value; }
    }


    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        /*
        if(FreeFall)
        {
            GameTile tileBelow = TileBelow;
            _velocity.y -= Acceleration * Time.deltaTime;
            transform.position += _velocity;

            if(tileBelow != null)
            {
                if(Bottom < tileBelow.Top)
                {
                    Bottom = tileBelow.Top;
                    _velocity.y = 0;
                }
            }
            else if(Bottom < gameboard.Bottom)
            {
                Bottom = gameboard.Bottom;
                _velocity.y = 0;
            }
        }
        */
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
    public void SetTarget(int x, int y)
    {
        targetX = x;
        targetY = y;
        StopCoroutine("FallToTarget");
        StartCoroutine("FallToTarget");
    }

    IEnumerator FallToTarget()
    {
        float yTarget = gameboard.Top - gameboard.TileSet.TileHeight * targetY - gameboard.TileSet.TileHeight / 2;
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
    }
    IEnumerator Move(Vector3 endPosition)
    {
        float timer = 0f;
        Vector3 startPosition = this.transform.position;

        while (timer < MoveTime)
        {
            timer += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, timer * (1f / MoveTime));
            yield return null;
        }
    }
}
