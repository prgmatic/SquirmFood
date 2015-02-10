using UnityEngine;
using System.Collections;

public static class Utils
{
    private static Sprite _dummySprite;
    
    public static Sprite DummySprite
    {
        get
        {
            if(_dummySprite == null)
            {
                Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                _dummySprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(.5f, .5f), 1);
            }
            return _dummySprite;
        }
    }

    public static Point CursorGridPosotion { get { return Gameboard.Instance.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)); } }

    public static float GetWidth(this Sprite sprite)
    {
        return sprite.bounds.size.x;
    }
    public static float GetHeight(this Sprite sprite)
    {
        return sprite.bounds.size.y;
    }
	
}
