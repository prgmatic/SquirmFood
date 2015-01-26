using UnityEngine;
using System.Collections.Generic;

public class Gameboard : MonoBehaviour
{
    #region Events
    public delegate void GridEvent(int x, int y);
    public event GameTile.GameTileEvent TileSettled;
    public event GameTile.GameTileEvent TileDestroyed;
    #endregion
    #region Variables
    public int Columns = 8;
    public int Rows = 9;
    public Color BackGroundColor1 = new Color(32f / 255, 30f / 255, 24f / 255);
    public Color BackGroundColor2 = new Color(63f / 255, 51f / 255, 47f / 255);

    [SerializeField]
    private TileSet tileSet;
    private GameTile[,] tileTable;
    private int hopperSize = 3;

    [HideInInspector]
    public List<GameTile> gameTiles = new List<GameTile>();
    #endregion

    #region Properties
    public float Left { get { return transform.position.x - (float)Columns / 2f * tileSet.TileWidth; } }
    public float Right { get { return transform.position.x + (float)Columns / 2f * tileSet.TileWidth; } }
    public float Top { get { return transform.position.y + (float)Rows / 2f * tileSet.TileHeight; } }
    public float Bottom { get { return transform.position.y - (float)Rows / 2f * tileSet.TileHeight; } }
    public Rectangle GridBounds { get { return new Rectangle(0, 0, Columns, Rows); } }
    public Rectangle GridBoundsWithHopper { get { return new Rectangle(0, -HopperSize, Columns, Rows + HopperSize); } }

    public float TileWidth { get { return tileSet.TileWidth; } }
    public float TileHeight { get { return tileSet.TileHeight; } }
    public List<TileProperties> Tiles { get { return tileSet.Tiles; } }
    public int HopperSize { get { return hopperSize; } }
    #endregion


    void Awake()
    {
        tileSet.GameboardReference = this;
        CreateBackgroundTiles();
        tileTable = new GameTile[Columns, Rows + hopperSize];

        GameTile[,] test = new GameTile[4, 4];
        var thing = test[1, 1];
        //thing = GenerateTileFromTileProperties(Tiles[0]);
        Debug.Log("hmmm");
    }

    private void FillTileTable()
    {
        BlackTileTable();
        foreach (var tile in gameTiles)
        {
            AddTileToTileTable(tile);
        }
    }
    private void AddTileToTileTable(GameTile tile)
    {
        for (int x = 0; x < tile.Width; x++)
        {
            for (int y = 0; y < tile.Height; y++)
            {
                tileTable[tile.GridLeft + x, tile.GridTop + y + hopperSize] = tile;
            }
        }
    }
    private void BlackTileTable()
    {
        for(int x = 0; x< Columns; x++)
        {
            for(int y = 0; y < Rows; y++)
            {
                tileTable[x, y] = null;
            }
        }
    }
    private void RemoveBoundsFromTileTable(Rectangle bounds, GameTile thisTileOnly = null)
    {
        for (int x = 0; x < bounds.width; x++)
        {
            for (int y = 0; y < bounds.height; y++)
            {
                if(thisTileOnly == null || thisTileOnly == GetTileAt(bounds.x + x, bounds.y + y))
                    tileTable[bounds.x + x, bounds.y + y + hopperSize] = null;
            }
        }
    }

    public void AddTile(int index, int x, int y)
    {
        TileProperties tp = Tiles[index];
        AddTile(tp, x, y);
    }
    public void AddTile(int x, int y)
    {
        AddTile(Tiles[Random.Range(0, Tiles.Count)], x, y);
    }
    public void AddTile(TileProperties tileProperties, int x, int y)
    {
        if(!Tiles.Contains(tileProperties))
        {
            Debug.Log("Tile must belong to set");
            return;
        }

        Rectangle newTileBounds = new Rectangle(x, y, tileProperties.Width, tileProperties.Height);
        if(!GridBoundsWithHopper.Contains(newTileBounds))
        {
            Debug.Log("Tile out of bounds");
            return;
        }
        foreach(var tile in gameTiles)
        {
            if (tile.GridBounds.Intersects(newTileBounds))
            {
                Debug.Log("This tile intersects with another tile");
                return;
            }
        }

        GameTile newTile = GenerateTileFromTileProperties(tileProperties);
        newTile.transform.SetParent(this.transform);

        Vector3 worldPosition = GridPositionToWorldPosition(x, y);
        newTile.GridPosition = new Point(x, y);
        newTile.WorldLeft = worldPosition.x;
        newTile.WorldTop = worldPosition.y;
        newTile.SettledFromFall += NewTile_SettledFromFall;
        newTile.GridPositionMoved += NewTile_GridPositionMoved;
        gameTiles.Add(newTile);
        AddTileToTileTable(newTile);
        newTile.ApplyGravity();
    }

