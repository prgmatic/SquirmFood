using UnityEngine;
using System.Collections;

public class Utils
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
                _dummySprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(.5f, .5f));
            }
            return _dummySprite;
        }
    }
	
}
