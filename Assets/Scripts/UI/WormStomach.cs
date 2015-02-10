using UnityEngine;
using System.Collections.Generic;

public class WormStomach : MonoBehaviour 
{
    public static WormStomach Instance { get { return _instance; } }
    private static WormStomach _instance;
    

    public float BarWidth = .5f;

    private List<SpriteRenderer> DisplayedTokens = new List<SpriteRenderer>();
    private SpriteRenderer _renderer;
    private float Width { get { return _renderer.transform.localScale.x; } }
    private float Height { get { return _renderer.transform.localScale.y; } }

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(this.gameObject);
        }
    }

	void Start()
    {
        GameObject go = new GameObject();
        go.name = "Background";
        go.transform.SetParent(this.transform);
        go.transform.localPosition = new Vector3(0, 0, 1);


        _renderer = go.AddComponent<SpriteRenderer>();
        _renderer.sprite = Utils.DummySprite;

        SetBackgroundScale(BarWidth, BarWidth);

        UpdatePosition();

        SetWorm(null);
    }

    private void SetBackgroundScale(float width, float height)
    {
        _renderer.transform.localScale = new Vector3(width, height, 0);
    }

    public void SetWorm(Worm worm)
    {
        ClearDisplayedTokens();
        if(worm == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.gameObject.SetActive(true);
        SetBackgroundScale(BarWidth, BarWidth * worm.StomachSize);
        
        foreach(Token token in worm.Stomach)
        {
            if(token is TexturedToken)
            {
                AddTokenToDisplay((TexturedToken)token);
            }
        }
        UpdatePosition();
    }

    void UpdatePosition()
    {
        this.transform.position = Gameboard.Instance.transform.position +
            new Vector3(Gameboard.Instance.Width / 2 + Width / 2 + 0.2f, -Gameboard.Instance.Height / 2 + Height / 2, 0);
    }


    void AddTokenToDisplay(TexturedToken token)
    {
        GameObject go = new GameObject();
        go.transform.SetParent(this.transform);
        go.name = token.name;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = token.Sprite;
        renderer.color = token.Color;
        renderer.transform.localPosition = new Vector2 (0, -Height / 2 + DisplayedTokens.Count * Width + Width / 2);
        float w = renderer.sprite.bounds.size.x;
        float h = renderer.sprite.bounds.size.y;
        float scale = 1;
        if (w > h)
            scale = BarWidth / w;
        else
            scale = BarWidth / h;
        renderer.transform.localScale = new Vector3(scale, scale, 1);

        DisplayedTokens.Add(renderer);
    }
    void ClearDisplayedTokens()
    {
        while(DisplayedTokens.Count > 0)
        {
            Destroy(DisplayedTokens[0].gameObject);
            DisplayedTokens.RemoveAt(0);
        }
    }
}
