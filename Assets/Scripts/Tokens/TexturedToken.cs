using UnityEngine;
using System.Collections.Generic;

public class TexturedToken : ColoredToken 
{
    public Sprite Sprite;
    public bool UseSpriteArray = false;
    public List<Sprite> SpriteArray = new List<Sprite>();
    public Sprite Screen = null;

    public Sprite GetSpriteFromSpriteArray(out int index)
    {
		index = 0;
        if (SpriteArray.Count == 0) return null;
        int rand = Random.Range(0, SpriteArray.Count);
		index = rand;
        return SpriteArray[rand];
    }
}
