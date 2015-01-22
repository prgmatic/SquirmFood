using UnityEngine;
using System.Collections.Generic;

public class Gameboard : MonoBehaviour
{
    public int TilesPerRow = 8;
    public int TilesPerComlumn = 9;
    public TileSet TileSet;
    public Color BackGroundColor1 = new Color(32f / 255, 30f / 255, 24f / 255);
    public Color BackGroundColor2 = new Color(63f / 255, 51f / 255, 47f / 255);

    public float Left { get { return transform.position.x - (float)TilesPerRow / 2f * TileSet.TileWidth; } }
    public float Right { get { return transform.position.x + (float)TilesPerRow / 2f * TileSet.TileWidth; } }
    public float Top { get { return transform.position.y + (float)TilesPerComlumn / 2f * TileSet.TileHeight; } }
    public float Bottom { get { return transform.position.y - (float)TilesPerComlumn / 2f * TileSet.TileHeight; } }

    private GameTile[,] tileTable;
    private List<GameTile> gameTiles = new List<GameTile>();
    private RuleSet[] ruleSets;
    

    void Awake()
    {
        tileTable = new GameTile[TilesPerRow, TilesPerComlumn];
        TileSet.GameboardReference = this;
        CreateBackgroundTiles();
        ruleSets = GetComponents<RuleSet>();
    }

    public void AddTile(GameTile tile, int x, int y)
    {
        if (!IsValidTileCoordinate(x, y))
            return;     // If the coordinates are not valid, do nothing
        if (GetTileAt(x, y) != null)                  // If a tile is already in this space, destory the tile
        {
            Destroy(tile.gameObject);
            return;
        }          
        
        tile.transform.SetParent(this.transform);
        tile.transform.localPosition = new Vector3(this.Left + TileSet.TileWidth * x + TileSet.TileWidth / 2, this.Top - TileSet.TileHeight * y - TileSet.TileHeight / 2);
        gameTiles.Add(tile);

        while(y < TilesPerComlumn - 1 && GetTileAt(x, y + 1) == null)
        {
            y++;
        }
        tileTable[x, y] = tile;
        tile.SetTarget(x, y);
        tile.Settled += Tile_Settled;
    }

    private void Tile_Settled(GameTile sender)
    {
        foreach(var ruleset in ruleSets)
        {
            ruleset.OnTileSettled(sender);
        }
    }

    public bool WorldPositionToGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = (int)((worldPosition.x - Left) / TileSet.TileWidth);
        y = -(int)((worldPosition.y - Top) / TileSet.TileHeight);
        return IsValidTileCoordinate(x, y);
    }

    public GameTile GetTileAt(int x, int y)
    {
        return tileTable[x, y];   
    }
    public void MoveTile(int oldX, int oldY, int newX, int newY)
    {
        tileTable[newX, newY] = tileTable[oldX, oldY];
        tileTable[oldX, oldY] = null;
        tileTable[newX, newY].SetTarget(newX, newY);
    }
    public void DestroyTile(GameTile tile)
    {
        DestroyTileAt(tile.X, tile.Y);
    }
    public void DestroyTileAt(int x, int y)
    {
        if (!IsValidTileCoordinate(x, y)) return;
        if (tileTable[x, y] == null) return;

        
        GameTile tile = tileTable[x, y];
        gameTiles.Remove(tile);
        tileTable[x, y] = null;
        Destroy(tile.gameObject);

        int tileCheck = y - 1;
        while(tileCheck >= 0)
        {
            if(tileTable[x, tileCheck] != null)
            {
                MoveTile(x, tileCheck, x, y);
                y--;
            }
            tileCheck--;
        }
    }

    public bool IsValidTileCoordinate(int x, int y)
    {
        return x >= 0 && x < TilesPerRow && y >= 0 && y < TilesPerComlumn;
    }

    private void CreateBackgroundTiles()
    {
        GameObject backgroundTiles = new GameObject();
        backgroundTiles.name = "BackgroundTiles";
        backgroundTiles.transform.SetParent(this.transform);

        for(int y = 0; y < TilesPerComlumn; y++)
        {
            for(int x = 0; x < TilesPerRow; x++)
            {
                GameObject backgroundTile = new GameObject();
                backgroundTile.name = "BackgroundTile";
                backgroundTile.transform.SetParent(backgroundTiles.transform);
                SpriteRenderer bgRenderer = backgroundTile.AddComponent<SpriteRenderer>();
                bgRenderer.sprite = Utils.DummySprite;
                bgRenderer.transform.localScale = new Vector3(TileSet.TileWidth * 100, TileSet.TileHeight * 100, 1);
                bgRenderer.transform.position = new Vector3(
                    Left + x * TileSet.TileWidth + TileSet.TileWidth / 2,
                    Top - y * TileSet.TileHeight - TileSet.TileHeight / 2
                    );
                bgRenderer.color = (x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1) ? BackGroundColor1 : BackGroundColor2;
            }
        }
    }

    

    private GameTile TileAt(int x, int y)
    {
        return gameTiles[x + y * TilesPerRow];
    }

    /*
    private bool CheckForMatch(int x, int y)
    {
        // check vertical
        GameTile currentTile = TileAt(x, y);
        int verticalMatches = 0;
        int checkY = y + 1;
        int checkX = x;
        while (checkY < TilesPerComlumn && TileAt(checkX, checkY) != null && TileAt(checkX, checkY).TileCategory == currentTile.TileCategory) // Check above
        {
            verticalMatches++;
            checkY++;
        }
        checkY = y - 1;
        while (checkY >= 0 && TileAt(checkX, checkY) != null && TileAt(checkX, checkY).TileCategory == currentTile.TileCategory) // Check below
        {
            verticalMatches++;
            checkY--;
        }
        if (verticalMatches > 1)
            return true;

        int horizontalMatches = 0;
        checkY = y;
        checkX = x - 1;

        while (checkX >= 0 && TileAt(checkX, checkY) != null && TileAt(checkX, checkY).TileCategory == currentTile.TileCategory) // Check left
        {
            horizontalMatches++;
            checkX--;
        }
        checkX = x + 1;
        while (checkX < TilesPerRow && TileAt(checkX, checkY) != null && TileAt(checkX, checkY).TileCategory == currentTile.TileCategory) // Check Right
        {
            horizontalMatches++;
            checkX++;
        }
        if (horizontalMatches > 1)
            return true;

        return false;
    }
    */

}
