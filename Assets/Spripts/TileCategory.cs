using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileCategorySet
{

    public TileCategoryType TileCategory;
    public bool Active = true;
    public Color Color = Color.white;
    public List<Sprite> Spites;
}

public enum TileCategoryType
{
    Bolt,
    Bug,
    Bulb,
    Coin,
    Egg,
    Eye,
    Jewelry,
    Larva,
    Rock,
    Seed,
    Tooth,
    Worm
}
