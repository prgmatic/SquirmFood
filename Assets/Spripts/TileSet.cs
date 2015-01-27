using UnityEngine;
using System.Collections.Generic;

public class TileSet : ScriptableObject {

    public float TileWidth = 2.3f;
    public float TileHeight = 2.3f;
    public GameTile DefaultPrefab;
    public List<TileProperties> Tiles;

    [HideInInspector]
    public Gameboard GameboardReference;

    public TileProperties GetTileFromWeightedValues()
    {
        return Tiles[GetTIleIndexFromWeightedValues()];
    }
    public int GetTIleIndexFromWeightedValues()
    {
        int randMax = 0;
        foreach(var tile in Tiles)
        {
            randMax += tile.SpawnWeight;
        }
        int rand = Random.Range(0, randMax);

        int weightValue = 0;
        for(int i = 0;i < Tiles.Count; i++)
        {
            TileProperties tile = Tiles[i];
            if(rand >= weightValue && rand < weightValue + tile.SpawnWeight)
            {
                return i;
            }
            else
            {
                weightValue += tile.SpawnWeight;
            }
        }
        return -1;
    }

}

[System.Serializable]
public class TileProperties
{
    public string Category;
    public int Width = 1;
    public int Height = 1;
    [Range(0, 100)]
    public int SpawnWeight = 50;
    public GameTile Prefab;
    public List<Sprite> Sprites;

    public Rectangle Bounds(int x, int y)
    {
        return new Rectangle(x, y, Width, Height);
    }
}