    private void NewTile_GridPositionMoved(GameTile sender, Rectangle oldGridBounds)
    {
        RemoveBoundsFromTileTable(oldGridBounds, sender);
        AddTileToTileTable(sender);
    }
    private void NewTile_SettledFromFall(GameTile sender)
    {
        if (TileSettled != null)
            TileSettled(sender);
    }

    public GameTile GetTileAt(int x, int y)
    {
        return tileTable[x, y + hopperSize];   
    }
    public GameTile[] GetTilesInBounds(Rectangle bounds, GameTile exclusion = null)
    {
        List<GameTile> result = new List<GameTile>();
        for (int x = 0; x < bounds.width; x++)
        {
            for (int y = 0; y < bounds.height; y++)
            {
                GameTile tile = GetTileAt(bounds.x + x, bounds.y + y);
                if (tile != null && tile != exclusion)
                    result.Add(tile);
            }
        }
        return result.ToArray();
    }
    public short NumberOfTilesInBounds(Rectangle bounds, GameTile exclusion = null)
    {
        short result = 0;
        for (int x = 0; x < bounds.width; x++)
        {
            for (int y = 0; y < bounds.height; y++)
            {
                GameTile tile = GetTileAt(bounds.x + x, bounds.y + y); // tileTable[bounds.x + x, bounds.y + y + hopperSize];
                if (tile != null && tile != exclusion)
                    result++;
            }
        }
        return result;
    }

    private GameTile GenerateTileFromTileProperties(TileProperties tileProperties)
    {
        GameTile prefab = tileSet.DefaultPrefab;
        if (tileProperties.Prefab != null)
            prefab = tileProperties.Prefab;
        GameTile result = (GameTile)Instantiate(prefab);

        result.Category = tileProperties.Category;
        result.gameObject.name = tileProperties.Category;
        result.Width = tileProperties.Width;
        result.Height = tileProperties.Height;
        result.Sprite = tileProperties.Sprites[Random.Range(0, tileProperties.Sprites.Count)];
        result.SetGameboard(this);
        return result;
    }

    public Point WorldPositionToGridPosition(Vector3 worldPosition)
    {
        int x = (int)((worldPosition.x - Left) / tileSet.TileWidth);
        int y = -(int)((worldPosition.y - Top) / tileSet.TileHeight);
        return new Point(x, y);
    }
    public Vector3 GridPositionToWorldPosition(int x, int y)
    {
        return new Vector3(
            Left + tileSet.TileWidth * x,
            Top - tileSet.TileHeight * y, 0);
    }
    public bool IsValidTileCoordinate(Point point, bool includeHopper = false)
    {
        return IsValidTileCoordinate(point.x, point.y, includeHopper);
    }
    public bool IsValidTileCoordinate(int x, int y, bool includeHopper = false)
    {
        if (includeHopper)
            return x >= 0 && x < Columns && y >= -hopperSize && y < Rows;
        else
            return x >= 0 && x < Columns && y >= 0 && y < Rows;
    }

    public void DestroyTile(GameTile tile)
    {
        RemoveBoundsFromTileTable(tile.GridBounds, tile);

        for (int i = 0; i < tile.Width; i++)
        {
            ApplyGravitToColumn(tile.GridLeft + i);
        }

        if (TileDestroyed != null)
            TileDestroyed(tile);
        gameTiles.Remove(tile);
        Destroy(tile.gameObject);
    }
    public void DestroyTileAt(int x, int y)
    {
        if(GetTileAt(x, y) != null)
            DestroyTile(GetTileAt(x, y));
    }

    private void ApplyGravitToColumn(int column)
    {
        for(int i = Rows - 1; i >= -HopperSize; i--)
        {
            GameTile tile = GetTileAt(column, i);
            if (tile != null)
                tile.ApplyGravity();
        }
    }

    private void CreateBackgroundTiles()
    {
        GameObject backgroundTiles = new GameObject();
        backgroundTiles.name = "BackgroundTiles";
        backgroundTiles.transform.SetParent(this.transform);
        backgroundTiles.transform.localPosition = new Vector3(0, 0, 0.1f);

        for(int y = 0; y < Rows; y++)
        {
            for(int x = 0; x < Columns; x++)
            {
                GameObject backgroundTile = new GameObject();
                backgroundTile.name = "BackgroundTile";
                backgroundTile.transform.SetParent(backgroundTiles.transform);
                SpriteRenderer bgRenderer = backgroundTile.AddComponent<SpriteRenderer>();
                bgRenderer.sprite = Utils.DummySprite;
                bgRenderer.transform.localScale = new Vector3(tileSet.TileWidth * 100, tileSet.TileHeight * 100, 1);
                bgRenderer.transform.localPosition = new Vector3(
                    Left + x * tileSet.TileWidth + tileSet.TileWidth / 2,
                    Top - y * tileSet.TileHeight - tileSet.TileHeight / 2
                    );
                bgRenderer.color = (x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1) ? BackGroundColor1 : BackGroundColor2;
            }
        }
    }
}
