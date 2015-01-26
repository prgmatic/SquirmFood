using UnityEngine;
using System.Collections.Generic;

public class TileSet : ScriptableObject {

    public float TileWidth = 2.3f;
    public float TileHeight = 2.3f;
    public GameTile DefaultPrefab;
    public List<TileProperties> Tiles;

    [HideInInspector]
    public Gameboard GameboardReference;

}

[System.Serializable]
public class TileProperties
{
    public string Category;
    public int Width = 1;
    public int Height = 1;
    public GameTile Prefab;
    public List<Sprite> Sprites;
}
