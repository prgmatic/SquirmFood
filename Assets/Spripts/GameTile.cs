using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class GameTile : MonoBehaviour
{
    public delegate void GameTileEvent(GameTile sender);
    public event GameTileEvent Settled;

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

    public int X { get { return targetX; } }
    public int Y { get { return targetY; } }
    public Color Color
    {
        get { return _renderer.color; }
        set { _renderer.color = value; }
    }


    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
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
        if(Settled != null)
        {
            Settled(this);
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
