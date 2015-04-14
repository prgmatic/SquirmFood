using UnityEngine;
using System.Collections.Generic;

public class TexturedToken : ColoredToken 
{
    public Sprite Sprite;
    public bool UseSpriteArray = false;
    public List<Sprite> SpriteArray = new List<Sprite>();
    public Sprite Screen = null;

    public Sprite GetSpriteFromSpriteArray()
    {
        if (SpriteArray.Count == 0) return null;
        int rand = Random.Range(0, SpriteArray.Count);
        return SpriteArray[rand];
    }
}
