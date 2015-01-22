using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class GameTile : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private bool _selected;
    private Gameboard gameboard = null;
    

    private static float MoveTime = .2f;
    public TileCategoryType TileCategory;

    int Width = 1;
    int Height = 1;

    private bool Selected
    {
        get { return _selected; }
    }

    public Color Color
    {
        get { return _renderer.color; }
        set { _renderer.color = value; }
    }

    public void MoveTo(Vector3 endPosition, bool animate = true)
    {
        StartCoroutine("Move", endPosition);
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

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Select()
    {
        _selected = true;
    }

    public void UnSelect()
    {
        _selected = false;
    }

    public void SetGameboard(Gameboard gameboard)
    {
        this.gameboard = gameboard;
    }
    public void SetSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }
}
