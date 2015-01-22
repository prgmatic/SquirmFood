using UnityEngine;
using System.Collections.Generic;

public class TileSet : ScriptableObject {

    public float TileWidth = 2.3f;
    public float TileHeight = 2.3f;
    public GameTile DefaultPrefab;
    public List<TileProperties> Tiles;

    [HideInInspector]
    public Gameboard GameboardReference;

    

    public GameTile CreateTile(TileProperties tile)
    {
        int index = Tiles.IndexOf(tile);
        if(index != -1)
        {
            return CreateTile(index);
        }
        else
        {
            Debug.Log("Tile must belong to this set to create");
        }
        return null;
    }

    public GameTile CreateTile(int index)
    {
        TileProperties tp = Tiles[index];
        GameTile prefab = DefaultPrefab;
        if (tp.Prefab != null)
            prefab = tp.Prefab;
        GameTile result = (GameTile)Instantiate(prefab);
        result.SetSprite(tp.Sprites[Random.Range(0, tp.Sprites.Count)]);
        result.gameObject.AddComponent<BoxCollider2D>().size = new Vector2(TileWidth, TileHeight);

        result.Category = tp.Category;
        result.gameObject.name = tp.Category;
        result.SetGameboard(this.GameboardReference);
        return result;
    }

    public void CreateTile(int index, int x, int y)
    {
        GameboardReference.AddTile(CreateTile(index), x, y);
    }
}

[System.Serializable]
public class TileProperties
{
    public string Category;
    public GameTile Prefab;
    public List<Sprite> Sprites;
}
