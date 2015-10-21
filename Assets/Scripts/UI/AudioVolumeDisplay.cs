using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioVolumeDisplay : MonoBehaviour
{
    public List<Sprite> NumberSprites = new List<Sprite>();

    private Image _image;

    public void SetSprite(byte spriteNumber)
    {
        if (spriteNumber >= 0 && spriteNumber < NumberSprites.Count)
            _image.sprite = NumberSprites[spriteNumber];
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
}
