using UnityEngine;

public class IndicatorLightController : MonoBehaviour
{
    public enum IndicatorLightColor
    {
        Green,
        Red,
        Yellow
    }

    public Sprite Green;
    public Sprite Yellow;
    public Sprite Red;

    public IndicatorLightColor Color
    {
        get { return _color; }
        set
        {
            _color = value;
            switch (_color)
            {
                case IndicatorLightColor.Green:
                    _spriteRenderer.sprite = Green;
                    break;
                case IndicatorLightColor.Yellow:
                    _spriteRenderer.sprite = Yellow;
                    break;
                case IndicatorLightColor.Red:
                    _spriteRenderer.sprite = Red;
                    break;
            }
        }
    }

    private IndicatorLightColor _color = IndicatorLightColor.Green;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var state = GameManager.Instance.State;
        if(state == GameManager.GameState.PlayingGame)
        {
            Color = IndicatorLightColor.Yellow;
        }
        else if(state == GameManager.GameState.PostLevel)
        {
            Color = IndicatorLightColor.Green;
        }
        else
        {
            Color = IndicatorLightColor.Red;
        }
    }
}